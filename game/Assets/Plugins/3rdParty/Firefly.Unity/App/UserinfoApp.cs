using Firefly.Core.Config.INI;
using Firefly.Core.Data;
using System;
using UnityEngine;

namespace Firefly.Unity.App
{
    public class UserinfoApp : AppBase
    {
        public override void OnAwake(Configuration app_ini)
        {
            SdkCert = false;
            try
            {
                Uuid = app_ini["account"]["uuid"].StringValue;
                Logger.Debug(app_ini["account"]["uuid"].StringValue);
                if (string.IsNullOrEmpty(Uuid))
                {
                    Uuid = SystemInfo.deviceUniqueIdentifier;
                }
            }
            catch (Exception ex)
            {
                throw new INIException("UserApp read uuid fail", ex);
            }
        }

        public override void OnClose()
        {
        }

        public override void OnOpen()
        {
        }

        public bool SdkCert { get; set; }

        public string SdkToken { get; set; }

        public string Token
        {
            get
            {
                return GetToken(Uuid);
            }
            set
            {
                SetToken(Uuid, value);
            }
        }

        public string Uuid
        {
            get
            {
                return GetUuid();
            }
            set
            {
                SetUuid(value);
            }
        }

        public string GetToken(string uuid)
        {
            return PlayerPrefs.GetString("uuid_" + uuid);
        }

        public void SetToken(string uuid, string token)
        {
            PlayerPrefs.SetString("uuid_" + uuid, token);
            PlayerPrefs.Save();
        }

        public string SessionKey
        {
            get
            {
                return GetSessionKey();
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    SetSessionKey(value);
                }
            }
        }

        public void SetEntity(PersistID pid, string json)
        {
            PlayerPrefs.SetString(pid.ToString(), json);
        }

        public string GetEntity(PersistID pid)
        {
            return PlayerPrefs.GetString(pid.ToString());
        }

        public string GetUuid()
        {
            return PlayerPrefs.GetString("uuid");
        }

        public string GetSessionKey()
        {
            return PlayerPrefs.GetString("session_key");
        }

        public void SetUuid(string uuid)
        {
            PlayerPrefs.SetString("uuid", uuid);
            PlayerPrefs.Save();
        }

        public void SetSessionKey(string session_key)
        {
            PlayerPrefs.SetString("session_key", session_key);
            PlayerPrefs.Save();
        }
    }
}
