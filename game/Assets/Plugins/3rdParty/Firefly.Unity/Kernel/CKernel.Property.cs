using Firefly.Core.Data;
using Firefly.Core.Interface;
using Firefly.Core.Variant;

namespace Firefly.Unity.Kernel
{
    partial class CKernel
    {
        public bool FindProperty(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return false;
            }

            return entity.FindProperty(property_name);
        }

        #region ------ ------ ------ ------Get------ ------ ------ ------
        public bool GetPropertyBool(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_BOOLEAN;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_BOOLEAN;
            }

            return property.GetBool();
        }

        public byte GetPropertyByte(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_BYTE;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_BYTE;
            }

            return property.GetByte();
        }

        public float GetPropertyFloat(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_FLOAT;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_FLOAT;
            }

            return property.GetFloat();
        }

        public int GetPropertyInt(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_INTEGER;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_INTEGER;
            }

            return property.GetInt();
        }

        public long GetPropertyLong(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_LONG;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_LONG;
            }

            return property.GetLong();
        }

        public PersistID GetPropertyPid(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return PersistID.Empty;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return PersistID.Empty;
            }

            return property.GetPid();
        }

        public string GetPropertyString(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Constant.NULL_STRING;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Constant.NULL_STRING;
            }

            return property.GetString();
        }

        public Bytes GetPropertyBytes(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Bytes.Zero;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Bytes.Zero;
            }

            return property.GetBytes();
        }

        public Int2 GetPropertyInt2(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Int2.Zero;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Int2.Zero;
            }

            return property.GetInt2();
        }

        public Int3 GetPropertyInt3(PersistID pid, string property_name)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return Int3.Zero;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return Int3.Zero;
            }

            return property.GetInt3();
        }
        #endregion

        #region ------ ------ ------ ------Set------ ------ ------ ------
        public void SetPropertyBool(PersistID pid, string property_name, bool value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetBool(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyByte(PersistID pid, string property_name, byte value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetByte(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyFloat(PersistID pid, string property_name, float value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetFloat(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyInt(PersistID pid, string property_name, int value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetInt(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyLong(PersistID pid, string property_name, long value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetLong(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyPid(PersistID pid, string property_name, PersistID value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetPid(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyString(PersistID pid, string property_name, string value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetString(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyBytes(PersistID pid, string property_name, Bytes value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetBytes(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyInt2(PersistID pid, string property_name, Int2 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetInt2(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        public void SetPropertyInt3(PersistID pid, string property_name, Int3 value)
        {
            IEntity entity = _EntityManager.GetEntity(pid);
            if (entity == null)
            {
                return;
            }

            IProperty property = entity.GetProperty(property_name);
            if (property == null)
            {
                return;
            }

            VariantList result;
            if (!property.TrySetInt3(value, out result))
            {
                return;
            }

            _DataCallbackManager.CallPropertyHandler(pid, property_name, PropertyEvent.Changed, result);

            if (entity.Online)
            {
                CallbackPropertyData(pid, property_name, PropertyEvent.Changed, result);
            }
        }

        private void CallbackPropertyData(PersistID pid, string property_name,
            PropertyEvent property_event, VariantList result)
        {
            VariantList msg = VariantList.New();
            msg.Append(property_name).Append((byte)property_event).Append(result);
            _DataTypeCallbackManager.Callback(DataType.Property, this, pid, msg);
        }

        #endregion
    }
}
