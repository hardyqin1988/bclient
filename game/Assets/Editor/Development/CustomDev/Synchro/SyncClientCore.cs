using Firefly.Core.Config;
using Firefly.Core.Data;
using Firefly.Unity.Global;
using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DevEditor.Custom
{
    public class SyncClientCore
    {
        private static string ClientEntityPath = AssetPath.GameConfigPath;
        private static string ClientCorePath = Application.dataPath + "/Scripts/MainRoleQuery/";

        public static void UpdateRoleProperty()
        {
            Definition define = Definition.Load(ClientEntityPath);
            if (define == null)
            {
                return;
            }

            EntityDef role_entity = define.GetEntity("role");
            if (role_entity == null)
            {
                return;
            }

            string filePath = ClientCorePath + "MainRoleQuery.Property.cs";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileInfo fi = new FileInfo(filePath);
            var di = fi.Directory;
            if (!di.Exists)
                di.Create();

            string name = "";
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("using IO.Common.Entity;");

                sb.AppendLine();

                sb.AppendLine("public partial class MainRoleQuery");

                sb.AppendLine("{");

                PropertyDef[] properties = role_entity.GetAllProperties();
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyDef property = properties[i];
                    if (property == null)
                    {
                        continue;
                    }

                    name = property.Name;
                    sb.Append(EditorConst.str_tab).Append("public ");
                    VariantType var_type = (VariantType)property.Define.GetByte(Constant.FLAG_TYPE);

                    string return_variant_type = "";
                    string get_variant_type = "";

                    switch (var_type)
                    {
                        case VariantType.Bool:
                            return_variant_type = "bool";
                            get_variant_type = "Bool";
                            break;
                        case VariantType.Byte:
                            return_variant_type = "byte";
                            get_variant_type = "Byte";
                            break;
                        case VariantType.Int:
                            return_variant_type = "int";
                            get_variant_type = "Int";
                            break;
                        case VariantType.Float:
                            return_variant_type = "float";
                            get_variant_type = "Float";
                            break;
                        case VariantType.Long:
                            return_variant_type = "long";
                            get_variant_type = "Long";
                            break;
                        case VariantType.PersistID:
                            return_variant_type = "PersistID";
                            get_variant_type = "Pid";
                            break;
                        case VariantType.String:
                            return_variant_type = "string";
                            get_variant_type = "String";
                            break;
                        case VariantType.Bytes:
                            return_variant_type = "Bytes";
                            get_variant_type = "Bytes";
                            break;
                    }

                    /*public int Diamond { get { return Kernel.GetPropertyInt(Role, RoleEntity.Properties.DIAMOND) } }*/
                    sb.Append(return_variant_type)
                        .Append(" ")
                        .Append(FormatPropertyHump(property.Name))
                        .Append(" { get { return Kernel.GetProperty")
                        .Append(get_variant_type)
                        .Append("(Role, RoleEntity.Properties.")
                        .Append(property.Name.ToUpper())
                        .Append("); } }").AppendLine();

                    sb.AppendLine();
                }

                sb.AppendLine("}");
                sw.Write(sb.ToString());
                sw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                Debug.LogError(name);
                Debug.LogError(e);
            }
        }

        public static string FormatPropertyHump(string name)
        {
            string[] property_temps = name.Split('_');
            string entityName = "";
            for (int i = 0; i < property_temps.Length; i++)
            {
                string temp = property_temps[i];
                temp = temp.Substring(0, 1).ToUpper() + temp.Substring(1, temp.Length - 1);

                entityName += temp;
            }

            return entityName;
        }

    }
}

