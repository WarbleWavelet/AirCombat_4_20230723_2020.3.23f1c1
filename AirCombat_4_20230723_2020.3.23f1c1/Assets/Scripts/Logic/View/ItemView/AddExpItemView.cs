using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddExpItemView : ItemInPlaneLevelViewBase, ICanGetModel   ,ICanSendCommand
{

    public override EItemType E_ItemType { get { return EItemType.ADD_EXP; } }
    protected override IEffectContainer GetEffectContainer()
    {
        return SEffectContainerFactory.New(E_ItemType);
    }

    protected override string GetGameAudio()
    {
        return AudioGame.Get_Item;
    }

    protected override void ItemLogic()
    {
        base.ItemLogic();

         int after=  this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel+1;
        this.SendCommand(new SetPlaneSpriteLevelCommand(after));
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_GAME_EXP_LEVEL_UP);
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_GAME_UPDATE_LEVEL);


        STest.LogItemName(SpritePath());
    }

    protected override string SpritePath()
    {
        return ResourcesPath.PICTURE_ADD_EXP;
    }


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}
