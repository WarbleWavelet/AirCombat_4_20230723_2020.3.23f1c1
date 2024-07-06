using QFramework;
using QFramework.AirCombat;
using System;



[Obsolete("已过时")]
public class SwitchPlayerController : ControllerBase ,QFramework.IController
{
    private int _id;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void InitChild()
    {
       int planeID = this.GetUtility<IStorageUtil>().Get<int>(DataKeys.PLANE_ID);
        this.SendCommand(new SetSelectedPlaneIDCommand(planeID));
        _id = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform, GameObjectName.Left, () => Switch(ref _id, -1));
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Right, () => Switch(ref _id, 1));
    }

    private void Switch(ref int id, int direction)
    {
        UpdateId(ref id, direction);
        this.SendCommand(new SetSelectedPlaneIDCommand(id));
    }

    private void UpdateId(ref int id, int direction)
    {
        var min = 0;
        var max = this.GetModel<IAirCombatAppModel>().PlaneCount;
        id += direction;
        id = id < 0 ? 0 : id >= max ? id = max - 1 : id;
    }
}