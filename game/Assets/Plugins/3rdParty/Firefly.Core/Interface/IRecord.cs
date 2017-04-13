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
    [ProtoInclude(Constant.DATA_PROTO_RECORD_TYPE, typeof(Record))]
    public interface IRecord
    {
        string Name { get; }

        VariantList ColTypes { get; }

        void Clear();

        bool IsFree(int row);

        bool TryAddRow(VariantList row_value, out VariantList result);
        bool TrySetRow(int row, VariantList row_value, out VariantList result);
        bool TryDelRow(int row, out VariantList result);
        VariantList GetRow(int row);
        VariantList GetRows();

        bool TrySetBool(int row, int col, bool value, out VariantList result);
        bool TrySetByte(int row, int col, byte value, out VariantList result);
        bool TrySetInt(int row, int col, int value, out VariantList result);
        bool TrySetLong(int row, int col, long value, out VariantList result);
        bool TrySetFloat(int row, int col, float value, out VariantList result);
        bool TrySetString(int row, int col, string value, out VariantList result);
        bool TrySetPid(int row, int col, PersistID value, out VariantList result);
        bool TrySetBytes(int row, int col, Bytes value, out VariantList result);
        bool TrySetInt2(int row, int col, Int2 value, out VariantList result);
        bool TrySetInt3(int row, int col, Int3 value, out VariantList result);

        bool      GetBool(int row, int col);
        byte      GetByte(int row, int col);
        int       GetInt(int row, int col);
        long      GetLong(int row, int col);
        float     GetFloat(int row, int col);
        string    GetString(int row, int col);
        PersistID GetPid(int row, int col);
        Bytes     GetBytes(int row, int col);
        Int2      GetInt2(int row, int col);
        Int3      GetInt3(int row, int col);

        int FindRowBool(int col, bool value);
        int FindRowByte(int col, byte value);
        int FindRowInt(int col, int value);
        int FindRowLong(int col, long value);
        int FindRowFloat(int col, float value);
        int FindRowString(int col, string value);
        int FindRowPid(int col, PersistID value);
        int FindRowBytes(int col, Bytes value);
        int FindRowInt2(int col, Int2 value);
        int FindRowInt3(int col, Int3 value);
    }
}
