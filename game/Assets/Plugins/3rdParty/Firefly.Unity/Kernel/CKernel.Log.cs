using System;

namespace Firefly.Unity.Kernel
{
    partial class CKernel
    {
        public void Debug(string message)
        {
            _Logger.Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            _Logger.Debug(message, args);
        }

        public void Error(Exception ex)
        {
            _Logger.Error(ex);
        }

        public void Error(Exception ex, string message, params object[] args)
        {
            _Logger.Error(ex, message, args);
        }

        public void Fatal(Exception ex)
        {
            _Logger.Fatal(ex);
        }

        public void Fatal(Exception ex, string message, params object[] args)
        {
            _Logger.Fatal(ex, message, args);
        }

        public void Info(string message)
        {
            _Logger.Info(message);
        }

        public void Info(string message, params object[] args)
        {
            _Logger.Info(message, args);
        }

        public void Trace(string message)
        {
            _Logger.Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            _Logger.Trace(message, args);
        }

        public void Warn(string message)
        {
            _Logger.Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            _Logger.Warn(message, args);
        }
    }

}
