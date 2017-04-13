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
using System;
using System.Collections.Generic;

namespace Firefly.Core.Data
{
    [ProtoContract]
    public class Record : IRecord
    {
        [ProtoMember(Constant.DATA_PROTO_RECORD_NAME)]
        private string _Name;
        public string Name { get { return _Name; } }

        [ProtoMember(Constant.DATA_PROTO_RECORD_COLS)]
        private VariantList _ColTypes;
        public VariantList ColTypes { get { return _ColTypes; } }

        [ProtoMember(Constant.DATA_PROTO_RECORD_ROW_VALUES)]
        private Dictionary<int, VariantList> _RowValues;

        [ProtoMember(Constant.DATA_PROTO_RECORD_USED_MAX_ROWS)]
        private int _UsedMaxRow = Constant.INVALID_ROW;

        public Record()
        {
            _RowValues  = new Dictionary<int, VariantList>();
            _UsedMaxRow = Constant.INVALID_ROW;
        }

        public Record(string name, VariantList col_types) : this()
        {
            _Name       = name;
            _ColTypes   = new VariantList(col_types);
        }

        public void Clear()
        {
            _RowValues.Clear();
        }

        public bool IsFree(int row)
        {
            return !_RowValues.ContainsKey(row);
        }

        private int FreeRow
        {
            get
            {
                for (int row = 0; row <= _UsedMaxRow; row++)
                {
                    if (_RowValues.ContainsKey(row))
                    {
                        continue;
                    }

                    return row;
                }

                return _UsedMaxRow+1;
            }
        }

        public bool TryAddRow(VariantList row_value, out VariantList result)
        {
            return TrySetRow(FreeRow, row_value, out result);
        }

        public bool TrySetRow(int row, VariantList row_value, out VariantList result)
        {
            result = null;

            if (row == Constant.INVALID_ROW)
            {
                return false;
            }
            else if (!_RowValues.ContainsKey(row))
            {
                _RowValues.Add(row, row_value);

                result = VariantList.New();
                result.Append(row);
                result.Append(row_value);
                return true;
            }
            else
            {
                _RowValues[row] = row_value;
                if (row > _UsedMaxRow)
                {
                    _UsedMaxRow = row;
                }

                result = VariantList.New();
                result.Append(row);
                result.Append(row_value);
                return true;
            }
        }

        public bool TryDelRow(int row, out VariantList result)
        {
            result = null;

            VariantList row_value = VariantList.Empty;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            _RowValues.Remove(row);

            result = VariantList.New();
            result.Append(row).Append(row_value);
            return true;
        }

        private bool Check(int row, int col, VariantType type)
        {
            if (!_RowValues.ContainsKey(row))
            {
                return false;
            }

            VariantType col_type = (VariantType)ColTypes.ByteAt(col);

            return col_type == type;
        }

