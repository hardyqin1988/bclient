using Firefly.Core.Variant;
using Firefly.Network;
using Firefly.Unity.Asset;
using Firefly.Unity.Databind;
using Firefly.Unity.Stage;
using Firefly.Unity.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginStage : StageBase
{
    public override IEnumerator OnLoad()
    {
        bool load_finish = false;
        AssetEngine.Instance.LoadScene("login", (is_done, progress)=>
        {
            load_finish = is_done;
        });

        while (!load_finish)
        {
            yield return null;
        }

        UIEngine.Instance.ShowPanel<LoginPanel>("login_panel");
    }

    public override IEnumerator OnUnload()
    {
        AsyncOperation oper = SceneManager.UnloadSceneAsync("login");
        while (!oper.isDone)
        {
            yield return null;
        }

        UIEngine.Instance.HidePanel<LoginPanel>();
    }
}
