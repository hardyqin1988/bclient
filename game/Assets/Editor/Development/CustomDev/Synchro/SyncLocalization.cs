using Excel;
using Firefly.Unity.Global;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
using UnityEditor;

namespace DevEditor.Custom
{
    using L10N_Dir = Dictionary<string, Dictionary<string, string>>;
    using L10N = Dictionary<string, string>;
    public class SyncLocalization
    {
        public static void Excel2Language()
        {
            localizations.Clear();
            FileStream stream = File.Open(AssetPath.LocalizationPath + "localization.xlsx", FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            DataSet result = excelReader.AsDataSet();

            foreach (DataTable table in result.Tables)
            {
                ReadL10NFormSheet(table);
            }

            foreach (KeyValuePair<string, L10N> pair in _L10NS)
            {
                string language_dir = AssetPath.LocalizationPath + pair.Key;

                string filePath = AssetPath.LocalizationPath + pair.Key + ".language";

                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                XmlNode rootNode = xmlDoc.CreateElement("languages");

                foreach (KeyValuePair<string, string> key_text in pair.Value)
                {
                    XmlNode node = xmlDoc.CreateElement("language");

                    XmlAttribute name_attr = xmlDoc.CreateAttribute("name");
                    name_attr.Value = key_text.Key;
                    node.Attributes.Append(name_attr);

                    XmlAttribute text_attr = xmlDoc.CreateAttribute("text");
                    text_attr.Value = key_text.Value;
                    node.Attributes.Append(text_attr);

                    rootNode.AppendChild(node);
                }

                //附加根节点
                xmlDoc.AppendChild(rootNode);

                xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);

                //保存Xml文档
                xmlDoc.Save(filePath);
            }

            AssetDatabase.Refresh();
        }

        static Dictionary<string, L10N> _L10NS = new Dictionary<string, L10N>();

        static Dictionary<string, L10N_Dir> localizations
                = new Dictionary<string, L10N_Dir>();

        private static void ReadL10NFormSheet(DataTable sheet)
        {
            for (int col = 1; col < sheet.Columns.Count; col++)
            {
                for (int row = 1; row < sheet.Rows.Count; row++)
                {
                    string language = sheet.Rows[0][col].ToString();

                    L10N l10n = null;
                    if (!_L10NS.TryGetValue(language, out l10n))
                    {
                        l10n = new L10N();
                        _L10NS.Add(language, l10n);
                    }

                    string key = sheet.Rows[row][0].ToString();
                    string value = sheet.Rows[row][col].ToString();

                    l10n.Add(key, value);
                }
            }
        }

        /*private static void ReadLocalizationFormSheet(DataTable sheet)
        {
            L10N_Dir localization;
            if (!localizations.TryGetValue(sheet.TableName, out localization))
            {
                localization = new L10N_Dir();
                localizations.Add(sheet.TableName, localization);
            }

            Dictionary<int, string> language_dic = new Dictionary<int, string>();

            for (int col = 1; col < sheet.Columns.Count; col++)
            {
                string language = sheet.Rows[0][col].ToString();
                language_dic.Add(col, language);
            }

            for (int row = 1; row < sheet.Rows.Count; row++)
            {
                string key = sheet.Rows[row][0].ToString();

                for (int col = 1; col < sheet.Columns.Count; col++)
                {
                    string value = sheet.Rows[row][col].ToString();
                    string language = language_dic[col];

                    AddLanguageKeyValue(key, language, value, ref localization);
                }
            }

            CreateLocalization(sheet.TableName, localization);
        }

        private static void AddLanguageKeyValue(string key, string language, string value, ref L10N_Dir l)
        {
            Dictionary<string, string> dic = null;
            if (!l.TryGetValue(language, out dic))
            {
                dic = new Dictionary<string, string>();
                l.Add(language, dic);
            }

            if (!dic.ContainsKey(key))
            {
                dic.Add(key, value);
            }
            else
            {
                dic[key] = value;
            }
        }*/

        /*private static void CreateLocalization(string name, L10N_Dir l)
        {
            foreach (KeyValuePair<string, Dictionary<string, string>> pair in l)
            {
                string language_dir = AssetPath.LocalizationPath + pair.Key;

                if (!Directory.Exists(language_dir))
                {
                    Directory.CreateDirectory(language_dir);
                }
                string filePath = language_dir + "/" + name + ".language";

                XmlDocument xmlDoc = new XmlDocument();

                XmlDeclaration Declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

                XmlNode rootNode = xmlDoc.CreateElement(name + "s");

                foreach (KeyValuePair<string, string> key_text in pair.Value)
                {
                    XmlNode node = xmlDoc.CreateElement(name);

                    XmlAttribute name_attr = xmlDoc.CreateAttribute("name");
                    name_attr.Value = key_text.Key;
                    node.Attributes.Append(name_attr);

                    XmlAttribute text_attr = xmlDoc.CreateAttribute("text");
                    text_attr.Value = key_text.Value;
                    node.Attributes.Append(text_attr);

                    rootNode.AppendChild(node);
                }

                //附加根节点
                xmlDoc.AppendChild(rootNode);

                xmlDoc.InsertBefore(Declaration, xmlDoc.DocumentElement);

                //保存Xml文档
                xmlDoc.Save(filePath);
            }
        }*/
    }
}

