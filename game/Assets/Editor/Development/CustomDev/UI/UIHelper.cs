using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

public class UIHelper
{
    [MenuItem("Development/UI/Create Label(msyh)")]
    [MenuItem("GameObject/UI/Label(msyh)")]
    public static void CreateLabel_MSYH()
    {
        GameObject select = Selection.activeGameObject;
		if (select == null)
		{
			return;
		}

		RectTransform trans = select.GetComponent<RectTransform>();
		if (trans == null)
		{
			return;
		}

        GameObject go = new GameObject();
        go.name = "lbl";

        go.AddComponent<RectTransform>();

        Text text = go.AddComponent<Text>();

        Font font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Resources/Font/msyh.ttf");
        text.font = font;
		text.fontSize = 24;
        text.text = "label";
        text.color = new Color((float)85 / 255, (float)89 / 255, (float)96 / 255, (float)255 / 255);

        text.GetComponent<RectTransform> ().SetParent(trans);
		text.GetComponent<RectTransform> ().localPosition = Vector3.zero;

		text.gameObject.layer = LayerMask.NameToLayer("UI");
    }

    [MenuItem("Development/UI/Create Label(impact)")]
    [MenuItem("GameObject/UI/Label(impact)")]
    public static void CreateLabel_Impact()
    {
        GameObject select = Selection.activeGameObject;
        if (select == null)
        {
            return;
        }

        RectTransform trans = select.GetComponent<RectTransform>();
        if (trans == null)
        {
            return;
        }

        GameObject go = new GameObject();
        go.name = "lbl";

        go.AddComponent<RectTransform>();

        Text text = go.AddComponent<Text>();

        Font font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Resources/Font/impact.ttf");
        text.font = font;
        text.fontSize = 24;
        text.text = "label";
        text.color = new Color((float)85/255, (float)89/255, (float)96/255, (float)255/255);

        text.GetComponent<RectTransform>().SetParent(trans);
        text.GetComponent<RectTransform>().localPosition = Vector3.zero;

        text.gameObject.layer = LayerMask.NameToLayer("UI");
    }
}
