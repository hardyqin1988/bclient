using BiuBiu.Common.Opcode;
using Firefly.Core.Variant;
using Firefly.Unity.App;
using Firefly.Unity.UI;
using Firefly.Unity.Utility;
using UnityEngine.UI;

public class LoginPanel : PanelBase
{
    protected override void OnCreate()
    {
        /*UnityUtil.GetComponent<Button>(PanelObject, "login_btn").onClick.AddListener(() =>
        {
            AppEngine.Instance.Login();
        });*/

        UnityUtil.GetComponent<Button>(PanelObject, "btn_join_arena").onClick.AddListener(() =>
        {
            AppEngine.Instance.Network.SendBattleMsg(VariantList.New().Append(ArenaOpcode.CLIENT.JOIN_SCUFFLE_ARENA));
        });
    }

    protected override void OnDestroy()
    {
    }

    protected override void OnShow()
    {
    }

    protected override void OnHide()
    {
    }
}
