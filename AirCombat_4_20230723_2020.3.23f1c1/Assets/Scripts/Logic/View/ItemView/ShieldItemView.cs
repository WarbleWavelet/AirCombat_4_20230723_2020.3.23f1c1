using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>右下角的护盾</summary>
public class ShieldItemView : ItemInPlaneLevelViewBase, QFramework.IController
{
    public override EItemType E_ItemType { get { return EItemType.SHIELD; } }
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
        return ResourcesPath.PICTURE_SHIELD;
    }

    protected override void ItemLogic()
    {
        base.ItemLogic();
        this.GetModel<IAirCombatAppModel>().ShieldCount.Value++;
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_SHIELD);

        STest.LogItemName(SpritePath());
    }
    



    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
