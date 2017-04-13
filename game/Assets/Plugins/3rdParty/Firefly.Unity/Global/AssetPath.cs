using UnityEngine;

namespace Firefly.Unity.Global
{
    public static class AssetPath
    {
        public static string ResourcePath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/Build/";
#else
                return PersistentDataPath   + "/Build/";
#endif
            }
        }

        public static string PersistentDataPath { get { return Application.persistentDataPath + "/"; } }

        public static string StreamingPath      { get { return Application.streamingAssetsPath + "/"; } }

        public static string BundlePath         { get { return ResourcePath + "bundle/"; } }

        public static string LuaPath            { get { return ResourcePath + "lua/"; } }

        public static string GameConfigPath     { get { return ResourcePath + "config/"; } }

        public static string LocalizationPath   { get { return GameConfigPath + "localization/"; } }

        public static string AppIni             { get { return GameConfigPath + "engine/app_config.ini"; } }

        public static string SDKIni             { get { return GameConfigPath + "engine/sdk_config.ini"; } }

        public static string VersionIni         { get { return GameConfigPath + "engine/sdk_config.ini"; } }
    }
}