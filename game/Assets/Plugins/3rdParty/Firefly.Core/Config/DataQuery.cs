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
    public interface IData<ENTRY> : IDataQuery
    {
        ENTRY Entry { get; }

        VariantMap Map { get; set; }
    }

    public interface IDataQuery
    {
        void OnLoadFinish();
    }

    public class BaseQuery<ENTRY, DATA> where DATA : IData<ENTRY>
    { 
        private static Dictionary<ENTRY, DATA> dataMap = new Dictionary<ENTRY, DATA>();

        public static void Init<T>(string resources_path) where T : BaseQuery<ENTRY, DATA>
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(resources_path);

            StreamReader sr = new StreamReader(sb.ToString(), Encoding.UTF8);
            string content = sr.ReadToEnd();
            sr.Close();

            XMLDocument xml = new XMLDocument(content);

            VariantMap data_define = new VariantMap();

            foreach (XMLAttribute xml_attr in xml.RootNode.Attributes)
            {
                string value = xml_attr.Value;
                string name = xml_attr.Name;
                VariantType type = (VariantType)Enum.Parse(typeof(VariantType), value);

                data_define.Add(name, (byte)type);
            }

            T data_query = Activator.CreateInstance<T>();

            foreach (XMLNode xml_node in xml.RootNode.SubNodes)
            {
                DATA data = Activator.CreateInstance<DATA>();
                data.Map = new VariantMap();

                foreach (XMLAttribute xml_attr in xml_node.Attributes)
                {
                    string name = xml_attr.Name;
                    string value = xml_attr.Value;

                    VariantType type = (VariantType)data_define.GetByte(name);
                    switch (type)
                    {
                        case VariantType.Bool:
                            data.Map.Add(name, Convert.ToBoolean(value));
                            break;
                        case VariantType.Byte:
                            data.Map.Add(name, Convert.ToByte(value));
                            break;
                        case VariantType.Int:
                            data.Map.Add(name, Convert.ToInt32(value));
                            break;
                        case VariantType.Float:
                            data.Map.Add(name, Convert.ToSingle(value));
                            break;
                        case VariantType.Long:
                            data.Map.Add(name, Convert.ToInt64(value));
                            break;
                        case VariantType.String:
                            data.Map.Add(name, value);
                            break;
                        case VariantType.PersistID:
                            data.Map.Add(name, PersistID.Parse(value));
                            break;
                        case VariantType.Bytes:
                            data.Map.Add(name, Bytes.Parse(value));
                            break;
                    }
                }

                data.OnLoadFinish();
                data_query.AddData(data.Entry, data);
            }

            IDataQuery iquery = data_query as IDataQuery;
            if (iquery != null)
            {
                iquery.OnLoadFinish();
            }
        }

        private void AddData(ENTRY entry, DATA data)
        {
            if (dataMap.ContainsKey(entry))
            {
                return;
            }

            dataMap.Add(data.Entry, data);
        }

        public static DATA GetData(ENTRY entry)
        {
            DATA data = default(DATA);

            dataMap.TryGetValue(entry, out data);

            return data;
        }

        static DATA[] _datas = null;
        public static DATA[] GetAll()
        {
            if (_datas != null)
            {
                return _datas;
            }

            _datas = new DATA[dataMap.Values.Count];
            dataMap.Values.CopyTo(_datas, 0);
            return _datas;
        }

        public static bool HasEntry(ENTRY entry)
        {
            return dataMap.ContainsKey(entry);
        }
    }
}
