using QFramework.AirCombat;
using QFramework;

public class ShieldController : ControllerBase, QFramework.IController
{
    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Icon , ()=>
        {
            this.SendCommand<UseShieldCommand>();
        });
    }


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}