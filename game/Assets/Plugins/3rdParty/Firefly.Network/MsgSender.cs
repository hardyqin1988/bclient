using Firefly.Core.Data;
using Firefly.Core.Variant;
using System.Collections.Generic;

namespace Firefly.Network
{
    public sealed class MsgSender
    {
        private static Queue<MsgSender> _PoolingMsgs = new Queue<MsgSender>();

        public static MsgSender New(int opcode)
        {
            if (_PoolingMsgs.Count == 0)
            {
                MsgSender msg = new MsgSender(opcode);

                return msg;
            }
            else
            {
                MsgSender msg = _PoolingMsgs.Dequeue();
                msg.Opcode = opcode;
                return msg;
            }
        }

        public static void Recycle(MsgSender handler)
        {
            handler.Msg.Clear();
            handler.Opcode = 0;
            _PoolingMsgs.Enqueue(handler);
        }

        public int Opcode { get; private set; }
        public VariantList Msg { get; private set; }

        MsgSender(int opcode)
        {
            Opcode = opcode;
            Msg = VariantList.New();
        }

        public MsgSender AppendBool(bool value)        { Msg.Append(value); return this; }
        public MsgSender AppendByte(byte value)        { Msg.Append(value); return this; }
        public MsgSender AppendInt(int value)          { Msg.Append(value); return this; }
        public MsgSender AppendFloat(float value)      { Msg.Append(value); return this; }
        public MsgSender AppendLong(long value)        { Msg.Append(value); return this; }
        public MsgSender AppendString(string value)    { Msg.Append(value); return this; }
        public MsgSender AppendPid(PersistID value)    { Msg.Append(value); return this; }
        public MsgSender AppendBytes(byte[] value)     { Msg.Append(value); return this; }
        public MsgSender AppendBytes(Bytes value)      { Msg.Append(value); return this; }
        public MsgSender AppendInt2(Int2 value)        { Msg.Append(value); return this; }
        public MsgSender AppendInt3(Int3 value)        { Msg.Append(value); return this; }
        public MsgSender AppendList(VariantList value) { Msg.Append(value); return this; }
    }
}
