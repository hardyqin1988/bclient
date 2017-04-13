using Firefly.Unity.Global;
using Firefly.Unity.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Firefly.Unity.Localization
{
    public class L10NEngine : SingletonEngine<L10NEngine>, IEngine
    {
        private Dictionary<SystemLanguage, L10N> _Localizations = null;
        private L10N _Localization  = null;

        public override void Awake()
        {
            base.Awake();
            _Localizations = new Dictionary<SystemLanguage, L10N>();
        }

        protected override IEnumerator Shutdown()
        {
            yield return new WaitForFixedUpdate();
        }

        protected override IEnumerator Startup()
        {
            SetLanguage(Application.systemLanguage);
            yield return new WaitForFixedUpdate();
        } 

        public void SetLanguage(SystemLanguage language)
        {
            if (!FileUtil.Exists(AssetPath.LocalizationPath + language.ToString() + ".language"))
            {
                language = SystemLanguage.ChineseSimplified;
            }

            if (_Localization != null && _Localization.Language == language)
            {
                return;
            }

            L10N localization = null;

            if (!_Localizations.TryGetValue(language, out localization))
            {
                localization = new L10N(language);
                localization.LoadTexts();
                _Localizations.Add(language, localization);
            }

            _Localization = localization;

            if (GameClient.Instance.ChangeLanguage != null) GameClient.Instance.ChangeLanguage();
        }

        public string GetText(string key)
        {
            return _Localization.GetText(key);
        }

        public string GetFormatText(string key, params object[] args)
        {
            string text = _Localization.GetText(key);
            text = string.Format(text, args);
            return text;
        }
    }
}

