using Firefly.Core.Interface;
using System;
using System.Collections.Generic;

namespace Firefly.Unity.Global
{
    public enum LogLevel
    {
        None  = 0,
        Trace = 1,
        Debug = 2,
        Info  = 3,
        Warn  = 4,
        Error = 5,
        Fatal = 6,
    }

    public class LogAssert
    {
        private static Dictionary<string, ILog> _Loggers = new Dictionary<string, ILog>();

        public static LogLevel Level = LogLevel.None;

        private static ILog _UtilLog = null;
        public static ILog Util
        {
            get
            {
                if (_UtilLog == null)
                    _UtilLog = GetLog("Util");

                return _UtilLog;
            }
        }

        public static ILog GetLog(string name)
        {
            ILog log = null;

            if (!_Loggers.TryGetValue(name, out log))
            {
                log = new UnityLog(name);
                _Loggers.Add(name, log);
            }

            return log;
        }
    }

    public class UnityLog : ILog
    {
        private string _Name;
        public string Name { get { return _Name; } }

        public UnityLog(string name)
        {
            _Name = name;
        }

        public void Debug(string message)
        {
            if (LogAssert.Level >= LogLevel.Debug) return;

            string fmt = string.Format("[{0}]|[Debug]|{1}", Name, message);
            UnityEngine.Debug.Log(fmt);
        }

        public void Debug(string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Debug) return;

            string fmt = string.Format("[{0}]|[Debug]|{1}", Name, message);
            UnityEngine.Debug.LogFormat(fmt, args);
        }

        public void Error(Exception ex)
        {
            if (LogAssert.Level >= LogLevel.Error) return;

            UnityEngine.Debug.LogException(ex);
        }

        public void Error(Exception ex, string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Error) return;

            string fmt = string.Format("[{0}]|[Error]|{1}", Name, message);
            UnityEngine.Debug.LogErrorFormat(fmt, ex);
        }

        public void Fatal(Exception ex)
        {
            if (LogAssert.Level >= LogLevel.Fatal) return;

            UnityEngine.Debug.LogException(ex);
        }

        public void Fatal(Exception ex, string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Fatal) return;

            UnityEngine.Debug.LogException(ex);
        }

        public void Info(string message)
        {
            if (LogAssert.Level >= LogLevel.Info) return;

            string fmt = string.Format("[{0}]|[Info]|{1}", Name, message);
            UnityEngine.Debug.Log(fmt);
        }

        public void Info(string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Info) return;

            string fmt = string.Format("[{0}]|[Info]|{1}", Name, message);
            UnityEngine.Debug.LogFormat(fmt, args);
        }

        public void Trace(string message)
        {
            if (LogAssert.Level >= LogLevel.Trace) return;

            string fmt = string.Format("[{0}]|[Trace]|{1}", Name, message);
            UnityEngine.Debug.Log(fmt);
        }

        public void Trace(string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Trace) return;

            string fmt = string.Format("[{0}]|[Trace]|{1}", Name, message);
            UnityEngine.Debug.LogFormat(fmt, args);
        }

        public void Warn(string message)
        {
            if (LogAssert.Level >= LogLevel.Warn) return;

            string fmt = string.Format("[{0}]|[Warn]|{1}", Name, message);
            UnityEngine.Debug.LogWarning(fmt);
        }

        public void Warn(string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Warn) return;

            string fmt = string.Format("[{0}]|[Warn]|{1}", Name, message);
            UnityEngine.Debug.LogWarningFormat(fmt, args);
        }

        public void Error(string message)
        {
            if (LogAssert.Level >= LogLevel.Error) return;

            string fmt = string.Format("[{0}]|[Error]|{1}", Name, message);
            UnityEngine.Debug.LogError(fmt);
        }

        public void Error(string message, params object[] args)
        {
            if (LogAssert.Level >= LogLevel.Error) return;

            string fmt = string.Format("[{0}]|[Error]|{1}", Name, message);
            UnityEngine.Debug.LogError(fmt);
        }
    }
}
