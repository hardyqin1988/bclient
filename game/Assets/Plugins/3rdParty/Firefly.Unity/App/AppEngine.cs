using Firefly.Core.Config.INI;
using Firefly.Core.Interface;
using Firefly.Network;
using Firefly.Network.Encryption;
using Firefly.Network.Opcode;
using Firefly.Unity.Global;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Unity.App
{
    public abstract class AppBase
    {
        protected ILog Logger;

        public abstract void OnAwake(Configuration app_ini);

        public abstract void OnOpen();

        public abstract void OnClose();

        public virtual void OnLoop() { }

        public AppBase()
        {
            Logger = LogAssert.GetLog(GetType().Name);
        }
    }

    public class AppEngine : SingletonEngine<AppEngine>, IEngine
    {
        private Configuration _Ini;

        private Dictionary<string, AppBase> _AppDic = null;

        public override void Awake()
        {
            base.Awake();

            _AppDic = new Dictionary<string, AppBase>();
        }

        APP CreateApp<APP>() where APP : AppBase
        {
            Type type = typeof(APP);
            if (_AppDic.ContainsKey(type.Name))
            {
                LogAssert.Util.Warn("{0} App is exist.", type.Name);
                return null;
            }

            APP app = Activator.CreateInstance<APP>();
            app.OnAwake(_Ini);

            return app;
        }

        protected override void Loop()
        {
            Dispatch.OnLoop();

            Userinfo.OnLoop();

            Network.OnLoop();
        }

        protected override IEnumerator Startup()
        {
            _Ini = Configuration.LoadFromFile(AssetPath.AppIni);

            yield return new WaitForFixedUpdate();

            Dispatch = CreateApp<DispatchApp>();
            Userinfo = CreateApp<UserinfoApp>();
            Network = CreateApp<NetworkApp>();
            yield return new WaitForFixedUpdate();

            Dispatch.OnOpen();
            Userinfo.OnOpen();
            Network.OnOpen();
        }

        protected override IEnumerator Shutdown()
        {
            Dispatch.OnClose();
            yield return new WaitForFixedUpdate();

            Userinfo.OnClose();
            yield return new WaitForFixedUpdate();

            Network.OnClose();
            yield return new WaitForFixedUpdate();
        }

        public UserinfoApp      Userinfo        { get; private set; }
        public NetworkApp       Network         { get; private set; }
        public DispatchApp      Dispatch        { get; private set; }

        public void Login()
        {
            bool sdk_cert = Userinfo.SdkCert;
            string uuid = Userinfo.Uuid;
            string token = Userinfo.SdkToken;

            Network.SendSystemMsg(SystemOpcode.CLIENT.LOGIN_VALIDATE_REQUEST)
                .AppendBool(sdk_cert)
                .AppendString(uuid)
                .AppendString(token);

            Logger.Debug("Login: {0}\r\n{1}\r\n{2}", sdk_cert, uuid, token);
        }
    }
}

