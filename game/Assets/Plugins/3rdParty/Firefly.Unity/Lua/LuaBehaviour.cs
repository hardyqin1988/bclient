using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using SLua;

namespace Firefly.Unity.Lua
{
    public class LuaBehaviour : MonoBehaviour, ISerializationCallbackReceiver
    {
        public string lua;

        [SerializeField]
        private LuaBehaviourFields m_fields = new LuaBehaviourFields();

        public LuaTable luaTable
        {
            get
            {
                return TryGetLuaTable();
            }
        }

#if UNITY_EDITOR
        public LuaBehaviourFields fields
        {
            get
            {
                return m_fields;
            }
        }
#endif

        protected LuaBehaviourTable m_table;
        protected LuaFunction m_funcUpdate;
        protected LuaFunction m_funcLateUpdate;

        [DoNotToLua]
        protected virtual void Awake()
        {
            TryGetLuaTable();
            Invoke0("Awake");
        }

        [DoNotToLua]
        protected virtual void Start()
        {
            Invoke0("Start");
        }

        [DoNotToLua]
        protected virtual void OnEnable()
        {
            Invoke0("OnEnable");
        }

        [DoNotToLua]
        protected virtual void OnDisable()
        {
            Invoke0("OnDisable");
        }

        [DoNotToLua]
        protected virtual void OnMouseDown()
        {
            Invoke0("OnMouseDown");
        }

        [DoNotToLua]
        protected virtual void OnMouseUp()
        {
            Invoke0("OnMouseUp");
        }

        [DoNotToLua]
        public virtual void OnUpdate(float deltaTime)
        {
            if (m_funcUpdate != null)
            {
                m_table.Invoke(m_funcUpdate);
            }
        }

        [DoNotToLua]
        public virtual void OnLateUpdate()
        {
            if (m_funcLateUpdate != null)
            {
                m_table.Invoke(m_funcLateUpdate);
            }
        }

        [DoNotToLua]
        protected virtual void OnDestroy()
        {
            LuaEngine.Instance.RemoveUpdateBehaviour(this);
            LuaEngine.Instance.RemoveLateUpdateBehaviour(this);
            Invoke0("OnDestroy");
            Dispose();
        }

        [DoNotToLua]
        public void OnAfterDeserialize()
        {
            this.m_fields.OnAfterDeserialize();
        }

        [DoNotToLua]
        public void OnBeforeSerialize()
        {
            this.m_fields.OnBeforeSerialize();
        }

        [DoNotToLua]
        public void SetLua(string lua)
        {
            if (this.lua != lua)
            {
                Dispose();
                this.lua = lua;
                TryGetLuaTable();
                Invoke0("Awake");
            }
        }

        [DoNotToLua]
        protected void Dispose()
        {
            if (m_table != null)
            {
                m_table.Dispose();
                m_table = null;
                m_funcUpdate = null;
                m_funcLateUpdate = null;
            }
        }

        [DoNotToLua]
        public object Invoke0(string funcName)
        {
            if (m_table != null)
            {
                return m_table.Invoke(funcName);
            }
            return null;
        }

        [DoNotToLua]
        public object Invoke1(string funcName, object a1)
        {
            if (m_table != null)
            {
                return m_table.Invoke(funcName, a1);
            }
            return null;
        }

        [DoNotToLua]
        public object Invoke2(string funcName, object a1, object a2)
        {
            if (m_table != null)
            {
                return m_table.Invoke(funcName, a1, a2);
            }
            return null;
        }

        [DoNotToLua]
        public object Invoke3(string funcName, object a1, object a2, object a3)
        {
            if (m_table != null)
            {
                return m_table.Invoke(funcName, a1, a2, a3);
            }
            return null;
        }

        [DoNotToLua]
        public object Invoke(string funcName, params object[] args)
        {
            if (m_table != null)
            {
                return m_table.Invoke(funcName, args);
            }
            return null;
        }

        [DoNotToLua]
        protected virtual void OnInitTable(LuaTable table)
        {
            if (table != null)
            {
                table["gameObject"] = base.gameObject;
                table["transform"] = base.transform;
                table["luaBehaviour"] = this;
            }
            this.m_fields.OnInitLuaBehaviourTable(this);
        }

        [DoNotToLua]
        public LuaTable TryGetLuaTable()
        {
            if (m_table == null && !string.IsNullOrEmpty(this.lua))
            {
                m_table = new LuaBehaviourTable(lua);
                m_funcUpdate = m_table.GetFunction("Update");
                m_funcLateUpdate = m_table.GetFunction("LateUpdate");
                OnInitTable(m_table.table);
            }

            if (m_funcUpdate != null)
            {
                LuaEngine.Instance.AddUpdateBehaviour(this);
            }

            if (m_funcLateUpdate != null)
            {
                LuaEngine.Instance.AddLateUpdateBehaviour(this);
            }

            if (m_table != null)
            {
                return m_table.table;
            }

            return null;
        }

        [DoNotToLua]
        public void SetValue(string k, object v)
        {
            if (m_table != null)
            {
                m_table.SetValue(k, v);
            }
        }
    }
}


