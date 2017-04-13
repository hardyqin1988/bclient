using Firefly.Unity.Global;
using Firefly.Unity.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Firefly.Unity.Lua
{
    [System.Serializable]
    public class LuaBehaviourFields
    {
        [System.Serializable]
        public class Field
        {
            public string name;
            public string type;
            public int valueIndex = -1;
        }

        [SerializeField]
        public List<Field> fieldList = new List<Field>();

        [SerializeField]
        private List<Object> m_objects = new List<Object>();

        [SerializeField]
        private List<int> m_ints = new List<int>();

        [SerializeField]
        private List<string> m_strings = new List<string>();

        private Dictionary<string, System.Type> m_nameTypeMap = new Dictionary<string, System.Type>();

        public void OnAfterDeserialize()
        {
            m_nameTypeMap.Clear();

            Assembly[] assemblies =
            {
                typeof(GameObject).Assembly,
                typeof(UnityEngine.UI.Button).Assembly,
            };

            for (int i = 0; i < fieldList.Count; i++)
            {
                Field f = fieldList[i];
                System.Type found = GetTypeByName(f.type, assemblies);
                if (found != null)
                {
                    m_nameTypeMap[f.type] = found;
                }
            }
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnInitLuaBehaviourTable(LuaBehaviour behaviour)
        {
            for (int i = 0; i < fieldList.Count; i++)
            {
                Field f = fieldList[i];
                behaviour.SetValue(f.name, GetFieldValue(f));
            }
        }

        public void Clear()
        {
            m_nameTypeMap.Clear();
            fieldList.Clear();
            m_ints.Clear();
            m_strings.Clear();
            m_objects.Clear();
        }

        public System.Type GetFieldType(string type)
        {
            System.Type found = null;
            if (m_nameTypeMap.TryGetValue(type, out found))
            {
                return found;
            }

#if UNITY_EDITOR
            Assembly[] assemblies =
            {
                typeof(GameObject).Assembly,
                typeof(UnityEngine.UI.Button).Assembly,
            };
                  
            found = GetTypeByName(type, assemblies);
            if (found != null)
            {
                m_nameTypeMap[type] = found;
                return found;
            }
#endif

            LogAssert.Util.Warn("Get lua behaviour field type {0} failed.", type);
            return null;
        }

        public void SetFieldType(Field f, string type)
        {
            if (type == f.type)
            {
                return;
            }

            System.Type newType = GetFieldType(type);
            if (newType == null)
            {
                return;
            }

            RemoveFieldValue(f);
            f.type = type;
            AddFieldValue(f, GetDefaultFieldValue(f));
        }

        public Field AddField(string name, object value)
        {
            Field f = new Field();
            f.name = name;
            f.type = value.GetType().ToString();
            AddFieldValue(f, value);
            fieldList.Add(f);
            return f;
        }

        public Field AddFieldAt(int idx, string name, object value)
        {
            Field f = new Field();
            f.name = name;
            f.type = value.GetType().ToString();
            AddFieldValue(f, value);
            fieldList.Insert(idx, f);
            return f;
        }

        public void RemoveField(Field f)
        {
            fieldList.Remove(f);
            RemoveFieldValue(f);
        }

        public void RemoveFieldAt(int idx)
        {
            if (idx >= 0 && idx < fieldList.Count)
            {
                Field f = fieldList[idx];
                RemoveField(f);
            }
        }

        public object GetFieldValue(Field f)
        {
            System.Type type = GetFieldType(f.type);
            if (type == null)
            {
                return null;
            }

            if (type == typeof(int))
            {
                return GetInt(f.valueIndex);
            }
            if (type == typeof(float))
            {
                return GetFloat(f.valueIndex);
            }
            if (type == typeof(bool))
            {
                return GetBool(f.valueIndex);
            }
            if (type == typeof(string))
            {
                return GetString(f.valueIndex);
            }
            return GetObject(f.valueIndex);
        }

        private object GetDefaultFieldValue(Field f)
        {
            System.Type type = GetFieldType(f.type);
            if (type == null)
            {
                return null;
            }

            if (type == typeof(int))
            {
                return 0;
            }
            if (type == typeof(float))
            {
                return 0.0f;
            }
            if (type == typeof(bool))
            {
                return false;
            }
            if (type == typeof(string))
            {
                return "";
            }
            return null;
        }

        private void AddFieldValue(Field f, object value)
        {
            System.Type type = GetFieldType(f.type);
            if (type == null)
            {
                return;
            }

            if (type == typeof(int))
            {
                f.valueIndex = AddInt((int)value);
            }
            else if (type == typeof(float))
            {
                f.valueIndex = AddFloat((float)value);
            }
            else if (type == typeof(bool))
            {
                f.valueIndex = AddBool((bool)value);
            }
            else if (type == typeof(string))
            {
                f.valueIndex = AddString((string)value);
            }
            else
            {
                f.valueIndex = AddObject((Object)value);
            }
        }

        public void SetFieldValue(Field f, object value)
        {
            System.Type type = GetFieldType(f.type);
            if (type == null)
            {
                return;
            }

            if (type == typeof(int))
            {
                SetInt(f.valueIndex, (int)value);
            }
            else if (type == typeof(float))
            {
                SetFloat(f.valueIndex, (float)value);
            }
            else if (type == typeof(bool))
            {
                SetBool(f.valueIndex, (bool)value);
            }
            else if (type == typeof(string))
            {
                SetString(f.valueIndex, (string)value);
            }
            else
            {
                SetObject(f.valueIndex, (Object)value);
            }
        }

        private void RemoveFieldValue(Field f)
        {
            System.Type type = GetFieldType(f.type);
            if (type == null)
            {
                return;
            }

            if (type == typeof(int))
            {
                RemoveInt(f.valueIndex);
            }
            else if (type == typeof(float))
            {
                RemoveFloat(f.valueIndex);
            }
            else if (type == typeof(bool))
            {
                RemoveBool(f.valueIndex);
            }
            else if (type == typeof(string))
            {
                RemoveString(f.valueIndex);
            }
            else
            {
                RemoveObject(f.valueIndex);
            }
            f.valueIndex = -1;
        }

        private T GetObject<T>(int idx) where T : Object
        {
            if (m_objects.Count <= idx)
            {
                return (T)((object)null);
            }
            return (T)((object)m_objects[idx]);
        }

        private void SetObject<T>(int idx, T obj) where T : Object
        {
            while (idx >= m_objects.Count)
            {
                m_objects.Add(null);
            }
            m_objects[idx] = obj;
        }

        private int AddObject<T>(T obj) where T : Object
        {
            m_objects.Add(obj);
            return m_objects.Count - 1;
        }

        private Object GetObject(int idx)
        {
            return GetObject<Object>(idx);
        }

        private void SetObject(int idx, Object obj)
        {
            SetObject<Object>(idx, obj);
        }

        private int AddObject(Object obj)
        {
            return AddObject<Object>(obj);
        }

        private void RemoveObject(int idx)
        {
            if (idx >= 0 && idx < m_objects.Count)
            {
                m_objects.RemoveAt(idx);
            }
        }

        private float GetFloat(int idx)
        {
            if (!HasInt(idx))
            {
                return 0f;
            }
            return SystemUtil.IntBitsToFloat(GetInt(idx));
        }

        private void SetFloat(int idx, float v)
        {
            SetInt(idx, SystemUtil.FloatToIntBits(v));
        }

        private int AddFloat(float v)
        {
            return AddInt(SystemUtil.FloatToIntBits(v));
        }

        private void RemoveFloat(int idx)
        {
            RemoveInt(idx);
        }

        private bool GetBool(int idx)
        {
            return HasInt(idx) && GetInt(idx) != 0;
        }

        private void SetBool(int idx, bool b)
        {
            SetInt(idx, (!b) ? 0 : 1);
        }

        private int AddBool(bool b)
        {
            return AddInt((!b) ? 0 : 1);
        }

        private void RemoveBool(int idx)
        {
            RemoveInt(idx);
        }

        private int GetInt(int idx)
        {
            if (!HasInt(idx))
            {
                return 0;
            }
            return m_ints[idx];
        }

        private void SetInt(int idx, int v)
        {
            while (idx >= m_ints.Count)
            {
                m_ints.Add(0);
            }
            m_ints[idx] = v;
        }

        private int AddInt(int v)
        {
            m_ints.Add(v);
            return m_ints.Count - 1;
        }

        private void RemoveInt(int idx)
        {
            if (idx >= 0 && idx < m_ints.Count)
            {
                m_ints.RemoveAt(idx);
            }
        }

        private bool HasInt(int idx)
        {
            return idx < m_ints.Count;
        }

        private string GetString(int idx)
        {
            if (m_strings.Count <= idx)
            {
                return null;
            }
            return m_strings[idx];
        }

        private void SetString(int idx, string v)
        {
            while (idx >= m_strings.Count)
            {
                m_strings.Add(null);
            }
            m_strings[idx] = v;
        }

        private int AddString(string v)
        {
            m_strings.Add(v);
            return m_strings.Count - 1;
        }

        private void RemoveString(int idx)
        {
            if (idx >= 0 && idx < m_strings.Count)
            {
                m_strings.RemoveAt(idx);
            }
        }

        private System.Type GetTypeByName(string name, Assembly[] assemblies = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            System.Type type = null;
            if (name == "int")
            {
                type = typeof(int);
            }
            else if (name == "float")
            {
                type = typeof(float);
            }
            else if (name == "bool")
            {
                type = typeof(bool);
            }
            else if (name == "string")
            {
                type = typeof(string);
            }
            else if (name == "LuaTable")
            {
                type = typeof(SLua.LuaTable);
            }
            else
            {
                if (assemblies != null)
                {
                    for (int i = 0; i < assemblies.Length; i++)
                    {
                        type = assemblies[i].GetType(name);
                        if (type != null)
                        {
                            break;
                        }
                    }
                }

                if (type == null)
                {
                    type = System.Type.GetType(name);
                }
            }

            return type;
        }
    }
}


