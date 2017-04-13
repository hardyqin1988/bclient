using Excel;
using Firefly.Core.Config.XML;
using Firefly.Core.Data;
using Firefly.Core.Variant;
using Firefly.Unity.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DevEditor.Custom
{
    public class SyncDataQuery
    {
        private static string DataConfigExcelPath = AssetPath.GameConfigPath + "excel/";
        private static string DataConfigXmlPath = AssetPath.GameConfigPath + "data/";

        public static string data_path = "";
        public static string excel_path = "";

        private const string COMMON_DATAQUERY_NAMESPACE = "namespace " + EditorConst.PROJECT_NAME + ".Common.DataQuery";

        //从客户端的data/和excel/文件夹拷贝到服务端的data/和excel/文件夹
        private static string ClientDataPath = AssetPath.GameConfigPath + "data/";
        private static string ServerDataPath = Application.dataPath + "/../../../server/build/resources/data/";
        private static string ClientExcelPath = AssetPath.GameConfigPath + "excel/";
        private static string ServerExcelPath = Application.dataPath + "/../../../server/build/resources/excel/";

        private static string ClientCorePath = 
            Application.dataPath + 
            EditorConst.CLIENT_DEP_FOLDER + 
            EditorConst.PROJECT_DATAQUERY_FOLDER;

        private static string ServerCorePath = 
            Application.dataPath +
            EditorConst.SERVER_DEP_FOLDER + 
            EditorConst.PROJECT_DATAQUERY_FOLDER;

        private static string ClientDataQueryDefineFile = 
            Application.dataPath +
            EditorConst.CLIENT_DEP_FOLDER +
            EditorConst.PROJECT_DATAQUERY_FOLDER + 
            "DataQueryDefine.cs";

        /*private static string ServerDataQueryDefineFile = 
            Application.dataPath + 
            EditorConst.PROJECT_SERVER_DEP_FOLDER +
            EditorConst.PROJECT_DATAQUERY_FOLDER +
            "DataQueryDefine.cs";*/

        private static string ClientDataQueryPathFile =
            Application.dataPath +
            EditorConst.CLIENT_DEP_FOLDER +
            EditorConst.PROJECT_DATAQUERY_FOLDER + 
            "DataQueryPath.cs";

        /*private static string ServerDataQueryPathFile = 
            Application.dataPath +
            EditorConst.PROJECT_SERVER_DEP_FOLDER +
            EditorConst.PROJECT_DATAQUERY_FOLDER + 
            "DataQueryPath.cs";*/

        private static VariantList _Contents = VariantList.New();
        private static VariantList _Paths = VariantList.New();

        public static void RefreshDataQuery()
        {
            _Contents.Clear();
            _Paths.Clear();

            string srcDir = ClientDataPath;

            DirectoryInfo source = new DirectoryInfo(srcDir);
            if (!source.Exists)
            {
                throw new Exception("源目录不存在！");
            }

            FileInfo[] files = source.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(".meta"))
                {
                    continue;
                }
                StreamReader sr = new StreamReader(file.FullName, Encoding.UTF8);
                string content = sr.ReadToEnd();
                sr.Close();
                XMLDocument xml = new XMLDocument(content);
                VariantList list = VariantList.New();
                foreach (XMLAttribute xml_attr in xml.RootNode.Attributes)
                {
                    string name = xml_attr.Name;
                    list.Append(name);
                    Debug.Log(name);
                }

                GenDataQueryCSharpScript(file.Name, list);
            }

            CreateDataDefine(ClientDataQueryDefineFile);
            CreateDataPath(ClientDataQueryPathFile);
        }

        private static void GenDataQueryCSharpScript(string data_name, VariantList list)
        {
            string class_define_name = EditorConst.FormatName(data_name.Replace(".xml", "")) + "Def";
            string class_path_name = data_name.Replace(".xml", "");
            string[] class_path_temp = class_path_name.Split('_');

            StringBuilder new_class_path = new StringBuilder();
            new_class_path.Append(EditorConst.str_tab2);
            new_class_path.Append("public const string ");

            foreach (string temp in class_path_temp)
            {
                new_class_path.Append(temp.ToUpper());
                new_class_path.Append("_");
            }

            new_class_path.Append("PATH = \"data/");
            new_class_path.Append(data_name);
            new_class_path.Append("\";");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine();

            sb.Append(EditorConst.str_tab);

            sb.Append("public class ").Append(class_define_name);
            sb.AppendLine();
            sb.Append(EditorConst.str_tab + "{");
            sb.AppendLine();
            int count = list.Count;
            for (int i = 0; i < count; i++)
            {
                string prop_name = list.StringAt(i);
                sb.Append(EditorConst.str_tab2).Append("public const string ").Append(prop_name).Append(" = \"").Append(prop_name).Append("\";");
                sb.AppendLine();
            }


            sb.AppendLine();
            sb.Append(EditorConst.str_tab + "}");

            sb.AppendLine();

            Debug.Log(sb.ToString());

            _Contents.Append(sb.ToString());
            _Paths.Append(new_class_path.ToString());
        }

        private static void CreateDataPath(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.CreateNew);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            StringBuilder sb = new StringBuilder(COMMON_DATAQUERY_NAMESPACE);
            sb.AppendLine();
            sb.AppendLine("{");
            sb.Append(EditorConst.str_tab).Append("public class DataQueryPath").AppendLine();
            sb.Append(EditorConst.str_tab).Append("{").AppendLine();

            for (int i = 0; i < _Paths.Count; i++)
            {
                sb.AppendLine(_Paths.StringAt(i));
                sb.AppendLine();
            }

            sb.AppendLine();
            sb.Append(EditorConst.str_tab).AppendLine("}");
            sb.AppendLine("}");
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
        }

        private static void CreateDataDefine(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.CreateNew);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            StringBuilder sb = new StringBuilder(COMMON_DATAQUERY_NAMESPACE);
            sb.AppendLine();
            sb.Append("{");
            for (int i = 0; i < _Contents.Count; i++)
            {
                sb.Append(_Contents.StringAt(i));
            }

            sb.AppendLine();
            sb.Append("}");
            sw.Write(sb.ToString());
            sw.Close();
            fs.Close();
        }

        public static void Upload2Server()
        {
            //Data
            EditorConst.MoveDir(ClientDataPath, ServerDataPath);
            //Excle
            EditorConst.MoveDir(ClientExcelPath, ServerExcelPath);

            EditorConst.MoveDir(ClientCorePath, ServerCorePath);
        }

        // Use this for initialization
        public static void Excel2Xml()
        {
            string[] find_files = Directory.GetFiles(DataConfigXmlPath, "*", SearchOption.AllDirectories);

            for (int i = 0; i < find_files.Length; i++)
            {
                string file = find_files[i];

                File.Delete(file);
            }

            data_path = DataConfigXmlPath;
            excel_path = DataConfigExcelPath;

            string[] excel_paths = Directory.GetFiles(excel_path);
            for (int i = 0; i < excel_paths.Length; i++)
            {
                string temp = excel_paths[i];
                if (temp.LastIndexOf("~$") >= 0)
                {
                    continue;
                }
                if (temp.LastIndexOf(".meta") >= 0)
                {
                    continue;
                }

                try
                {
                    TransformExcel2Xml(temp);
                    UnityEngine.Debug.LogFormat("已保存Xml文档 {0}", temp);
                }
                catch (Exception ex)
                {
                    UnityEngine.Debug.LogFormat("!!!!!!!!!!!!!! Error {0}", temp);
                    UnityEngine.Debug.Log(ex);
                }
            }
            AssetDatabase.Refresh();
        }

        private static void TransformExcel2Xml(string excel_path)
        {
            string name = excel_path.Remove(0, excel_path.LastIndexOf("/") + 1);
            name = name.Replace(".xlsx", ".xml");

            FileStream file_stream = new FileStream(excel_path, FileMode.Open);
            IExcelDataReader excelReader =
                ExcelReaderFactory.CreateOpenXmlReader(file_stream);

            DataSet result = excelReader.AsDataSet(true);

            VariantList des_list = VariantList.New();
            VariantList name_list = VariantList.New();
            VariantList type_list = VariantList.New();
            List<VariantList> values_list = new List<VariantList>();

            for (int sheet = 0; sheet < result.Tables.Count; sheet++)
            {
                DataTable table = result.Tables[sheet];
                int rows = table.Rows.Count;

                for (int row = 0; row < table.Rows.Count; row++)
                {
                    DataRow data_row = table.Rows[row];
                    if (data_row.ItemArray.Length == 0)
                    {
                        continue;
                    }

                    VariantList value_list = VariantList.New();

                    for (int col = 0; col < data_row.Table.Columns.Count; col++)
                    {
                        string columnValue = data_row.Table.Rows[row][col].ToString();
                        if (row == 0 && !string.IsNullOrEmpty(columnValue) && sheet == 0)
                        {
                            des_list.Append(columnValue);
                        }
                        else if (row == 1 && !string.IsNullOrEmpty(columnValue) && sheet == 0)
                        {
                            name_list.Append(columnValue);
                        }
                        else if (row == 2 && !string.IsNullOrEmpty(columnValue) && sheet == 0)
                        {
                            type_list.Append((byte)(VariantType)(Enum.Parse(typeof(VariantType), columnValue)));
                        }
                        else if (row >= 3)
                        {
                            value_list.Append(columnValue);
                        }
                    }

                    if (value_list.Count == 0)
                    {
                        continue;
                    }

                    values_list.Add(value_list);
                }
            }

            /*Console.WriteLine(des_list.ToString());
            Console.WriteLine(name_list.ToString());
            Console.WriteLine(type_list.ToString());*/

            for (int i = 0; i < values_list.Count; i++)
            {
                VariantList list = values_list[i];
                //Console.WriteLine(list.ToString());
            }

            string filePath = data_path + name;

            XmlDocument xmlDoc = new XmlDocument();
            //创建Xml声明部分，即<?xml version="1.0" encoding="utf-8" ?>
            XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

            //创建根节点
            XmlNode rootNode = xmlDoc.CreateElement("objects");
            for (int i = 0; i < name_list.Count; i++)
            {
                XmlAttribute attr = xmlDoc.CreateAttribute(name_list.StringAt(i));
                attr.Value = ((VariantType)type_list.ByteAt(i)).ToString();

                rootNode.Attributes.Append(attr);
            }

            for (int i = 0; i < values_list.Count; i++)
            {
                XmlNode node = xmlDoc.CreateElement("object");

                VariantList list = values_list[i];

                bool isNull = true;
                for (int index = 0; index < list.Count; index++)
                {
                    if (!string.IsNullOrEmpty(list.StringAt(index)))
                    {
                        isNull = false;
                        break;
                    }
                }

                if (isNull)
                {
                    continue;
                }

                for (int j = 0; j < list.Count; j++)
                {
                    string value = list.StringAt(j);
                    if (value == "")
                    {
                        continue;
                    }

                    XmlAttribute attr = xmlDoc.CreateAttribute(name_list.StringAt(j));
                    attr.Value = value;

                    node.Attributes.Append(attr);
                }

                rootNode.AppendChild(node);
            }

            //附加根节点
            xmlDoc.AppendChild(rootNode);

            xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);

            //保存Xml文档
            xmlDoc.Save(filePath);
        }
    }
}