using System.IO;
using UnityEditor;
using UnityEngine;

namespace DevEditor.Custom
{
    public class BundleChecker
    {
        public static void ResetAllBundleNames()
        {
            string path = Application.dataPath + "/" + "Resources/";
            SetAllFileBundle(path);
            AssetDatabase.Refresh();
        }

        private static void SetAllFileBundle(string path)
        {
            string[] files = Directory.GetFiles(path);
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains(".meta"))
                {
                    string basePath = "Assets" + files[i].Substring(Application.dataPath.Length);
                    BuildBundleName.CreateBundle(basePath);
                }
            }
            string[] dir_arr = Directory.GetDirectories(path);
            for (int i = 0; i < dir_arr.Length; i++)
            {
                SetAllFileBundle(dir_arr[i]);
            }
        }
    }
}
