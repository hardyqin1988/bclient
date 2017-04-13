using Firefly.Core.Config.INI;
using Firefly.Core.Data;
using Firefly.Core.Utility;
using Firefly.Core.Variant;
using Firefly.Network;
using Firefly.Network.Opcode;
using Firefly.Unity.Utility;
using Lidgren.Network;
using System;
using System.Collections.Generic;

namespace Firefly.Unity.App
{
    public class NetworkApp : AppBase
    {
        private string             _Address;
        private int                _Port;
        private string             _Ident;
        private NetClient          _Client;
        private NetConnection      _Connection;
        private Queue<MsgSender>   _SendMessages;

        public int Ping
        {
            get
            {
                return (int)(_Connection.AverageRoundtripTime * Constant.SECOND);
            }
        }

        public bool Connected
        {
            get
            {
                return _Connection.Status == NetConnectionStatus.Connected;
            }
        }

        public override void OnAwake(Configuration app_ini)
        {
            try
            {
                _Address = NetUtil.Resolve(app_ini["proxy_net"]["address"].StringValue);
                _Port = app_ini["proxy_net"]["port"].IntValue;
                _Ident = app_ini["proxy_net"]["ident"].StringValue;
            }
            catch (Exception ex)
            {
                throw new INIException("NetworkApp read ini fail", ex);
            }

            _SendMessages = new Queue<MsgSender>();

            NetPeerConfiguration config = new NetPeerConfiguration(_Ident);
#if UNITY_EDITOR
            config.ConnectionTimeout = 60;
#endif

            config.EnableMessageType(NetIncomingMessageType.ConnectionLatencyUpdated);
            _Client = new NetClient(config);
            _Client.Start();
        }

        public override void OnClose()
        {
            _Client.Disconnect("LidgrenNetworkApp shutdown");
        }

        public override void OnOpen()
        {
            _Connection = _Client.Connect(_Address, _Port);
        }

        private void ProcessMessage()
        {
            NetIncomingMessage msg = null;
            while ((_Client != null) && (msg = _Client.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        string text = msg.ReadString();
                        Logger.Trace(text);
                        break;

                    case NetIncomingMessageType.StatusChanged:
                        NetConnectionStatus status = (NetConnectionStatus)msg.ReadByte();
                        Logger.Debug("Connect {0} Status Changed, Now is {1}", msg.SenderEndPoint.ToString(), status);
                        break;

                    case NetIncomingMessageType.Data:
                        if (msg.LengthBytes > 0)
                        {
                            int length = msg.ReadInt32();
                            byte[] msg_data = msg.ReadBytes(length);

                            VariantList recv_msg = DataUtil.BytesToObject<VariantList>(msg_data);
                            int opcode = recv_msg.SubtractInt();
                            Logger.Info("DispatchMessage msg={0}, data={1}", OpcodeUtil.ToName<SystemOpcode.SERVER>(opcode), recv_msg);
                            AppEngine.Instance.Dispatch.DispatchMessage(opcode, recv_msg);
                        }
                        else
                        {
                            Logger.Warn("Empty NetIncomingMessageType.Data message from {0}", msg.SenderEndPoint);
                        }

                        break;

                    default:
                        Logger.Warn("Unhandled message, type: {0}, {1} bytes.", msg.MessageType, msg.LengthBytes);
                        break;
                }
            }
        }

        public override void OnLoop()
        {
            if (_SendMessages == null)
                return;

            ProcessMessage();

            while (_SendMessages.Count > 0 && _Client.ConnectionStatus == NetConnectionStatus.Connected /*&& Connected*/)
            {
                MsgSender client_msg = _SendMessages.Dequeue();

                Logger.Info("SendMsgImmediate msg={0}, data={1}", OpcodeUtil.ToName<SystemOpcode.CLIENT>(client_msg.Opcode), client_msg.Msg);
                SendSystemMsg(VariantList.New().Append(client_msg.Opcode).Append(client_msg.Msg));
            }
        }

        public void SendBattleMsg(VariantList send_msg)
        {
            byte[] send_bytes = DataUtil.ObjectToBytes(VariantList.New().Append(send_msg));

            NetOutgoingMessage om = _Client.CreateMessage();
            om.Write(send_bytes.Length);
            om.Write(send_bytes);

            _Client.SendMessage(om, NetDeliveryMethod.ReliableUnordered);

            send_msg.Recycle();
        }

        private void SendSystemMsg(VariantList send_msg)
        {
            byte[] send_bytes = DataUtil.ObjectToBytes(send_msg);

            NetOutgoingMessage om = _Client.CreateMessage();
            om.Write(send_bytes.Length);
            om.Write(send_bytes);

            _Client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);

            send_msg.Recycle();
        }

        public MsgSender SendSystemMsg(int opcode)
        {
            MsgSender handler = MsgSender.New(opcode);
            _SendMessages.Enqueue(handler);

            return handler;
        }

        public MsgSender SendCustomMsg(int custom)
        {
            MsgSender handler = MsgSender.New(SystemOpcode.CLIENT.CUSTOM_MSG_REQUEST);
            handler.Msg.Append(custom);

            _SendMessages.Enqueue(handler);

            return handler;
        }
        
    }
}
