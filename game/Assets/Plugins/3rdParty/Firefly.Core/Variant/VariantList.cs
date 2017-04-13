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

using Firefly.Core.Data;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Firefly.Core.Variant
{
    [DataContract]
    [Serializable]
    [ProtoContract]
    public class VariantList
    {
        [DataMember]
        [ProtoMember(1, OverwriteList = true)]
        private List<IVariant> _VariantList;

        public int Count { get { return _VariantList.Count; } }

        public static readonly VariantList Empty = new VariantList();

        public static VariantList New()
        {
#if UNITY
            return VariantPool.NewList();
#else
            return new VariantList();
#endif
        }

        public VariantList()
        {
            _VariantList = new List<IVariant>();
        }

        public VariantList(VariantList list)
        {
            _VariantList = new List<IVariant>();
            _VariantList.AddRange(list._VariantList);
        }

        public VariantList(IEnumerable<IVariant> collection)
        {
            _VariantList = new List<IVariant>(collection);
        }

        public IVariant this[int index]
        {
            get
            {
                return GetVariant(index);
            }
        }

        private IVariant GetVariant(int index)
        {
            if (index < 0 || index >= Count)
            {
                return null;
            }

            return _VariantList[index];
        }

        public bool BoolAt(int index) { return GetVariant(index) as VariantBoolean; }
        public byte ByteAt(int index) { return GetVariant(index) as VariantByte; }
        public int IntAt(int index) { return GetVariant(index) as VariantInteger; }
        public float FloatAt(int index) { return GetVariant(index) as VariantFloat; }
        public long LongAt(int index) { return GetVariant(index) as VariantLong; }
        public string StringAt(int index) { return GetVariant(index) as VariantString; }
        public PersistID PidAt(int index) { return GetVariant(index) as VariantPersistID; }
        public Bytes BytesAt(int index) { return GetVariant(index) as VariantBytes; }
        public Int2 Int2At(int index) { return GetVariant(index) as VariantInt2; }
        public Int3 Int3At(int index) { return GetVariant(index) as VariantInt3; }

        public void RemoveRange(int index, int n)
        {
            if (index >= _VariantList.Count || index < 0)
            {
                return;
            }

            _VariantList.RemoveRange(index, n);
        }
        public void Remove(int index)
        {
            if (index >= _VariantList.Count || index < 0)
            {
                return;
            }

            IVariant variant = _VariantList[index];
#if UNITY
            VariantPool.Recycle(variant);
#endif
            _VariantList.RemoveAt(index);
        }

        public void Clear() { _VariantList.Clear(); }

        public VariantList Append(VariantList value)
        {
            _VariantList.AddRange(value._VariantList);
            return this;
        }

        public VariantList Append(bool value)      { _VariantList.Add(VariantBoolean.New(value)); return this; }
        public VariantList Append(byte value)      { _VariantList.Add(VariantByte.New(value)); return this; }
        public VariantList Append(int value)       { _VariantList.Add(VariantInteger.New(value)); return this; }
        public VariantList Append(float value)     { _VariantList.Add(VariantFloat.New(value)); return this; }
        public VariantList Append(long value)      { _VariantList.Add(VariantLong.New(value)); return this; }
        public VariantList Append(string value)    { _VariantList.Add(VariantString.New(value)); return this; }
        public VariantList Append(PersistID value) { _VariantList.Add(VariantPersistID.New(value)); return this; }
        public VariantList Append(Bytes value)     { _VariantList.Add(VariantBytes.New(value)); return this; }
        public VariantList Append(byte[] value)    { _VariantList.Add(VariantBytes.New(value)); return this; }
        public VariantList Append(Int2 value)      { _VariantList.Add(VariantInt2.New(value)); return this; }
        public VariantList Append(Int3 value)      { _VariantList.Add(VariantInt3.New(value)); return this; }

        private bool Check(int index, VariantType type)
        {
            if (index >= _VariantList.Count)
            {
                return false;
            }

            return _VariantList[index].Type == type;
        }

        public void SetBool(int index, bool value)
        {
            if (!Check(index, VariantType.Bool))
            {
                return;
            }

            (_VariantList[index] as VariantBoolean).Value = value;
        }

        public void SetByte(int index, byte value)
        {
            if (!Check(index, VariantType.Byte))
            {
                return;
            }

            (_VariantList[index] as VariantByte).Value = value;
        }

        public void SetInt(int index, int value)
        {
            if (!Check(index, VariantType.Int))
            {
                return;
            }

            (_VariantList[index] as VariantInteger).Value = value;
        }

        public void SetLong(int index, long value)
        {
            if (!Check(index, VariantType.Long))
            {
                return;
            }

            (_VariantList[index] as VariantLong).Value = value;
        }

        public void SetFloat(int index, float value)
        {
            if (!Check(index, VariantType.Float))
            {
                return;
            }

            (_VariantList[index] as VariantFloat).Value = value;
        }

        public void SetString(int index, string value)
        {
            if (!Check(index, VariantType.String))
            {
                return;
            }

            (_VariantList[index] as VariantString).Value = value;
        }

        public void SetPersistID(int index, PersistID value)
        {
            if (!Check(index, VariantType.PersistID))
            {
                return;
            }

            (_VariantList[index] as VariantPersistID).Value = value;
        }

        public void SetBytes(int index, Bytes value)
        {
            if (!Check(index, VariantType.Bytes))
            {
                return;
            }

            (_VariantList[index] as VariantBytes).Value = value;
        }

        public void SetInt2(int index, Int2 value)
        {
            if (!Check(index, VariantType.Int2))
            {
                return;
            }

            (_VariantList[index] as VariantInt2).Value = value;
        }

        public void SetInt3(int index, Int3 value)
        {
            if (!Check(index, VariantType.Int3))
            {
                return;
            }

            (_VariantList[index] as VariantInt3).Value = value;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{VariantList");
            for (int i = 0; i < Count; i++)
            {
                sb.AppendFormat(" ({0}:{1}) ", i, _VariantList[i].ObjectValue);
            }
            sb.Append('}');
            return sb.ToString();
        }

        public bool SubtractBool(int index)
        {
            bool value = BoolAt(index);
            Remove(index);
            return value;
        }

        public byte SubtractByte(int index)
        {
            byte value = ByteAt(index);
            Remove(index);
            return value;
        }

        public int SubtractInt(int index)
        {
            int value = IntAt(index);
            Remove(index);
            return value;
        }

        public float SubtractFloat(int index)
        {
            float value = FloatAt(index);
            Remove(index);
            return value;
        }

        public long SubtractLong(int index)
        {
            long value = LongAt(index);
            Remove(index);
            return value;
        }

        public string SubtractString(int index)
        {
            string value = StringAt(index);
            Remove(index);
            return value;
        }

        public PersistID SubtractPid(int index)
        {
            PersistID value = PidAt(index);
            Remove(index);
            return value;
        }

        public Bytes SubtractBytes(int index)
        {
            Bytes value = BytesAt(index);
            Remove(index);
            return value;
        }

        public Int2 SubtractInt2(int index)
        {
            Int2 value = Int2At(index);
            Remove(index);
            return value;
        }

        public Int3 SubtractInt3(int index)
        {
            Int3 value = Int3At(index);
            Remove(index);
            return value;
        }

        public VariantList SubtractRange(int index, int count)
        {
            var result = new VariantList(_VariantList.GetRange(index, count));
            RemoveRange(index, count);
            return result;
        }

        public bool      SubtractBool()      { return SubtractBool(0); }
        public byte      SubtractByte()      { return SubtractByte(0); }
        public int       SubtractInt()       { return SubtractInt(0); }
        public float     SubtractFloat()     { return SubtractFloat(0); }
        public long      SubtractLong()      { return SubtractLong(0); }
        public string    SubtractString()    { return SubtractString(0); }
        public PersistID SubtractPid()       { return SubtractPid(0); }
        public Bytes     SubtractBytes()     { return SubtractBytes(0); }
        public Int2      SubtractInt2()      { return SubtractInt2(0); }
        public Int3      SubtractInt3()      { return SubtractInt3(0); }

        public VariantList Copy()
        {
            return new VariantList(this);
        }

        public void Recycle()
        {
#if UNITY
            VariantPool.Recycle(this);
#endif
        }
    }
}
