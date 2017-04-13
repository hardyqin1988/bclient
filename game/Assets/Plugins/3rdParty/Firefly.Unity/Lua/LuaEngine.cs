using Firefly.Core.Config;
using Firefly.Unity.Global;
using Firefly.Unity.Utility;
using System.Collections;
using UnityEngine;
using System;
using SLua;
using System.Collections.Generic;
using System.Text;
using Firefly.Unity;

namespace Firefly.Unity.Lua
{
    public class LuaEngine : SingletonEngine<LuaEngine>, IEngine
    {
        protected override IEnumerator Startup()
        {
            Logger.Debug("LuaManager init start ...");

            _LuaFiles.Clear();
            _LuaFuncs.Clear();

            if (_LuaSvr != null)
            {
                if (_LuaSvr.luaState != null)
                {
                    _LuaSvr.luaState.Dispose();
                    _LuaSvr.luaState = null;
                }
                _LuaSvr = null;
            }

            _LuaSvr = new LuaSvr();

            _LuaSvr.init(null, delegate
            {
                Logger.Debug("Lua init finished.");
            }, LuaSvrFlag.LSF_BASIC);

            foreach (var kv in _LuaGlobalValues)
            {
                LuaDLL.lua_pushstring(_LuaSvr.luaState.L, kv.Key);
                LuaDLL.lua_setglobal(_LuaSvr.luaState.L, kv.Value);
            }

            _LuaSvr.luaState.doString("require \"Global\"");

            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Shutdown()
        {
            if (_LuaSvr != null)
            {
                if (_LuaSvr.luaState != null)
                {
                    _LuaSvr.luaState.Dispose();
                    _LuaSvr.luaState = null;
                }
                _LuaSvr = null;
            }

            yield return new WaitForFixedUpdate();
        }

        public delegate byte[] LoaderDelegate(string fn);
        public static LoaderDelegate loaderDelegate;

        private LuaSvr                          _LuaSvr                   = null;
        private Dictionary<string, byte[]>      _LuaFiles                 = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
        private Dictionary<string, LuaFunction> _LuaFuncs                 = new Dictionary<string, LuaFunction>();
        private Dictionary<string, string>      _LuaGlobalValues          = new Dictionary<string, string>();
        private List<LuaBehaviour>              _UpdateLuaBehaviours      = new List<LuaBehaviour>();
        private List<LuaBehaviour>              _LateUpdateLuaBehaviours  = new List<LuaBehaviour>();

        public override void Awake()
        {
            base.Awake();

            LuaState.loaderDelegate = delegate (string name)
            {
                name = name.Replace(".", "/");
                return GetLuaBytes(name);
            };
            LuaState.logDelegate = delegate (string msg)
            {
                Logger.Debug("[LUA]: {0}\n", msg);
            };
            LuaState.errorDelegate = delegate (string msg)
            {
                Logger.Warn("[LUA]: {0}\n", msg);
            };
        }

        private byte[] GetLuaBytesForce(string name)
        {
            if (loaderDelegate != null)
            {
                return loaderDelegate(name);
            }
            else
            {
                byte[] found = null;

#if UNITY_EDITOR
                string fullPath = string.Format("{0}{1}.lua", AssetPath.LuaPath, name);
                found = FileUtil.GetFileBytes(fullPath);
#endif
                return found;
            }
        }

        public byte[] GetLuaBytes(string name)
        {
            byte[] found = null;

            if (_LuaFiles.TryGetValue(name, out found))
            {
                return found;
            }

            found = GetLuaBytesForce(name);

            _LuaFiles[name] = found;

            return found;
        }

        public void LuaGC()
        {
            LuaDLL.lua_gc(_LuaSvr.luaState.L, LuaGCOptions.LUA_GCCOLLECT, 0);
        }

        public LuaFunction GetFunction(byte[] bytes, string name = null)
        {
            object obj;
            if (_LuaSvr.luaState.doBuffer(bytes, name, out obj))
            {
                return obj as LuaFunction;
            }
            Logger.Warn("do lua function {0} failed.", name);
            return null;
        }

        public LuaTable GetTable(byte[] bytes, string name = null)
        {
            object obj;
            if (_LuaSvr.luaState.doBuffer(bytes, name, out obj))
            {
                return obj as LuaTable;
            }
            Logger.Warn("do lua table {0} failed.", name);
            return null;
        }

        public LuaTable GetTable(string name)
        {
            byte[] bytes = GetLuaBytes(name);
            if (bytes != null)
            {
                return GetTable(bytes, name);
            }
            return null;
        }

        public void DoLua(string name)
        {
            _LuaSvr.luaState.doString(string.Format("require \"{0}\"", name));
        }

        public LuaFunction GetGlobalFunction(string name)
        {
            LuaFunction luaFunction;

            if (!_LuaFuncs.ContainsKey(name))
            {
                luaFunction = GetFunction(Encoding.Default.GetBytes("return " + name), null);
                _LuaFuncs[name] = luaFunction;
            }
            else
            {
                luaFunction = _LuaFuncs[name];
            }

            return luaFunction;
        }

        public void AddGlobalValue(string k, string v)
        {
            _LuaGlobalValues.Add(k, v);

            LuaDLL.lua_pushstring(_LuaSvr.luaState.L, v);
            LuaDLL.lua_setglobal(_LuaSvr.luaState.L, k);
        }

        public void ClearGlobalValues()
        {
            _LuaGlobalValues.Clear();
        }

        public void OnUpdate(float deltaTime)
        {
            for (int i = 0; i < _UpdateLuaBehaviours.Count; i++)
            {
                LuaBehaviour b = _UpdateLuaBehaviours[i];
                if (b != null)
                {
                    b.OnUpdate(deltaTime);
                }
            }
        }

        public void OnLateUpdate()
        {
            for (int i = 0; i < _LateUpdateLuaBehaviours.Count; i++)
            {
                LuaBehaviour b = _LateUpdateLuaBehaviours[i];
                if (b != null)
                {
                    b.OnLateUpdate();
                }
            }
        }

        public void AddUpdateBehaviour(LuaBehaviour luaBehaviour)
        {
            if (_UpdateLuaBehaviours.Contains(luaBehaviour))
            {
                return;
            }
            _UpdateLuaBehaviours.Add(luaBehaviour);
        }

        public void RemoveUpdateBehaviour(LuaBehaviour luaBehaviour)
        {
            _UpdateLuaBehaviours.Remove(luaBehaviour);
        }

        public void AddLateUpdateBehaviour(LuaBehaviour luaBehaviour)
        {
            if (_LateUpdateLuaBehaviours.Contains(luaBehaviour))
            {
                return;
            }
            _LateUpdateLuaBehaviours.Add(luaBehaviour);
        }

        public void RemoveLateUpdateBehaviour(LuaBehaviour luaBehaviour)
        {
            _LateUpdateLuaBehaviours.Remove(luaBehaviour);
        }
    }
}
