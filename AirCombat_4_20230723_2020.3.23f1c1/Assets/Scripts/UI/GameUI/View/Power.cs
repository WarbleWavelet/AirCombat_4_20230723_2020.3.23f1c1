using QFramework.AirCombat;
using QFramework;
using UnityEngine;

public class Power : ViewBase, QFramework.IController
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
        this.GetModel<IAirCombatAppModel>().BombCount.RegisterWithInitValue(powerCount =>
        {
            if (powerCount == 0)
            {
                _cdEffect.SetMask();
                _itemEffect.Hide();
            }
            else if (powerCount > 0)
            {
                _cdEffect.SetShow();
                _itemEffect.Show();
            }
           //
            UiUtil.Get(GameObjectName.Num).SetText(powerCount);
        }).UnRegisterWhenGameObjectDestroyed(this);

        //
        this.GetModel<IAirCombatAppModel>().Life.RegisterWithInitValue(shieldCount =>
        {
            if (shieldCount > 0)
            {
                _cdEffect.StartCD(() => _itemEffect.Show());
            }

            _itemEffect.Hide();
        });
        //
        this.GetModel<IAirCombatAppStateModel>().HandMode.RegisterWithInitValue(handModel =>
        {
            this.GetUtility<IBattleUtil>().ChangeHandPos(transform.Rect(),handModel);
        }).UnRegisterWhenGameObjectDestroyed(this);

    }








    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}