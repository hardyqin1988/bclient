using Firefly.Core.Config.INI;
using Firefly.Unity.Global;
using System.Collections;
using UnityEngine;

namespace Firefly.Unity.Native
{
    public partial class NativeEngine : SingletonEngine<NativeEngine>, IEngine
    {
        private Configuration _Ini;

        public bool SDKLoginSuccess = false;

        protected override IEnumerator Startup()
        {
            _Ini = Configuration.LoadFromFile(AssetPath.SDKIni);

            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        //初始化配置文件
        void InitConfig()
        {
            InitGAConfig();
        }

        void InitGAConfig()
        {
#if UNITY_ANDROID && !UNITY_EDITOR

            _GAAppKey = _Ini["umeng_ga_android"]["appkey"].StringValue;
            _GAChannelID = _Ini["umeng_ga_android"]["channel_id"].StringValue;
#elif UNITY_IOS && !UNITY_EDITOR
            _GAAppKey = _Ini["umeng_ga_ios"]["appkey"].StringValue;
            _GAChannelID = _Ini["umeng_ga_ios"]["channel_id"].StringValue;
#endif
        }
    }
}
