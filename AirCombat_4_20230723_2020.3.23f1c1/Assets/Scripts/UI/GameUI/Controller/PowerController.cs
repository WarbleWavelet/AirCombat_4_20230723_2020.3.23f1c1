using QFramework.AirCombat;
using QFramework;

public class PowerController : ControllerBase, QFramework.IController
{
    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Icon, Click);
    }

    private void Click()
    {
#if QFRAMEWORK
        if (this.GetModel<IAirCombatAppModel>().BombCount > 0)
        {
            this.GetModel<IAirCombatAppModel>().BombCount.Value--;
            this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_USE_BOMB);
        }
#else
        if (GameModel.Single.PowerCount > 0)
        {
            GameModel.Single.PowerCount--;
            this.GetSystem<IMessageSystem>().DispatchMsg(MsgEvent.EVENT_USE_BOMB);
        }
#endif
    }

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}