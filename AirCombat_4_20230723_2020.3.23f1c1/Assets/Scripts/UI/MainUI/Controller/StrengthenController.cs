using QFramework;
using QFramework.AirCombat;

[BindPrefabAttribute(ResourcesPath.PREFAB_STRENGTHEN_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class StrengthenController : ControllerBase  ,QFramework.IController
{
    protected override void InitChild()
    {
        transform.Find("Switchplayer").gameObject.AddComponent<SwitchPlayerController>();
        transform.Find("Property").gameObject.AddComponent<PlanePropertyController>();
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,"Upgrades/Upgrades", Upgrades);
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,"Back", this.GetSystem<IUISystem>().Back);
    }

    private void Upgrades()
    {
        //判断是否能够升级
        //花费是否足够 当前等级是否超限
        var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES + DataKeys.COST_UNIT);
        var value = this.GetUtility<IStorageUtil>().Get<string>(key);

        key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES + this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel);
        var cost = this.GetUtility<IStorageUtil>().Get<int>(key);

        var money = this.GetModel<IAirCombatAppStateModel>().GetMoney(value);

        var levelMax = this.GetModel<IAirCombatAppModel>().SelectedPlaneSpriteLevelMax; //这个有,直接到model取

        if (money >= cost && this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel < levelMax)
        {
            ChangeMenoy(value, cost);
            ChangeLevel();
            ChangeData();
        }
        else
        {
            this.SendCommand(new OpenDialogPanelCommand("你没钻石了！"));
        }
    }


    /// <summary>cost是变化值</summary>
    private void ChangeMenoy(string costUnit, int cost)
    {
        var money = this.GetModel<IAirCombatAppStateModel>().GetMoney(costUnit);
        this.GetModel<IAirCombatAppStateModel>().SetMoney(costUnit, money - cost);
    }

    private void ChangeLevel()
    {
        var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES_RATIO);
        int level = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel;
        level++;
        this.GetUtility<IStorageUtil>().Set(key, level);
    }

    private void ChangeData()
    {
        //获取升级系数，修改数据
        var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES_RATIO);
        var ratio = this.GetUtility<IStorageUtil>().Get<int>(key);

        ChangeData(ratio, PropertyItem.ItemKey.value, PlaneProperty.Property.attack);
        ChangeData(ratio, PropertyItem.ItemKey.maxVaue, PlaneProperty.Property.attack);
        ChangeData(ratio, PropertyItem.ItemKey.grouth, PlaneProperty.Property.attack);
        //
        ChangeData(ratio, PropertyItem.ItemKey.value, PlaneProperty.Property.life);
        ChangeData(ratio, PropertyItem.ItemKey.maxVaue, PlaneProperty.Property.life);
        ChangeData(ratio, PropertyItem.ItemKey.grouth, PlaneProperty.Property.life);
    }


    private void ChangeData(int ratio, PropertyItem.ItemKey itemKey, PlaneProperty.Property property)
    {
        var key = this.GetUtility<IKeysUtil>().GetNewKey(itemKey, property.ToString());
        var value = this.GetUtility<IStorageUtil>().Get<int>(key);
        value *= ratio;
        this.GetUtility<IStorageUtil>().Set(key, value);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}