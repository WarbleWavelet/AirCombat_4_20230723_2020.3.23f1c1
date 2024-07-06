using QFramework.AirCombat;
using QFramework;

[BindPrefabAttribute(ResourcesPath.PREFAB_SETTING_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class SettingController : ControllerBase, QFramework.IController
{
    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Exit, Exit);
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Continue, Continue);
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Hand, ChangeHand);
    }

    public override void Show()
    {
        base.Show();

        this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.PAUSE;
    }

    private void Exit()
    {
        this.GetSystem<IUISystem>().Back();
        this.GetModel<IAirCombatAppStateModel>().TarScene.Value = ESceneName.Main;
        this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_LOADING_VIEW);
    }

    private void Continue()
    {
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.CONTINUE;

        this.GetSystem<IUISystem>().Back();
    }

    private void ChangeHand()
    {
        var hand = this.GetModel<AirCombatAppStateModel>().HandMode;
        this.GetModel<AirCombatAppStateModel>().HandMode.Value = (hand == HandMode.LEFT) ? HandMode.RIGHT : HandMode.LEFT;


        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_CHANGE_HAND);
    }

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}