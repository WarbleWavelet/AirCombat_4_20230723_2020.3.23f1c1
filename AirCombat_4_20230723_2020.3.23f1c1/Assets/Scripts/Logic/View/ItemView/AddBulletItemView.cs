using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBulletItemView : ItemInPlaneLevelViewBase,IBuffCarrier 
{

    public override EItemType E_ItemType { get { return EItemType.ADD_BULLET; } }
    public BuffType Type
    {
        get { return BuffType.LEVEL_UP; }
    }

    protected override void ItemLogic()
    {
        base.ItemLogic();
        STest.LogItemName(SpritePath());
    }
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
        return ResourcesPath.PICTURE_ADD_BULLET;
    }

}
