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

using Firefly.Core.Config.XML;
using Firefly.Core.Data;
using Firefly.Core.Variant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Firefly.Core.Config
{
    public class Definition
    {
        private Dictionary<string, EntityDef> _DefinitionDic = null;
        private string _ResourcePath = "";

        public static Definition Load(string resource_path)
        {
            Definition define = new Definition(resource_path);
            define.LoadEntityDefs();

            return define;
        }

        Definition(string resource_path)
        {
            _ResourcePath = resource_path;
            _DefinitionDic = new Dictionary<string, EntityDef>();
        }

        #region ------ ------ ------ ------Definition------ ------ ------ ------
        public EntityDef GetEntity(string class_name)
        {
            EntityDef found = null;
            _DefinitionDic.TryGetValue(class_name, out found);

            return found;
        }

        private EntityDef[] _Entities;
        public EntityDef[] GetEntities()
        {
            if (_Entities == null)
            {
                _Entities = new EntityDef[_DefinitionDic.Count];
                _DefinitionDic.Values.CopyTo(_Entities, 0);
            }
            return _Entities;
        }

        private void AddDefinition(string class_name, EntityDef define)
        {
            if (_DefinitionDic.ContainsKey(class_name))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("Get EntityDefinition Fail Name {0} is exist", class_name);
                throw new DefinitionException(sb.ToString());
            }
            else
            {
                _DefinitionDic.Add(class_name, define as EntityDef);
            }
        }

        private void LoadEntityDefs()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_ResourcePath);
            sb.Append("entity/entity_define.xml");

            StreamReader sr = new StreamReader(sb.ToString(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            sr.Close();

            XMLDocument xml = new XMLDocument(content);

            foreach (XMLNode xml_node in xml.RootNode.SubNodes)
            {
                LoadEntityDef(xml_node, null);
            }
        }

        private void LoadEntityDef(XMLNode xml_node, EntityDef parent)
        {
            string type = "";
            string path = "";
            foreach (var xml_attr in xml_node.Attributes)
            {
                switch (xml_attr.Name)
                {
                    case Constant.FLAG_TYPE:
                        type = xml_attr.Value;
                        break;
                    case Constant.FLAG_PATH:
                        path = xml_attr.Value;
                        break;
                }
            }

            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(path))
            {
                return;
            }

            EntityDef define = CreateEntityDef(type, path);
            define.Parent = parent;
            if (define.Parent != null)
            {
                define.Parent.AddChild(type);
            }

            AddDefinition(type, define);

            foreach (XMLNode xml_sub_node in xml_node.SubNodes)
            {
                LoadEntityDef(xml_sub_node, define);
            }
        }

        private EntityDef CreateEntityDef(string type, string define_path)
        {
            EntityDef define = new EntityDef(type);

            StringBuilder sb = new StringBuilder(_ResourcePath);
            sb.Append(define_path);

            StreamReader sr = new StreamReader(sb.ToString(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            sr.Close();

            XMLDocument xml = new XMLDocument(content);

            foreach (var xml_attr in xml.RootNode.Attributes)
            {
                if (xml_attr.Name.Equals(Constant.FLAG_NODE))
                {
                    define.Node = xml_attr.Value;
                }
            }

            foreach (XMLNode xml_node in xml.RootNode.SubNodes)
            {
                if (xml_node.Name.Equals(Constant.FLAG_PROPERTIES))
                {
                    LoadPropertiesDef(ref define, xml_node);
                }
                else if (xml_node.Name.Equals(Constant.FLAG_RECORDS))
                {
                    LoadRecordsDef(ref define, xml_node);
                }
                else if (xml_node.Name.Equals(Constant.FLAG_INCLUDES))
                {
                    LoadIncludesDef(ref define, xml_node);
                }
            }

            return define;
        }

        private void LoadIncludesDef(ref EntityDef define, XMLNode xml_includes)
        {
            foreach (XMLNode xml_include in xml_includes.SubNodes)
            {
                StringBuilder sb = new StringBuilder(_ResourcePath);
                sb.Append(xml_include.GetValue(Constant.FLAG_PATH));

                StreamReader sr = new StreamReader(sb.ToString(), Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();

                XMLDocument xml = new XMLDocument(content);

                foreach (XMLNode xml_node in xml.RootNode.SubNodes)
                {
                    if (xml_node.Name.Equals(Constant.FLAG_PROPERTIES))
                    {
                        LoadPropertiesDef(ref define, xml_node);
                    }
                    else if (xml_node.Name.Equals(Constant.FLAG_RECORDS))
                    {
                        LoadRecordsDef(ref define, xml_node);
                    }
                    else if (xml_node.Name.Equals(Constant.FLAG_INCLUDES))
                    {
                        LoadIncludesDef(ref define, xml_node);
                    }
                }
            }
        }

        private void LoadPropertiesDef(ref EntityDef define, XMLNode xml_properties)
        {
            foreach (XMLNode xml_node in xml_properties.SubNodes)
            {
                string property_name = "";

                VariantMap property_def = new VariantMap();

                property_name = xml_node.GetValue(Constant.FLAG_NAME);
                property_def.Add(Constant.FLAG_TYPE, (byte)Enum.Parse(typeof(VariantType), xml_node.GetValue(Constant.FLAG_TYPE)));
                property_def.Add(Constant.FLAG_SAVE, Convert.ToBoolean(xml_node.GetValue(Constant.FLAG_SAVE)));
                property_def.Add(Constant.FLAG_DESC, xml_node.GetValue(Constant.FLAG_DESC));

                if (string.IsNullOrEmpty(property_name))
                {
                    continue;
                }

                define.AddProperty(property_name, property_def);
            }
        }

        private void LoadRecordsDef(ref EntityDef define, XMLNode xml_records)
        {
            foreach (XMLNode xml_node in xml_records.SubNodes)
            {
                string record_name = "";

                VariantMap record_def = new VariantMap();

                record_name = xml_node.GetValue(Constant.FLAG_NAME);
                record_def.Add(Constant.FLAG_COLS, Convert.ToInt32(xml_node.GetValue(Constant.FLAG_COLS)));
                record_def.Add(Constant.FLAG_SAVE, Convert.ToBoolean(xml_node.GetValue(Constant.FLAG_SAVE)));
                record_def.Add(Constant.FLAG_DESC, xml_node.GetValue(Constant.FLAG_DESC));

                VariantList col_types = VariantList.New();
                VariantList col_names = VariantList.New();
                VariantList col_descs = VariantList.New();
                SortedDictionary<int, VariantMap> temp = new SortedDictionary<int, VariantMap>();
                foreach (var column_node in xml_node.SubNodes)
                {
                    VariantMap column = new VariantMap();
                    int index = Convert.ToInt32(column_node.GetValue(Constant.FLAG_INDEX));
                    byte type = (byte)Enum.Parse(typeof(VariantType), column_node.GetValue(Constant.FLAG_TYPE));
                    string name = column_node.GetValue(Constant.FLAG_NAME);
                    string desc = column_node.GetValue(Constant.FLAG_DESC);
                    column.Add(Constant.FLAG_TYPE, type);
                    column.Add(Constant.FLAG_NAME, name);
                    column.Add(Constant.FLAG_DESC, desc);
                    temp.Add(index, column);
                }

                if (string.IsNullOrEmpty(record_name))
                {
                    return;
                }

                if (record_def.GetInt(Constant.FLAG_COLS) != temp.Count)
                {
                    return;
                }

                foreach (VariantMap column in temp.Values)
                {
                    col_types.Append(column.GetByte(Constant.FLAG_TYPE));
                    col_names.Append(column.GetString(Constant.FLAG_NAME));
                    col_descs.Append(column.GetString(Constant.FLAG_DESC));
                }

                define.AddRecord(record_name, record_def, col_types, col_names, col_descs);
            }
        }

        #endregion
    }

    public class PropertyDef
    {
        public string Name { get; set; }

        public VariantMap Define { get; set; }

        public PropertyDef(string name, VariantMap define)
        {
            Name = name;
            Define = define;
        }
    }

    public class RecordDef
    {
        public string Name { get; set; }

        public VariantMap Define { get; set; }

        public VariantList ColTypes { get; set; }

        public VariantList ColNames { get; set; }

        public VariantList ColDescs { get; set; }

        public int ColOf(string col_name)
        {
            for (int col = 0; col < ColNames.Count; col++)
            {
                if (col_name.Equals(ColNames.StringAt(col)))
                {
                    return col;
                }
            }

            return Constant.INVALID_COL;
        }

        public RecordDef(string name, VariantMap define, VariantList col_types, VariantList col_names, VariantList col_descs)
        {
            Name = name;
            Define = define;
            ColTypes = col_types;
            ColNames = col_names;
            ColDescs = col_descs;
        }
    }


    public class EntityDef
    {
        private Dictionary<string, PropertyDef> _PropertyDefinitions;

        private Dictionary<string, RecordDef> _RecordDefinitions;

        private string _Name;
        private string _Node = Constant.DEFAULT_NODE;

        public EntityDef Parent { get; set; }

        private VariantList _InheritList;

        public VariantList InheritList
        {
            get
            {
                if (_InheritList.Count > 0) return _InheritList;

                FindParent(this, ref _InheritList);

                return _InheritList;
            }
        }

        public void AddChild(string child_type)
        {
            if (Parent != null)
            {
                AddChild(child_type);
            }

            if (!_Children.Contains(child_type))
            {
                _Children.Add(child_type);
            }
        }
        private HashSet<string> _Children;

        private VariantList _ChildList;
        public VariantList ChildList
        {
            get
            {
                if (_ChildList.Count > 0) return _ChildList;

                foreach (string child in _Children)
                {
                    _ChildList.Append(child);
                }

                return _ChildList;
            }
        }

        private VariantList _ChildListAndSelf;
        public VariantList ChildListAndSelf
        {
            get
            {
                if (_ChildListAndSelf.Count > 0) return _ChildListAndSelf;

                _ChildListAndSelf.Append(Name);

                foreach (string child in _Children)
                {
                    _ChildListAndSelf.Append(child);
                }

                return _ChildListAndSelf;
            }
        }

        private static void FindParent(EntityDef entity, ref VariantList list)
        {
            if (entity.Parent != null)
            {
                list.Append(entity.Parent.Name);
                FindParent(entity.Parent, ref list);
            }
        }

        public string Name { get { return _Name; } }

        public string Node { get { return _Node; } set { _Node = value; } }

        public EntityDef(string name)
        {
            _Name                = name;
            _PropertyDefinitions = new Dictionary<string, PropertyDef>();
            _RecordDefinitions   = new Dictionary<string, RecordDef>();
            _InheritList         = VariantList.New();
            _ChildList           = VariantList.New();
            _Children            = new HashSet<string>();
            _ChildListAndSelf    = VariantList.New();
        }

        public void AddProperty(string property_name, VariantMap define)
        {
            try
            {
                if (_PropertyDefinitions.ContainsKey(property_name))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("AddProperty Fail Name {0} is exist", property_name);
                }
                else
                {
                    PropertyDef property = new PropertyDef(property_name, define);
                    _PropertyDefinitions.Add(property_name, property);
                }
            }
            catch (Exception ex)
            {
                throw new DefinitionException("DefinitionExpection because ", ex);
            }
        }

        public void AddRecord(string record_name, VariantMap define, VariantList col_types, VariantList col_names, VariantList col_descs)
        {
            try
            {
                if (_RecordDefinitions.ContainsKey(record_name))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("AddRecord Fail Name {0} is exist", record_name);
                }
                else
                {
                    RecordDef record = new RecordDef(record_name, define, col_types, col_names, col_descs);
                    _RecordDefinitions.Add(record_name, record);
                }
            }
            catch (Exception ex)
            {
                throw new DefinitionException("DefinitionExpection because ", ex);
            }
        }

        private PropertyDef[] _AllProperties;
        public PropertyDef[] GetAllProperties()
        {
            if (_AllProperties != null) return _AllProperties;

            List<PropertyDef> list = new List<PropertyDef>();
            GetParentProperties(this, ref list);

            _AllProperties = list.ToArray();
            return _AllProperties;
        }

        private static void GetParentProperties(EntityDef entity, ref List<PropertyDef> list)
        {
            if (entity == null)
            {
                return;
            }

            PropertyDef[] self_properties = entity.GetProperties();
            if (self_properties == null)
            {
                return;
            }

            list.AddRange(self_properties);

            GetParentProperties(entity.Parent, ref list);
        }

        private RecordDef[] _AllRecords;
        public RecordDef[] GetAllRecords()
        {
            if (_AllRecords != null) return _AllRecords;

            List<RecordDef> list = new List<RecordDef>();
            GetParentRecords(this, ref list);

            _AllRecords = list.ToArray();
            return _AllRecords;
        }

        private static void GetParentRecords(EntityDef entity, ref List<RecordDef> list)
        {
            if (entity == null)
            {
                return;
            }

            RecordDef[] self_records = entity.GetRecords();
            if (self_records == null)
            {
                return;
            }

            list.AddRange(self_records);

            GetParentRecords(entity.Parent, ref list);
        }


        private PropertyDef[] _Properties;
        public PropertyDef[] GetProperties()
        {
            if (_Properties == null)
            {
                _Properties = new PropertyDef[_PropertyDefinitions.Count];
                _PropertyDefinitions.Values.CopyTo(_Properties, 0);
            }

            return _Properties;
        }

        private RecordDef[] _Records;
        public RecordDef[] GetRecords()
        {
            if (_Records == null)
            {
                _Records = new RecordDef[_RecordDefinitions.Count];
                _RecordDefinitions.Values.CopyTo(_Records, 0);
            }
            return _Records;
        }

        public PropertyDef GetProperty(string property_name)
        {
            PropertyDef[] properties = GetAllProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name.Equals(property_name))
                {
                    return properties[i];
                }
            }

            return null;
        }

        public RecordDef GetRecord(string record_name)
        {
            RecordDef[] records = GetAllRecords();
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i].Name.Equals(record_name))
                {
                    return records[i];
                }
            }

            return null;
        }
    }
}
