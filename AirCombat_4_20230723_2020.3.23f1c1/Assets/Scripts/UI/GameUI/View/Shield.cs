using QFramework.AirCombat;
using QFramework;

public class Shield : ViewBase, QFramework.IController
{
    private ItemCDEffect _cdEffect;
    private ItemEffect _itemEffect;

    protected override void InitChild()
    {
        var effect = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_ITEM_EFFECT_VIEW, LoadType.RESOURCES);
        effect.SetParent(transform);
        _itemEffect = effect.AddComponent<ItemEffect>();
        _cdEffect = UiUtil.Get(GameObjectName.Mask).Add<ItemCDEffect>();
        _cdEffect.SetShow();
    }

    public override void Show()
    {
        base.Show();
        this.RegisterEvent<UseShieldEvent>(_=>
        {
            _cdEffect.StartCD(() => 
            {
                _itemEffect.Show();
            });
            _itemEffect.Hide();
        });
        this.GetModel<IAirCombatAppModel>().ShieldCount.RegisterWithInitValue(shieldCount =>
        {
            UiUtil.Get(GameObjectName.Num).SetText(shieldCount);
            //
            if (shieldCount == 0)
            {
                _cdEffect.SetMask();
                _itemEffect.Hide();
            }
            else if (shieldCount > 0)
            {
                _cdEffect.SetShow();
                _itemEffect.Show();
            }
        }).UnRegisterWhenGameObjectDestroyed(this);

        this.GetModel<IAirCombatAppStateModel>().HandMode.RegisterWithInitValue(handMode =>
        {
            this.GetUtility<IBattleUtil>().ChangeHandPos(transform.Rect(), handMode);

        }).UnRegisterWhenGameObjectDestroyed(this);
    }


    public override void Hide()
    {
        base.Hide();
    }





    private void UseShield(params object[] args)
    {
        UpdateEffect();
    }

    private void UpdateEffect()
    {

    }







    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}