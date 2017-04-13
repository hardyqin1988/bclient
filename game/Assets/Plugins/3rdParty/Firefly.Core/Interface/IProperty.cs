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
using Firefly.Core.Variant;
using ProtoBuf;

namespace Firefly.Core.Interface
{
    [ProtoContract]
    [ProtoInclude(Constant.DATA_PROTO_PROPERTY_TYPE, typeof(Property))]
    public interface IProperty
    {
        string Name { get; }
        VariantType Type { get; }
        void Clear();

        bool      GetBool();
        byte      GetByte();
        int       GetInt();
        long      GetLong();
        float     GetFloat();
        string    GetString();
        PersistID GetPid();
        Bytes     GetBytes();
        Int2      GetInt2();
        Int3      GetInt3();

        bool TrySetBool(bool value, out VariantList result);
        bool TrySetByte(byte value, out VariantList result);
        bool TrySetInt(int value, out VariantList result);
        bool TrySetLong(long value, out VariantList result);
        bool TrySetFloat(float value, out VariantList result);
        bool TrySetString(string value, out VariantList result);
        bool TrySetPid(PersistID value, out VariantList result);
        bool TrySetBytes(Bytes value, out VariantList result);
        bool TrySetInt2(Int2 value, out VariantList result);
        bool TrySetInt3(Int3 value, out VariantList result);
    }
}
