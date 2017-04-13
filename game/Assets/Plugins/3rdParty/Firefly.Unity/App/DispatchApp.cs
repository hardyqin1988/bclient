using Firefly.Core.Config;
using Firefly.Core.Config.INI;
using Firefly.Core.Data;
using Firefly.Core.Variant;
using Firefly.Network.Opcode;
using Firefly.Unity.Databind;
using Firefly.Unity.Global;
using Firefly.Unity.Kernel;
using Firefly.Unity.Localization;
using Firefly.Unity.Native;
using System;
using System.Collections.Generic;

namespace Firefly.Unity.App
{
    public class DispatchApp : AppBase
    {
        private Dictionary<int, Action<VariantList>> _MessageHandlers = null;

        private CKernel Kernel { get { return KernelEngine.Instance.Kernel; } }

        public override void OnAwake(Configuration app_ini)
        {
            _MessageHandlers = new Dictionary<int, Action<VariantList>>();
        }

        public override void OnClose()
        {
        }

        public override void OnOpen()
        {
            RegisterMessageHandler(SystemOpcode.SERVER.SHAKE_HANDS,             OnRecvMsg_ShakeHands);
            RegisterMessageHandler(SystemOpcode.SERVER.LOGIN_VALIDATE_RESPONSE, OnRecvMsg_LoginResult);
            RegisterMessageHandler(SystemOpcode.SERVER.INIT_ROLE,               OnRecvMsg_InitRole);
            RegisterMessageHandler(SystemOpcode.SERVER.PROPERTY_CHANGED,        OnRecvMsg_PropertyChanged);
            RegisterMessageHandler(SystemOpcode.SERVER.RECORD_CHANGED,          OnRecvMsg_RecordChanged);
            RegisterMessageHandler(SystemOpcode.SERVER.SYSTEM_INFO,             OnRecvMsg_SystemInfo);
            RegisterMessageHandler(SystemOpcode.SERVER.CUSTOM_MSG_RESPONSE,     OnRecvMsg_Custom);
            RegisterMessageHandler(SystemOpcode.SERVER.ADD_ENTITY,              OnRecvMsg_AddEntity);
        }

        private void OnRecvMsg_ShakeHands(VariantList recv_msg)
        {
            string session_key = recv_msg.SubtractString();

            if (!NativeEngine.Instance.SDKLoginSuccess)
            {
                AppEngine.Instance.Userinfo.SessionKey = session_key;
                AppEngine.Instance.Login();
            }
            else
            {
                PersistID role_id = KernelEngine.Instance.MainRole;
                string sessionkey = AppEngine.Instance.Userinfo.SessionKey;
                LogAssert.Util.Trace("session_key:{0} " + sessionkey);
            }
        }
        
        private void OnRecvMsg_LoginResult(VariantList recv_msg)
        {
            bool result = recv_msg.SubtractBool();
            if (result)
            {
                AppEngine.Instance.Network.SendSystemMsg(SystemOpcode.CLIENT.SELECT_ROLE);
            }
        }

        private void OnRecvMsg_AddEntity(VariantList recv_msg)
        {
            PersistID role_id = recv_msg.SubtractPid();
            Kernel.LoadEntity(role_id, recv_msg);
        }

        private void OnRecvMsg_Custom(VariantList recv_msg)
        {
            int custom_id = recv_msg.SubtractInt();
            DatabindEngine.Instance.CallCustomMsg(custom_id, recv_msg);
        }

        private void OnRecvMsg_SystemInfo(VariantList recv_msg)
        {
            string info = recv_msg.SubtractString();
            string text = "";
            if (recv_msg.Count == 0)
            {
                text = L10NEngine.Instance.GetText(info);
            }
            else
            {
                object[] args = new object[recv_msg.Count];
                for (int i = 0; i < recv_msg.Count; i++)
                {
                    VariantType type = recv_msg[i].Type;
                    switch (type)
                    {
                        case VariantType.Bool:
                            args[i] = recv_msg.BoolAt(i);
                            break;
                        case VariantType.Byte:
                            args[i] = recv_msg.ByteAt(i);
                            break;
                        case VariantType.Int:
                            args[i] = recv_msg.IntAt(i);
                            break;
                        case VariantType.Float:
                            args[i] = recv_msg.FloatAt(i);
                            break;
                        case VariantType.Long:
                            args[i] = recv_msg.LongAt(i);
                            break;
                        default:
                            args[i] = L10NEngine.Instance.GetText(recv_msg.StringAt(i));
                            break;
                    }
                }
                text = L10NEngine.Instance.GetFormatText(info, args);
            }
        }

