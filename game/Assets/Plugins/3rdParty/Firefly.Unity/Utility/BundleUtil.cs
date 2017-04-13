using UnityEngine;

namespace Firefly.Unity.Utility
{
    public static class BundleUtil
    {
        public const string AssetBundlesOutputPath = "AssetBundles";

        public const string ASSET_BUNDLE_MANIFEST = "AssetBundleManifest";

        public static string GetPlatformName()
        {
#if UNITY_EDITOR
            return GetPlatformForAssetBundles(UnityEditor.EditorUserBuildSettings.activeBuildTarget);
#else
            return GetPlatformForAssetBundles(Application.platform);
#endif
        }

        public static string FormatAssetPath(string asset_path)
        {
            return asset_path.Replace('\\', '/');
        }

        public static string GetAssetName(string asset_path)
        {
            int index = asset_path.LastIndexOf('/');
            if (index < 0)
            {
                return asset_path;
            }

            return asset_path.Remove(0, index+1);
        }

#if UNITY_EDITOR
        private static string GetPlatformForAssetBundles(UnityEditor.BuildTarget target)
        {
            switch (target)
            {
                case UnityEditor.BuildTarget.Android:
                    return "Android";
                case UnityEditor.BuildTarget.iOS:
                    return "iOS";
                case UnityEditor.BuildTarget.WebGL:
                    return "WebGL";
                case UnityEditor.BuildTarget.StandaloneWindows:
                case UnityEditor.BuildTarget.StandaloneWindows64:
                    return "Windows";
                case UnityEditor.BuildTarget.StandaloneOSXIntel:
                case UnityEditor.BuildTarget.StandaloneOSXIntel64:
                case UnityEditor.BuildTarget.StandaloneOSXUniversal:
                    return "OSX";
                default:
                    return null;
            }
        }
#endif

        private static string GetPlatformForAssetBundles(RuntimePlatform platform)
        {
            switch (platform)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                    return "Windows";
                case RuntimePlatform.OSXPlayer:
                    return "OSX";
                default:
                    return null;
            }
        }
    }
}
