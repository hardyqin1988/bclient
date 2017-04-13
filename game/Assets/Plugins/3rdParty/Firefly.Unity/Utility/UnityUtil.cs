using Firefly.Core.Interface;
using Firefly.Unity.Global;
using SLua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Unity.Utility
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        protected ILog Logger;
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    System.Type type = typeof(T);
                    obj.name = "[Singleton][" + type.Name + "]";
                    _instance = obj.AddComponent<T>();

                    _instance.Logger = LogAssert.GetLog(obj.name);
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public static class UnityUtil
    {
        public static int TerrainLayerMask = 1 << LayerMask.NameToLayer("Terrain");

        public static int SceneObjectLayerMask = TerrainLayerMask | 1 << LayerMask.NameToLayer("Default");

        public static void StartCoroutine(this MonoBehaviour mb, LuaFunction func)
        {
            mb.StartCoroutine(LuaCoroutine(func));
        }

        public static IEnumerator LuaCoroutine(LuaFunction func)
        {
            var thread = new LuaThreadWrapper(func);
            while (true)
            {
                object obj;
                if (!thread.Resume(out obj))
                {
                    yield break;
                }
                yield return obj;
            }
        }

        public static void ObjectDestroy(Object obj, float t)
        {
            if (Application.isPlaying)
            {
                if (t > 0)
                {
                    Object.Destroy(obj, t);
                }
                else
                {
                    Object.Destroy(obj);
                }
            }
            else
            {
                Object.DestroyImmediate(obj);
            }
        }

        public static bool CheckIsChild(GameObject goChild, GameObject goParent)
        {
            if (goChild == goParent)
            {
                return true;
            }

            Transform f = goParent.transform.FindChildRecursively(goChild.name);
            return f == null ? false : true;
        }

        public static bool RayCastBySegment(Vector3 s, Vector3 e, out Vector3 pos)
        {
            pos = Vector3.zero;

            Vector3 dir = e - s;
            float dis = dir.magnitude;
            Ray r = new Ray(s, dir.normalized);

            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo, dis, SceneObjectLayerMask))
            {
                pos = hitInfo.point;
                return true;
            }

            return false;
        }

        private static List<Vector3> pointList = new List<Vector3>();

        public static bool RayCastByParabola(Vector3 s, Vector3 e, float angle, out Vector3 pos)
        {
            pos = Vector3.zero;

            MathUtil.ParabolaToSegment(s, e, angle, 8, ref pointList);

            for (int i = 1; i < pointList.Count; i++)
            {
                if (RayCastBySegment(pointList[i - 1], pointList[i], out pos))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool RayCastTerrain(Vector3 p, int layerMask, out Vector3 pos)
        {
            Ray r = new Ray(p + 5 * Vector3.up, -Vector3.up);
            return RayCastTerrain(r, layerMask, out pos);
        }

        public static bool RayCastTerrain(Ray r, int layerMask, out Vector3 pos)
        {
            pos = Vector3.zero;

            int lmask = layerMask == -1 ? TerrainLayerMask : layerMask;

            RaycastHit hitInfo;
            if (Physics.Raycast(r, out hitInfo, 1000, lmask))
            {
                pos = hitInfo.point;
                return true;
            }

            return false;
        }

        public static Transform FindChildRecursively(this Transform aParent, string aName)
        {
            Transform result = aParent.Find(aName);
            if (result != null)
            {
                return result;
            }

            foreach (Transform child in aParent)
            {
                result = child.FindChildRecursively(aName);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static void SetVisible(this GameObject go, bool visible, string layer)
        {
            if (visible)
            {
                go.SetLayer(layer, "");
            }
            else
            {
                go.SetLayer("Invisible", "");
            }
        }

        public static void SetLayer(this GameObject go, string layerName, string ignoreTag = null)
        {
            SetLayer(go, LayerMask.NameToLayer(layerName), ignoreTag);
        }

        public static void SetLayer(this GameObject go, int layer, string ignoreTag = null)
        {
            SetLayerRecursively(go, layer, ignoreTag);
        }

        public static T GetComponetSafty<T>(this GameObject go) where T : Component
        {
            T ret = go.GetComponent<T>();
            if (ret == null)
            {
                ret = go.AddComponent<T>();
            }
            return ret;
        }

        private static void SetLayerRecursively(GameObject go, int layer, string ignoreTag)
        {
            bool bSkip = !string.IsNullOrEmpty(ignoreTag) && go.CompareTag(ignoreTag);
            if (!bSkip)
            {
                go.layer = layer;
            }
            int childCount = go.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                SetLayerRecursively(go.transform.GetChild(i).gameObject, layer, ignoreTag);
            }
        }

        public static void QuitGame()
        {
            Application.Quit();
        }

        public static GameObject findChild(GameObject obj, string name)
        {
            if (obj == null)
            {
                return null;
            }

            int count = obj.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                Transform child = obj.transform.GetChild(i);
                if (child.name.Equals(name))
                    return child.gameObject;
                GameObject obj2 = findChild(child.gameObject, name);
                if (obj2 != null)
                    return obj2;
            }
            return null;
        }

        public static T GetComponent<T>(GameObject obj, string name) where T : Component
        {
            GameObject find_obj = findChild(obj, name);

            if (find_obj != null)
            {
                return find_obj.GetComponent<T>();
            }

            return null;
        }

        public static void SetParent(this Transform child, Transform parent,
            Vector3 position, Quaternion rotation, Vector3 scale)
        {
            child.SetParent(parent);
            child.localPosition = position;
            child.localRotation = rotation;
            child.localScale = scale;
        }

        public static void SetRectParent(this GameObject child, GameObject parent)
        {
            RectTransform child_rect_trans = child.GetComponent<RectTransform>();
            RectTransform parent_rect_trans = parent.GetComponent<RectTransform>();

            if (child_rect_trans == null || parent_rect_trans == null)
            {
                return;
            }
            child_rect_trans.SetParent(parent_rect_trans);
            child_rect_trans.localScale = Vector3.one;
        }

        public static void SetWidth(this RectTransform rectTrans, float width)
        {
            var size = rectTrans.sizeDelta;
            size.x = width;
            rectTrans.sizeDelta = size;
        }

        public static void SetHeight(this RectTransform rectTrans, float height)
        {
            var size = rectTrans.sizeDelta;
            size.y = height;
            rectTrans.sizeDelta = size;
        }

        public static void SetPositionX(this Transform t, float newX)
        {
            t.position = new Vector3(newX, t.position.y, t.position.z);
        }

        public static void SetPositionY(this Transform t, float newY)
        {
            t.position = new Vector3(t.position.x, newY, t.position.z);
        }

        public static void SetLocalPositionX(this Transform t, float newX)
        {
            t.localPosition = new Vector3(newX, t.localPosition.y, t.localPosition.z);
        }

        public static void SetLocalPositionY(this Transform t, float newY)
        {
            t.localPosition = new Vector3(t.localPosition.x, newY, t.localPosition.z);
        }

        public static void SetPositionZ(this Transform t, float newZ)
        {
            t.position = new Vector3(t.position.x, t.position.y, newZ);
        }

        public static void SetLocalPositionZ(this Transform t, float newZ)
        {
            t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, newZ);
        }

        public static void SetLocalScale(this Transform t, Vector3 newScale)
        {
            t.localScale = newScale;
        }

        public static void SetLocalScaleZero(this Transform t)
        {
            t.localScale = Vector3.zero;
        }

        public static float GetPositionX(this Transform t)
        {
            return t.position.x;
        }

        public static float GetPositionY(this Transform t)
        {
            return t.position.y;
        }

        public static float GetPositionZ(this Transform t)
        {
            return t.position.z;
        }

        public static float GetLocalPositionX(this Transform t)
        {
            return t.localPosition.x;
        }

        public static float GetLocalPositionY(this Transform t)
        {
            return t.localPosition.y;
        }

        public static float GetLocalPositionZ(this Transform t)
        {
            return t.localPosition.z;
        }

        public static Vector2 ToVector2(this Vector3 vec)
        {
            return new Vector2(vec.x, vec.y);
        }
    }
}
