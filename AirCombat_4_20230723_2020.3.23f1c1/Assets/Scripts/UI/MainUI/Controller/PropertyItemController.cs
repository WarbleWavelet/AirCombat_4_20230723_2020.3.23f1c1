using QFramework;
using QFramework.AirCombat;

public class PropertyItemController : ControllerBase ,QFramework.IController
{
    private string _key;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    public void Init(string key)
    {
        _key = key;
    }

    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,"Add", AddAction);
    }

    private void AddAction()
    {
        var key = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(_key + DataKeys.COST_UNIT);
        var unit = this.GetUtility<IStorageUtil>().Get<string>(key);
        var money =this.GetModel<IAirCombatAppStateModel>().GetMoney(unit);

        key = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.cost, _key);
        var cost = this.GetUtility<IStorageUtil>().Get<int>(key);


        if (money >= cost)
        { 
            ChangeData();
        }
        else
        { 
            this.SendCommand(new OpenDialogPanelCommand("你没星星了！")) ;
        }
    }

    private void ChangeData()
    {
        var valueKey = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.value, _key);
        var value = GetValue(valueKey);
        var grouthKey = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.grouth, _key);
        var grouth = GetValue(grouthKey);
        value += grouth;

        this.GetUtility<IStorageUtil>().SetObject(valueKey, value);
    }

    private int GetValue(string key)
    {
        return this.GetUtility<IStorageUtil>().Get<int>(key);
    }
}