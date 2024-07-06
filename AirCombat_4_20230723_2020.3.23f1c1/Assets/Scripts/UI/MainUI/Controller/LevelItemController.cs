using QFramework.AirCombat;
using QFramework;
/// <summary>关卡选择</summary>
public class LevelItemController : ControllerBase, QFramework.IController
{

    protected override void InitChild()
    {
        int id = transform.GetSiblingIndex();


        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Enter, () =>
        {

            this.SendCommand(new OpenLoadingPanelCommand(id));
        });

        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Mask, () =>
        {
            this.SendCommand(new OpenDialogPanelCommand("当前关卡未开放"));
        });

    }


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}