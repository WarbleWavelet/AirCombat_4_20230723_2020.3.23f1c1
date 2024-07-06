using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>右下角的炸弹,Power Bomb</summary>
public class PowerItemView : ItemInPlaneLevelViewBase, QFramework.IController
{
    public override EItemType E_ItemType { get { return EItemType.POWER; } }
    #region pro
    protected override IEffectContainer GetEffectContainer()
    {
        return SEffectContainerFactory.New(E_ItemType);
    }

    protected override string GetGameAudio()
    {
        return AudioGame.Get_Item;
    }

    protected override string SpritePath()
    {
        return ResourcesPath.PICTURE_POWER;
    }
    
    protected override void ItemLogic()
    {
        base.ItemLogic();
        this.GetModel<IAirCombatAppModel>().BombCount.Value++;
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_POWER);
        STest.LogItemName(SpritePath());
    }
    #endregion  

    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
