using Firefly.Unity.Global;
using UnityEditor;

namespace DevEditor.Custom
{
    public class BuildPak
    {
        public static void BuildAllPakToStreamingPath()
        {
            BuildConfigPakToStreamingPath();
            BuildLuaPakToStreamingPath();
            BuildBundlePakToStreamingPath();
        }

        public static void BuildConfigPakToStreamingPath()
        {
            lzip.compressDir(AssetPath.GameConfigPath, 1, AssetPath.StreamingPath + "config", true);
            /*ZipConstants.DefaultCodePage = System.Text.Encoding.UTF8.CodePage;
            ZipUtil.ZipFileDirectory(AssetPath.GameConfigPath, AssetPath.StreamingPath + "config");*/
            AssetDatabase.Refresh();
        }

        public static void BuildLuaPakToStreamingPath()
        {
            lzip.compressDir(AssetPath.LuaPath, 1, AssetPath.StreamingPath + "lua", true);
            AssetDatabase.Refresh();
        }

        public static void BuildBundlePakToStreamingPath()
        {
            lzip.compressDir(AssetPath.BundlePath, 1, AssetPath.StreamingPath + "bundle", true);
            AssetDatabase.Refresh();
        }
    }
}

