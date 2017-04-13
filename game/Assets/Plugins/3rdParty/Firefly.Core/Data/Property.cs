#region License
/*****************************************************************************
 *MIT License
 *
 *Copyright (c) 2017 cathy33

 *Permission is hereby granted, free of charge, to any person obtaining a copy
 *of this software and associated documentation files (the "Software"), to deal
 *in the Software without restriction, including without limitation the rights
 *to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *copies of the Software, and to permit persons to whom the Software is
 *furnished to do so, subject to the following conditions:

 *The above copyright notice and this permission notice shall be included in all
 *copies or substantial portions of the Software.

 *THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *SOFTWARE.
 *****************************************************************************/
#endregion

using Firefly.Core.Interface;
using Firefly.Core.Variant;
using ProtoBuf;

namespace Firefly.Core.Data
{
    [ProtoContract]
    public class Property : IProperty
    {
        [ProtoMember(Constant.DATA_PROTO_PROPERTY_VARIANT)]
        private IVariant _Variant;

        public VariantType Type { get { return _Variant == null ? VariantType.None : _Variant.Type; } }

        [ProtoMember(Constant.DATA_PROTO_PROPERTY_NAME)]
        private string _Name;
        public string Name { get { return _Name; } }

        public Property() { }
        public Property(string name, VariantType type)
        {
            _Name = name;

            switch (type)
            {
                case VariantType.Bool:
                    _Variant = VariantBoolean.New();
                    break;
                case VariantType.Byte:
                    _Variant = VariantByte.New();
                    break;
                case VariantType.Int:
                    _Variant = VariantInteger.New();
                    break;
                case VariantType.Long:
                    _Variant = VariantLong.New();
                    break;
                case VariantType.Float:
                    _Variant = VariantFloat.New();
                    break;
                case VariantType.String:
                    _Variant = VariantString.New();
                    break;
                case VariantType.PersistID:
                    _Variant = VariantPersistID.New();
                    break;
                case VariantType.Bytes:
                    _Variant = VariantBytes.New();
                    break;
                case VariantType.Int2:
                    _Variant = VariantInt2.New();
                    break;
                case VariantType.Int3:
                    _Variant = VariantInt3.New();
                    break;
                default:
                    break;
            }
        }

        private bool Check(VariantType type)
        {
            return _Variant.Type == type;
        }

        public bool      GetBool()   { return (_Variant as VariantBoolean); }
        public byte      GetByte()   { return (_Variant as VariantByte); }
        public int       GetInt()    { return (_Variant as VariantInteger); }
        public long      GetLong()   { return (_Variant as VariantLong); }
        public float     GetFloat()  { return (_Variant as VariantFloat); }
        public string    GetString() { return (_Variant as VariantString); }
        public PersistID GetPid()    { return (_Variant as VariantPersistID); }
        public Bytes     GetBytes()  { return (_Variant as VariantBytes); }
        public Int2      GetInt2()   { return (_Variant as VariantInt2); }
        public Int3      GetInt3()   { return (_Variant as VariantInt3); }

        public bool TrySetBool(bool value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Bool))
            {
                return false;
            }

            VariantBoolean vb = _Variant as VariantBoolean;
            bool old_value = vb;
            bool new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vb.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetByte(byte value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Byte))
            {
                return false;
            }

            VariantByte vb = _Variant as VariantByte;
            byte old_value = vb;
            byte new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vb.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt(int value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Int))
            {
                return false;
            }

            VariantInteger vi = _Variant as VariantInteger;
            int old_value = vi;
            int new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vi.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }
        public bool TrySetLong(long value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Long))
            {
                return false;
            }

            VariantLong vi = _Variant as VariantLong;
            long old_value = vi;
            long new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vi.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetFloat(float value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Float))
            {
                return false;
            }

            VariantFloat vi = _Variant as VariantFloat;
            float old_value = vi;
            float new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vi.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetString(string value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.String))
            {
                return false;
            }

            VariantString vs = _Variant as VariantString;
            string old_value = vs;
            string new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vs.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetPid(PersistID value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.PersistID))
            {
                return false;
            }

            VariantPersistID vp = _Variant as VariantPersistID;
            PersistID old_value = vp;
            PersistID new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vp.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetBytes(Bytes value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Bytes))
            {
                return false;
            }

            VariantBytes vp = _Variant as VariantBytes;
            Bytes old_value = vp;
            Bytes new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vp.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt2(Int2 value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Int2))
            {
                return false;
            }

            VariantInt2 vp = _Variant as VariantInt2;
            Int2 old_value = vp;
            Int2 new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vp.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt3(Int3 value, out VariantList result)
        {
            result = null;
            if (!Check(VariantType.Int3))
            {
                return false;
            }

            VariantInt3 vp = _Variant as VariantInt3;
            Int3 old_value = vp;
            Int3 new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            vp.Value = value;

            result = VariantList.New();
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public void Clear()
        {
            _Variant.Clear();
        }
    }
}
