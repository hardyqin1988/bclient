using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildBundleName : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string str in importedAssets)
        {
            //Debug.Log("Reimported Asset:" + str);
            CreateBundle(str);
        }
        foreach (string str in deletedAssets)
        {
            //Debug.Log("Deleted Asset:" + str);
        }

        for (int i = 0; i < movedAssets.Length; i++)
        {
            //Debug.Log("Moved Asset:" + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
            CreateBundle(movedAssets[i]);
        }
    }

    private static void CreateBundle(string path, string check)
    {

    }

    public static void CreateBundle(string path)
    {
        //Debug.Log("Reimported Asset: " + str);  
        if (path.EndsWith(".cs"))
        {
            return;
        }

        if (path.StartsWith("Assets/Runtime/Resources"))
        {
            int index = path.IndexOf("Assets/Runtime/Resources");
            if (index < 0)
            {
                return;
            }

            string bundle_name = path.Remove(0, "Assets/Runtime/Resources".Length+1);
            index = bundle_name.LastIndexOf(".");
            if (index >= 0)
            {
                bundle_name = bundle_name.Remove(index, bundle_name.Length - index);
            }

            AssetImporter asset = AssetImporter.GetAtPath(path);
            if (asset != null)
            {
                asset.assetBundleName = bundle_name;
            }
        }
        else if (path.StartsWith("Assets/_DepAssets"))
        {
            int index = path.IndexOf("Assets/_DepAssets");
            if (index < 0)
            {
                return;
            }

            string bundle_name = path.Remove(0, "Assets/_DepAssets".Length + 1);
            index = bundle_name.LastIndexOf(".");
            if (index >= 0)
            {
                bundle_name = bundle_name.Remove(index, bundle_name.Length - index);
            }

            AssetImporter asset = AssetImporter.GetAtPath(path);
            if (asset != null)
            {
                asset.assetBundleName = "dep_asset/" + bundle_name;
            }
        }
    }
}
