using QFramework.AirCombat;
using QFramework;

[BindPrefabAttribute(ResourcesPath.PREFAB_SETTING_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class SettingView : ViewBase, QFramework.IController
{
    protected override void InitChild()
    {
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        ChangeHand();
    }

    private void ChangeHand()
    {
        HandMode hand;


#if QFRAMEWORK
        hand = this.GetModel<IAirCombatAppStateModel>().HandMode;
# else
        hand= GameStateModel.Single.HandMode;
#endif
        var text = (hand == HandMode.LEFT) ? Const_CHI.RightHand : Const_CHI.LeftHand;
        UiUtil.Get(GameObjectPath.Hand_Text).SetText(text);
    }                                                                                      

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}