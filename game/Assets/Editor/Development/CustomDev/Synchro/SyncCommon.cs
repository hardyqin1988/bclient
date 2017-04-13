using Firefly.Unity.Global;
using UnityEditor;
using UnityEngine;

namespace DevEditor.Custom
{
    public class SyncCommon
    {
        private static string ClientEntityPath = AssetPath.GameConfigPath + "entity/";
        private static string ServerEntityPath = Application.dataPath + "/../../../server/build/resources/entity/";

        public static void DownloadEntityDefine()
        {
            EditorConst.CopyDir(ServerEntityPath, ClientEntityPath);
        }

        private static string ClientCommonPath = 
            Application.dataPath + EditorConst.PROJECT_CLIENT_COMMON_FOLDER;

        private static string ServerCommonPath =
            Application.dataPath + EditorConst.PROJECT_SERVER_COMMON_FOLDER;

        private static string ClientCorePath = 
            Application.dataPath + EditorConst.PROJECT_CLIENT_FIREFLY_FOLDER;

        private static string ServerCorePath = 
            Application.dataPath + EditorConst.PROJECT_SERVER_FIREFLY_FOLDER;

        [MenuItem("Development/Synchro/Common/DownloadCodes")]
        public static void DownloadCodes()
        {
            EditorConst.CopyDir(ServerCommonPath, ClientCommonPath);
            EditorConst.CopyDir(ServerCorePath, ClientCorePath);
        }
    }
}

