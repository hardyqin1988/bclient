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
using System.Collections.Generic;
using System.Text;

namespace Firefly.Core.Data
{
    [ProtoContract]
    public class Entity : IEntity
    {
        #region ------ ------ ------ ------Dictionary------ ------ ------ ------
        [ProtoMember(Constant.DATA_PROTO_ENTITY_PROPERTIES)]
        private Dictionary<string, IProperty>   _PropertyDic = null;
        [ProtoMember(Constant.DATA_PROTO_ENTITY_RECORDS)]
        private Dictionary<string, IRecord>     _RecordDic = null;
        #endregion

        #region ------ ------ ------ ------Cache------ ------ ------ ------
        private IProperty   _Property = null;
        private IRecord     _Record = null;
        #endregion

        public Entity()
        {
            _PropertyDic = new Dictionary<string, IProperty>();
            _RecordDic   = new Dictionary<string, IRecord>();
            Online      = false;
        }

        [ProtoMember(Constant.DATA_PROTO_ENTITY_SELF_PID)]
        private PersistID _Self;
        public PersistID Self
        {
            get
            {
                return _Self;
            }
            set
            {
                _Self = value;
            }
        }

        [ProtoMember(Constant.DATA_PROTO_ENTITY_TYPE)]
        private string _Type;
        public string Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
            }
        }

        public bool Online { get; set; }

        public void Clear()
        {
            foreach (Property property in _PropertyDic.Values)
            {
                property.Clear();
            }

            foreach (Record record in _RecordDic.Values)
            {
                record.Clear();
            }
        }

        #region ------ ------ ------ ------Get------ ------ ------ ------
        public IProperty GetProperty(string property_name)
        {
            if (_Property != null && _Property.Name.Equals(property_name))
            {
                return _Property;
            }

            _PropertyDic.TryGetValue(property_name, out _Property);

            return _Property;
        }

        public IRecord GetRecord(string record_name)
        {
            if (_Record != null && _Record.Name.Equals(record_name))
            {
                return _Record;
            }

            _RecordDic.TryGetValue(record_name, out _Record);

            return _Record;
        }

        #endregion

        #region ------ ------ ------ ------Gets------ ------ ------ ------
        public IProperty[] GetProperties()
        {
            Property[] found = new Property[_PropertyDic.Values.Count];
            _PropertyDic.Values.CopyTo(found, 0);

            return found;
        }

        public IRecord[] GetRecords()
        {
            Record[] found = new Record[_RecordDic.Values.Count];
            _RecordDic.Values.CopyTo(found, 0);

            return found;
        }
        #endregion

        #region ------ ------ ------ ------Find------ ------ ------ ------
        public bool FindProperty(string property_name)
        {
            return _PropertyDic.ContainsKey(property_name);
        }

        public bool FindRecord(string record_name)
        {
            return _RecordDic.ContainsKey(record_name);
        }

        #endregion

        #region ------ ------ ------ ------Create------ ------ ------ ------
        public IProperty CreateProperty(string property_name, VariantType property_type)
        {
            Property property = null;
            if (property_type == VariantType.None)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Create Property [{0}] Fail Type None", property_name);
                throw new EntityException(sb.ToString());
            }
            else if (FindProperty(property_name))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Create Property Fail [{0}] is exist", property_name);
                throw new EntityException(sb.ToString());
            }
            else
            {
                property = new Property(property_name, property_type);
                _PropertyDic.Add(property_name, property);
            }

            return property;
        }

        public IRecord CreateRecord(string record_name, VariantList col_types)
        {
            Record record = null;
            if (FindRecord(record_name))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Create Record Fail [{0}] is exist", record_name);
                throw new EntityException(sb.ToString());
            }
            else
            {
                record = new Record(record_name, col_types);
                _RecordDic.Add(record_name, record);
            }

            return record;
        }
        #endregion
    }
}
