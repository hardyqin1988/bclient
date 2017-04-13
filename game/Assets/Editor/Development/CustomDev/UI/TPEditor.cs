using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TPEditor : MonoBehaviour
{
    [MenuItem("Development/UI/Replace Image")]
    static void ReplaceImage()
    {
        //获取图集
        //得到所有prefer上的image组件
        //得到image上图片命名，然后在图集中找对应名字的图片
        //替换  如果没有找到就跳过或者设置为none
        //保存prefer   Apply

        Object[] imgObj = Resources.LoadAll("ui_element/common/element");
        Object mask = null;
        for (int a = 0; a < imgObj.Length; a++)
        {
            if (imgObj[a].name == "mask")
            {
                mask = imgObj[a];
                break;
            }
        }
        //prefer
        Object[] go = Resources.LoadAll("UI/");
        for (int i = 0; i < go.Length; i++)
        {
            GameObject obj = Instantiate(go[i]) as GameObject;
            Image[] allImage = obj.GetComponentsInChildren<Image>(true);
            foreach (var item in allImage)
            {
                Image image = item.GetComponent<Image>();
                string imgName = (image.sprite).ToString();
                string new_img = "";
                if (imgName.IndexOf(" (") > 0)
                    new_img = imgName.Substring(0, imgName.IndexOf(" ("));
                bool found = false;
                for (int j = 0; j < imgObj.Length; j++)
                {
                    if (new_img.Equals(imgObj[j].name))
                    {
                        print("T");
                        image.sprite = (imgObj[j]) as Sprite;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    image.sprite = mask as Sprite;
                }
            }
            string localPath = "Assets/Resources/UI/" + (go[i] as GameObject).name + ".prefab";
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject));
            PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
        Debug.LogError("END");
    }
}
