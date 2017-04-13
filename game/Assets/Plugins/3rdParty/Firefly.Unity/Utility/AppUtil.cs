using Firefly.Unity.Localization;

namespace Firefly.Unity.Utility
{
    public static class AppUtil
    {
        public static string GetLocalization(string text)
        {
            return L10NEngine.Instance.GetText(text);
        }
    }
}
