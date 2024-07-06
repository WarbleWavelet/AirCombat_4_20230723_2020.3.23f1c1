using QFramework;
using QFramework.AirCombat;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarComponent : ViewBase , QFramework.IController
{
    private List<LifeItemComponent> _itemLst;

    protected override void InitChild()
    {
        _itemLst = new List<LifeItemComponent>();
        InitItem();
        UpdateLife();

        GameObject.DontDestroyOnLoad(gameObject);

    }

    public override void Show()
    {
        base.Show();
        this.GetSystem<IMessageSystem>().AddListener(MsgEvent.EVENT_HP, ReceiveMessage);
    }

    public override void Hide()
    {
        base.Hide();
        this.GetSystem<IMessageSystem>().RemoveListener(MsgEvent.EVENT_HP, ReceiveMessage);
    }


    private void InitItem()
    {
        GameObject go = null;
        for (var i = 0; i < Const.LIFE_ITEM_NUM; i++)
        {
          
            go = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_LIFE_ITEM_VIEW, LoadType.RESOURCES);
            go.SetParent(transform);
            go.name = i.ToString();
            _itemLst.Add(go.AddComponent<LifeItemComponent>());
        }
    }

    private void UpdateLife()
    {
       UiUtilData uiUtil = UiUtil.Get(GameObjectName.Value);
        uiUtil.SetText(this.GetModel<IAirCombatAppModel>().Life); //这里不能打断点，会loop 
       
     
    }

    public void ReceiveMessage(params object[] args)
    {
        UpdateLife();
    }

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}