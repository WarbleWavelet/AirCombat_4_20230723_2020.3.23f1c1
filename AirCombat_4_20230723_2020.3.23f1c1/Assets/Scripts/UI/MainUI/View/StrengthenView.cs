using QFramework;
using QFramework.AirCombat;

[BindPrefabAttribute(ResourcesPath.PREFAB_STRENGTHEN_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class StrengthenView : ViewBase,QFramework.IController
{
    protected override void InitChild()
    {
        UiUtil.Get(GameObjectName.Switchplayer).GameObject.AddComponent<SwitchPlayerView>();
        UiUtil.Get(GameObjectName.Property).GameObject.AddComponent<PlaneProperty>();
        UiUtil.Get(GameObjectName.Money).GameObject.AddComponent<Money>();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        UpdateLevelView();
    }

    private void UpdateLevelView()
    {
        //名称
        var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES + DataKeys.NAME);
        var data = this.GetUtility<IStorageUtil>().Get<string>(key);
        UiUtil.Get(GameObjectPath.Upgrades_Text).SetText(data);
        //花费
        key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.LEVEL);
        var level = this.GetUtility<IStorageUtil>().Get<int>(key);
        key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES + level);
        var cost = this.GetUtility<IStorageUtil>().Get<int>(key);
        UiUtil.Get(GameObjectPath.Upgrades_Upgrades_Text).SetText(cost);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}