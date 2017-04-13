using Umeng;

namespace Firefly.Unity.Native
{
    public partial class NativeEngine
    {
        private string _GAAppKey = "";
        private string _GAChannelID = "";

        void InitGA()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        GA.StartWithAppKeyAndChannelId(_GAAppKey, _GAChannelID);
#elif UNITY_IOS && !UNITY_EDITOR
        GA.StartWithAppKeyAndChannelId(_GAAppKey, _GAChannelID);
#endif
        }
    }
}