        public bool TrySetBool(int row, int col, bool value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Bool))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            bool old_value = row_value.BoolAt(col);
            bool new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetBool(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetByte(int row, int col, byte value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Byte))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            byte old_value = row_value.ByteAt(col);
            byte new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetByte(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt(int row, int col, int value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Int))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            int old_value = row_value.IntAt(col);
            int new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetInt(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetLong(int row, int col, long value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Long))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            long old_value = row_value.LongAt(col);
            long new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetLong(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetFloat(int row, int col, float value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Float))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            float old_value = row_value.FloatAt(col);
            float new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetFloat(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetString(int row, int col, string value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.String))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            string old_value = row_value.StringAt(col);
            string new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetString(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetPid(int row, int col, PersistID value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.PersistID))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            PersistID old_value = row_value.PidAt(col);
            PersistID new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetPersistID(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetBytes(int row, int col, Bytes value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Bytes))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            Bytes old_value = row_value.BytesAt(col);
            Bytes new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetBytes(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt2(int row, int col, Int2 value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Int2))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            Int2 old_value = row_value.Int2At(col);
            Int2 new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetInt2(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool TrySetInt3(int row, int col, Int3 value, out VariantList result)
        {
            result = null;
            if (!Check(row, col, VariantType.Int3))
            {
                return false;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return false;
            }

            Int3 old_value = row_value.Int3At(col);
            Int3 new_value = value;

            if (old_value == new_value)
            {
                return false;
            }

            row_value.SetInt3(col, new_value);

            result = VariantList.New();
            result.Append(row);
            result.Append(col);
            result.Append(old_value);
            result.Append(new_value);

            return true;
        }

        public bool GetBool(int row, int col)
        {
            if (!Check(row, col, VariantType.Bool))
            {
                return Constant.NULL_BOOLEAN;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_BOOLEAN;
            }

            return row_value.BoolAt(col);
        }

        public byte GetByte(int row, int col)
        {
            if (!Check(row, col, VariantType.Byte))
            {
                return Constant.NULL_BYTE;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_BYTE;
            }

            return row_value.ByteAt(col);
        }

        public int GetInt(int row, int col)
        {
            if (!Check(row, col, VariantType.Int))
            {
                return Constant.NULL_INTEGER;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_INTEGER;
            }

            return row_value.IntAt(col);
        }

        public long GetLong(int row, int col)
        {
            if (!Check(row, col, VariantType.Long))
            {
                return Constant.NULL_LONG;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_LONG;
            }

            return row_value.LongAt(col);
        }

        public float GetFloat(int row, int col)
        {
            if (!Check(row, col, VariantType.Float))
            {
                return Constant.NULL_FLOAT;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_FLOAT;
            }

            return row_value.FloatAt(col);
        }

        public string GetString(int row, int col)
        {
            if (!Check(row, col, VariantType.String))
            {
                return Constant.NULL_STRING;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Constant.NULL_STRING;
            }

            return row_value.StringAt(col);
        }

        public PersistID GetPid(int row, int col)
        {
            if (!Check(row, col, VariantType.PersistID))
            {
                return PersistID.Empty;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return PersistID.Empty;
            }

            return row_value.PidAt(col);
        }

        public Bytes GetBytes(int row, int col)
        {
            if (!Check(row, col, VariantType.Bytes))
            {
                return Bytes.Zero;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Bytes.Zero;
            }

            return row_value.BytesAt(col);
        }

        public Int2 GetInt2(int row, int col)
        {
            if (!Check(row, col, VariantType.Int2))
            {
                return Int2.Zero;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Int2.Zero;
            }

            return row_value.Int2At(col);
        }

        public Int3 GetInt3(int row, int col)
        {
            if (!Check(row, col, VariantType.Int2))
            {
                return Int3.Zero;
            }

            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return Int3.Zero;
            }

            return row_value.Int3At(col);
        }

        public int FindRowBool(int col, bool value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.BoolAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowByte(int col, byte value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.ByteAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowInt(int col, int value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.IntAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowLong(int col, long value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.LongAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowFloat(int col, float value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.FloatAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowString(int col, string value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.StringAt(col).Equals(value, StringComparison.Ordinal))
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowPid(int col, PersistID value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.PidAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowBytes(int col, Bytes value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.BytesAt(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowInt2(int col, Int2 value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.Int2At(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public int FindRowInt3(int col, Int3 value)
        {
            int row = Constant.INVALID_ROW;
            foreach (KeyValuePair<int, VariantList> pair in _RowValues)
            {
                if (pair.Value.Int3At(col) == value)
                {
                    row = pair.Key;
                    break;
                }
            }

            return row;
        }

        public VariantList GetRow(int row)
        {
            VariantList row_value;
            if (!_RowValues.TryGetValue(row, out row_value))
            {
                return null;
            }

            return row_value;
        }

        public VariantList GetRows()
        {
            VariantList list = VariantList.New();
            foreach (int row in _RowValues.Keys)
            {
                list.Append(row);
            }

            return list;
        }
    }
}
