using System.IO;
using UnityEditor;

public class EditorConst
{
    public const string str_tab = "    ";
    public const string str_tab2 = str_tab + str_tab;
    public const string str_tab3 = str_tab2 + str_tab;
    public const string str_tab4 = str_tab3 + str_tab;
    public const string str_tab5 = str_tab4 + str_tab;
    public const string str_tab6 = str_tab5 + str_tab;
    public const string str_tab7 = str_tab6 + str_tab;

    public const string ASSET_BUNDLES_OUTPUT_PATH = "AssetBundles";
    public const string CLIENT_DEP_FOLDER = "/Plugins/3rdParty/";
    public const string SERVER_DEP_FOLDER = "/../../../server/biubiu/";
    public const string SERVER_FIREFLY_FOLDER = "/../../../server/firefly/";

    public const string PROJECT_CLIENT_FIREFLY_FOLDER = CLIENT_DEP_FOLDER + "Firefly.Core/";
    public const string PROJECT_SERVER_FIREFLY_FOLDER = SERVER_FIREFLY_FOLDER + "Firefly.Core/";

    public const string PROJECT_CLIENT_COMMON_FOLDER = CLIENT_DEP_FOLDER + PROJECT_COMMON_FOLDER;
    public const string PROJECT_SERVER_COMMON_FOLDER = SERVER_DEP_FOLDER + PROJECT_COMMON_FOLDER;

    public const string PROJECT_NAME = "BiuBiu";
    public const string PROJECT_COMMON_FOLDER = PROJECT_NAME + ".Common/";
    public const string PROJECT_DATAQUERY_FOLDER = PROJECT_COMMON_FOLDER + "/DataQuery/";

    public const string EDITOR_PATH = "/Development/Editor/";

    public static string FormatName(string name)
    {
        string[] entity_temps = name.Split('_');
        string entityName = "";
        for (int i = 0; i < entity_temps.Length; i++)
        {
            string temp = entity_temps[i];
            temp = temp.Substring(0, 1).ToUpper() + temp.Substring(1, temp.Length - 1);

            entityName += temp;
        }

        return entityName;
    }

    public static void MoveDir(string srPath, string targetPath)
    {
        string sourcePath = srPath;
        string tarPath = targetPath;
        //如果清空文件夹在粘贴,释放下边的循环
        //string[] find_files = Directory.GetFiles(tarPath, "*", SearchOption.AllDirectories);
        //for (int i = 0; i < find_files.Length; i++)
        //{
        //    string file = find_files[i];

        //    File.Delete(file);
        //}

        string[] source_paths = Directory.GetFiles(sourcePath);
        for (int i = 0; i < source_paths.Length; i++)
        {
            string temp = source_paths[i];
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
                FileInfo fi = new FileInfo(temp);
                fi.CopyTo(tarPath + fi.Name, true);
                UnityEngine.Debug.LogFormat("已更新文档 {0}", temp);
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogFormat("!!!!!!!!!!!!!! Error {0}", temp);
                UnityEngine.Debug.Log(ex);
            }

        }
        AssetDatabase.Refresh();
    }

    public static void CopyDir(string srcPath, string aimPath)
    {
        // 检查目标目录是否以目录分割字符结束如果不是则添加之
        if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
            aimPath += Path.DirectorySeparatorChar;
        // 判断目标目录是否存在如果不存在则新建之
        if (!Directory.Exists(aimPath)) Directory.CreateDirectory(aimPath);
        // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
        // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
        // string[] fileList = System.IO.Directory.GetFiles(srcPath);
        string[] fileList = Directory.GetFileSystemEntries(srcPath);
        for (int i = 0; i < fileList.Length; i++)
        {
            string temp = fileList[i];
            if (temp.LastIndexOf("Properties") >= 0)
            {
                continue;
            }
            if (temp.LastIndexOf("test") >= 0)
            {
                continue;
            }

            if (temp.LastIndexOf("bin") >= 0)
            {
                continue;
            }
            if (temp.LastIndexOf("obj") >= 0)
            {
                continue;
            }
            // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            if (Directory.Exists(temp))
                CopyDir(temp, aimPath + Path.GetFileName(temp));
            // 否则直接Copy文件
            else if ((temp.LastIndexOf(".meta") <= 0) && (temp.LastIndexOf(".csproj") <= 0))
            {
                File.Copy(temp, aimPath + Path.GetFileName(temp), true);
            }
        }

        AssetDatabase.Refresh();
    }
}

