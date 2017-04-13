using Firefly.Unity.Global;
using SLua;
using System;
using System.Collections.Generic;

namespace Firefly.Unity.Lua
{
    public class LuaBehaviourTable : IDisposable
    {
        public string lua;
        public string behaviour;
        public LuaTable table;

        private Dictionary<string, LuaFunction> m_luaFuncs;
        private bool m_bDisposed;

        public LuaBehaviourTable(string name)
        {
            this.lua = name;
            this.behaviour = name;

            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            int f = name.LastIndexOf("/");
            if (f != -1)
            {
                this.behaviour = name.Substring(f + 1, name.Length - f - 1);
            }

            LuaFunction func = LuaEngine.Instance.GetGlobalFunction("behaviourNew");

            if (func != null)
            {
                this.table = func.call(behaviour) as LuaTable;

                if (this.table == null)
                {
                    LuaEngine.Instance.DoLua(name);

                    this.table = func.call(behaviour) as LuaTable;
                }
            }
            else
            {
                this.table = LuaEngine.Instance.GetTable(name);
            }

            if (this.table == null)
            {
                LogAssert.Util.Warn("Can't get behaiour {0}", this.lua);
            }

            if (this.table != null)
            {
                this.m_luaFuncs = new Dictionary<string, LuaFunction>();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (m_bDisposed)
            {
                return;
            }
            if (table != null)
            {
                if (LuaState.main != null)
                {
                    table.Dispose();
                }
            }
            if (m_luaFuncs != null)
            {
                if (LuaState.main != null)
                {
                    foreach (LuaFunction v in m_luaFuncs.Values)
                    {
                        if (v != null)
                        {
                            v.Dispose();
                        }
                    }
                }
                m_luaFuncs.Clear();
                m_luaFuncs = null;
            }
            m_bDisposed = true;
        }

        ~LuaBehaviourTable()
        {
            Dispose(false);
        }

        public void SetValue(string k, object v)
        {
            if (table != null)
            {
                table[k] = v;
            }
        }

        public LuaFunction GetFunction(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (table == null || LuaState.main == null)
            {
                return null;
            }
            LuaFunction luaFunction;
            if (m_luaFuncs.ContainsKey(name))
            {
                luaFunction = m_luaFuncs[name];
            }
            else
            {
                luaFunction = (table[name] as LuaFunction);
                m_luaFuncs[name] = luaFunction;
            }
            return luaFunction;
        }

        public object Invoke(string name)
        {
            return this.Invoke(this.GetFunction(name));
        }

        public object Invoke(string name, object a1)
        {
            return this.Invoke(this.GetFunction(name), a1);
        }

        public object Invoke(string name, object a1, object a2)
        {
            return this.Invoke(this.GetFunction(name), a1, a2);
        }

        public object Invoke(string name, object a1, object a2, object a3)
        {
            return this.Invoke(this.GetFunction(name), a1, a2, a3);
        }

        public object Invoke(string name, params object[] args)
        {
            return this.Invoke(this.GetFunction(name), args);
        }

        private bool CheckInvoke(LuaFunction func)
        {
            if (table == null || LuaState.main == null)
            {
                return false;
            }
            if (func == null)
            {
                return false;
            }
            return true;
        }

        public object Invoke(LuaFunction func)
        {
            if (!CheckInvoke(func))
            {
                return null;
            }

            object result = null;
            try
            {
                result = func.call(table);
            }
            catch (Exception ex)
            {
                LogAssert.Util.Warn("Invoke {0} failed. {1}", func.Ref, ex.Message);
            }
            return result;
        }

        public object Invoke(LuaFunction func, object a1)
        {
            if (!CheckInvoke(func))
            {
                return null;
            }

            object result = null;
            try
            {
                result = func.call(table, a1);
            }
            catch (Exception ex)
            {
                LogAssert.Util.Error(ex, "Invoke failed. {0}", func.Ref);
            }
            return result;
        }

        public object Invoke(LuaFunction func, object a1, object a2)
        {
            if (!CheckInvoke(func))
            {
                return null;
            }

            object result = null;
            try
            {
                result = func.call(table, a1, a2);
            }
            catch (Exception ex)
            {
                LogAssert.Util.Error(ex, "Invoke failed. {0}", func.Ref);
            }
            return result;
        }

        public object Invoke(LuaFunction func, object a1, object a2, object a3)
        {
            if (!CheckInvoke(func))
            {
                return null;
            }

            object result = null;
            try
            {
                result = func.call(table, a1, a2, a3);
            }
            catch (Exception ex)
            {
                LogAssert.Util.Error(ex, "Invoke failed. {0}", func.Ref);
            }
            return result;
        }

        public object Invoke(LuaFunction func, params object[] args)
        {
            if (table == null || LuaState.main == null)
            {
                return null;
            }
            if (func == null)
            {
                return null;
            }
            object result = null;
            try
            {
                result = func.call(table, args);
            }
            catch (Exception ex)
            {
                LogAssert.Util.Error(ex, "Invoke failed. {0}", func.Ref);
            }
            return result;
        }
    }
}