        private void OnRecvMsg_RecordChanged(VariantList args)
        {
            PersistID pid = args.SubtractPid();
            string record_name = args.SubtractString();
            RecordEvent record_event = (RecordEvent)args.SubtractByte();

            string type = Kernel.GetType(pid);
            EntityDef entity_def = Kernel.GetEntityDef(type);
            if (entity_def == null)
            {
                return;
            }

            RecordDef record_def = entity_def.GetRecord(record_name);
            if (record_def == null)
            {
                return;
            }

            if (record_event == RecordEvent.AddRow)
            {
                int row = args.SubtractInt();
                Kernel.SetRecordRow(pid, record_name, row, args);
            }
            else if (record_event == RecordEvent.SetRow)
            {
                int row = args.SubtractInt();
                Kernel.SetRecordRow(pid, record_name, row, args);
            }
            else if (record_event == RecordEvent.SetCol)
            {
                int row = args.SubtractInt();
                int col = args.SubtractInt();
                byte record_type = record_def.ColTypes.ByteAt(col);
                switch ((VariantType)record_type)
                {
                    case VariantType.Bool:
                        {
                            bool old_value = args.SubtractBool();
                            bool new_value = args.SubtractBool();
                            Kernel.SetRecordBool(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.Byte:
                        {
                            byte old_value = args.SubtractByte();
                            byte new_value = args.SubtractByte();
                            Kernel.SetRecordByte(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.Int:
                        {
                            int old_value = args.SubtractInt();
                            int new_value = args.SubtractInt();
                            Kernel.SetRecordInt(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.Float:
                        {
                            float old_value = args.SubtractFloat();
                            float new_value = args.SubtractFloat();
                            Kernel.SetRecordFloat(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.Long:
                        {
                            long old_value = args.SubtractLong();
                            long new_value = args.SubtractLong();
                            Kernel.SetRecordLong(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.String:
                        {
                            string old_value = args.SubtractString();
                            string new_value = args.SubtractString();
                            Kernel.SetRecordString(pid, record_name, row, col, new_value);
                        }
                        break;
                    case VariantType.PersistID:
                        {
                            PersistID old_value = args.SubtractPid();
                            PersistID new_value = args.SubtractPid();
                            Kernel.SetRecordPid(pid, record_name, row, col, new_value);
                        }
                        break;
                }
            }
            else if (record_event == RecordEvent.DelRow)
            {
                int row = args.SubtractInt();
                Kernel.DelRecordRow(pid, record_name, row);
            }
            else if (record_event == RecordEvent.Clear)
            {
                Kernel.ClearReocrd(pid, record_name);
            }
        }

        private void OnRecvMsg_PropertyChanged(VariantList recv_msg)
        {
            PersistID pid = recv_msg.SubtractPid();
            string property_name = recv_msg.SubtractString();
            PropertyEvent property_event = (PropertyEvent)recv_msg.SubtractByte();

            string type = Kernel.GetType(pid);
            EntityDef entity_def = Kernel.GetEntityDef(type);
            if (entity_def == null)
            {
                return;
            }

            PropertyDef property_def = entity_def.GetProperty(property_name);
            if (property_def == null)
            {
                return;
            }

            byte property_type = property_def.Define.GetByte(Constant.FLAG_TYPE);
            switch ((VariantType)property_type)
            {
                case VariantType.Bool:
                    {
                        bool old_value = recv_msg.SubtractBool();
                        bool new_value = recv_msg.SubtractBool();
                        Kernel.SetPropertyBool(pid, property_name, new_value);
                    }
                    break;
                case VariantType.Byte:
                    {
                        byte old_value = recv_msg.SubtractByte();
                        byte new_value = recv_msg.SubtractByte();
                        Kernel.SetPropertyByte(pid, property_name, new_value);
                    }
                    break;
                case VariantType.Int:
                    {
                        int old_value = recv_msg.SubtractInt();
                        int new_value = recv_msg.SubtractInt();
                        Kernel.SetPropertyInt(pid, property_name, new_value);
                    }
                    break;
                case VariantType.Float:
                    {
                        float old_value = recv_msg.SubtractFloat();
                        float new_value = recv_msg.SubtractFloat();
                        Kernel.SetPropertyFloat(pid, property_name, new_value);
                    }
                    break;
                case VariantType.Long:
                    {
                        long old_value = recv_msg.SubtractLong();
                        long new_value = recv_msg.SubtractLong();
                        Kernel.SetPropertyLong(pid, property_name, new_value);
                    }
                    break;
                case VariantType.String:
                    {
                        string old_value = recv_msg.SubtractString();
                        string new_value = recv_msg.SubtractString();
                        Kernel.SetPropertyString(pid, property_name, new_value);
                    }
                    break;
                case VariantType.PersistID:
                    {
                        PersistID old_value = recv_msg.SubtractPid();
                        PersistID new_value = recv_msg.SubtractPid();
                        Kernel.SetPropertyPid(pid, property_name, new_value);
                    }
                    break;
            }

            Logger.Trace("{0} {1} event={2} ", pid, property_name, property_event);
        }

        private void OnRecvMsg_InitRole(VariantList recv_msg)
        {
            PersistID role_id = recv_msg.SubtractPid();
            if (PersistID.IsEmpty(role_id))
            {
                Logger.Warn("InitRole {0} Failed!", role_id.ToString());
                return;
            }

            KernelEngine.Instance.MainRole = role_id;

            Kernel.LoadEntity(role_id, recv_msg);

            Kernel.OnlineEntity(role_id);

            AppEngine.Instance.Network.SendSystemMsg(SystemOpcode.CLIENT.INIT_ROLE_FINISH);
        }


        public void DispatchMessage(int opcode, VariantList msg)
        {
            Action<VariantList> handler;

            if (_MessageHandlers.TryGetValue(opcode, out handler))
            {
                handler(msg);
            }
        }

        private void OnRecvMsg_SelectCharacter(VariantList recv_msg)
        {
        }

        private void RegisterMessageHandler(int opcode, Action<VariantList> handle)
        {
            if (_MessageHandlers.ContainsKey(opcode))
            {
                Logger.Warn("RegisterMessageHandler opcode={0} is exist.", opcode);
                return;
            }

            _MessageHandlers.Add(opcode, handle);
        }
    }
}
