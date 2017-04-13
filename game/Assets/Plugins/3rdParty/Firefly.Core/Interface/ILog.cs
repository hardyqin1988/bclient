using System;

namespace Firefly.Core.Interface
{
    public interface ILog
    {
        string Name { get; }
        void Debug(string message);
        void Debug(string message, params object[] args);

        void Info(string message);
        void Info(string message, params object[] args);

        void Trace(string message);
        void Trace(string message, params object[] args);

        void Warn(string message);
        void Warn(string message, params object[] args);

        void Error(Exception ex);
        void Error(Exception ex, string message, params object[] args);

        void Fatal(Exception ex);
        void Fatal(Exception ex, string message, params object[] args);
    }
}
