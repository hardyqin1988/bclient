using Firefly.Core.Data;
using Firefly.Core.Interface;
using Firefly.Core.Variant;

namespace Firefly.Unity.Kernel
{
    partial class CKernel
    {
        public bool FindRecord(PersistID pid, string record_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return false;
            }

            return entity.FindRecord(record_name);
        }

        public void ClearReocrd(PersistID pid, string record_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            record.Clear();

            VariantList result = VariantList.New();
            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.Clear, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.Clear, result);
            }
        }

        public void DelRecordRow(PersistID pid, string record_name, int row)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TryDelRow(row, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.DelRow, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.DelRow, result);
            }
        }

        public int AddRecordRow(PersistID pid, string record_name, VariantList value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            VariantList result;
            if (!record.TryAddRow(value, out result))
            {
                return Constant.INVALID_ROW;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.AddRow, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.AddRow, result);
            }

            return result.IntAt(0);
        }

        public void SetRecordRow(PersistID pid, string record_name, int row, VariantList value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetRow(row, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetRow, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetRow, result);
            }
        }

        #region ------ ------ ------ ------Find------ ------ ------ ------
        public int FindRecordRowBool(PersistID pid, string record_name, int col, bool value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowBool(col, value);
        }

        public int FindRecordRowByte(PersistID pid, string record_name, int col, byte value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowByte(col, value);
        }

        public int FindRecordRowFloat(PersistID pid, string record_name, int col, float value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowFloat(col, value);
        }

        public int FindRecordRowInt(PersistID pid, string record_name, int col, int value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowInt(col, value);
        }

        public int FindRecordRowLong(PersistID pid, string record_name, int col, long value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowLong(col, value);
        }

        public int FindRecordRowPid(PersistID pid, string record_name, int col, PersistID value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowPid(col, value);
        }

        public int FindRecordRowString(PersistID pid, string record_name, int col, string value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowString(col, value);
        }

        public int FindRecordRowBytes(PersistID pid, string record_name, int col, Bytes value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowBytes(col, value);
        }

        public int FindRecordRowInt2(PersistID pid, string record_name, int col, Int2 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowInt2(col, value);
        }

        public int FindRecordRowInt3(PersistID pid, string record_name, int col, Int3 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.INVALID_ROW;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.INVALID_ROW;
            }

            return record.FindRowInt3(col, value);
        }

        #endregion

        #region ------ ------ ------ ------Get------ ------ ------ ------
        public bool GetRecordBool(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_BOOLEAN;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_BOOLEAN;
            }

            return record.GetBool(row, col);
        }

        public byte GetRecordByte(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_BYTE;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_BYTE;
            }

            return record.GetByte(row, col);
        }

        public float GetRecordFloat(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_FLOAT;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_FLOAT;
            }

            return record.GetFloat(row, col);
        }

        public int GetRecordInt(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_INTEGER;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_INTEGER;
            }

            return record.GetInt(row, col);
        }

        public long GetRecordLong(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_LONG;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_LONG;
            }

            return record.GetLong(row, col);
        }

        public PersistID GetRecordPid(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return PersistID.Empty;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return PersistID.Empty;
            }

            return record.GetPid(row, col);
        }

        public string GetRecordString(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_STRING;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Constant.NULL_STRING;
            }

            return record.GetString(row, col);
        }

        public Int2 GetRecordInt2(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Int2.Zero;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Int2.Zero;
            }

            return record.GetInt2(row, col);
        }

        public Int3 GetRecordInt3(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Int3.Zero;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Int3.Zero;
            }

            return record.GetInt3(row, col);
        }

        public Bytes GetRecordBytes(PersistID pid, string record_name, int row, int col)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Bytes.Zero;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return Bytes.Zero;
            }

            return record.GetBytes(row, col);
        }

        public VariantList GetRecordRow(PersistID pid, string record_name, int row)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return VariantList.Empty;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return VariantList.Empty;
            }

            return record.GetRow(row);
        }

        public VariantList GetRecordRows(PersistID pid, string record_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return VariantList.Empty;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return VariantList.Empty;
            }

            return record.GetRows();
        }
        #endregion

        #region ------ ------ ------ ------Set------ ------ ------ ------
        public void SetRecordBool(PersistID pid, string record_name, int row, int col, bool value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetBool(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordByte(PersistID pid, string record_name, int row, int col, byte value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetByte(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordFloat(PersistID pid, string record_name, int row, int col, float value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetFloat(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordInt(PersistID pid, string record_name, int row, int col, int value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetInt(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordLong(PersistID pid, string record_name, int row, int col, long value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetLong(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordPid(PersistID pid, string record_name, int row, int col, PersistID value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetPid(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordString(PersistID pid, string record_name, int row, int col, string value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetString(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordInt2(PersistID pid, string record_name, int row, int col, Int2 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetInt2(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordInt3(PersistID pid, string record_name, int row, int col, Int3 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetInt3(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        public void SetRecordBytes(PersistID pid, string record_name, int row, int col, Bytes value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IRecord record = entity.GetRecord(record_name);
            if (record == null)
            {
                return;
            }

            VariantList result;
            if (!record.TrySetBytes(row, col, value, out result))
            {
                return;
            }

            _DataCallbackManager.CallRecordHandler(pid, record_name, RecordEvent.SetCol, result);

            if (entity.Online)
            {
                CallbackRecordData(pid, record_name, RecordEvent.SetCol, result);
            }
        }

        private void CallbackRecordData(PersistID pid, string record_name,
            RecordEvent record_event, VariantList result)
        {
            VariantList msg = VariantList.New();
            msg.Append(record_name).Append((byte)record_event).Append(result);
            _DataTypeCallbackManager.Callback(DataType.Record, this, pid, msg);
        }

        #endregion
    }
}
