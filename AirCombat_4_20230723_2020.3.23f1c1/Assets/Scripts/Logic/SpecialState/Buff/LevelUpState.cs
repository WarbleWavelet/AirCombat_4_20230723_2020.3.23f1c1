using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpState : IBuff ,QFramework.IController
{
    private const float TIME_ONCE = 20;
    private int _level;
    private int _coroutineID = -1;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    public void Start()
    {
        this.GetSystem<IMessageSystem>().AddListener(MsgEvent.EVENT_GAME_EXP_LEVEL_UP,LevelUpByExp);
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_GAME_UPDATE_LEVEL);

        _coroutineID = this.GetSystem<IDelayDetalCoroutineSystem>().Start(TIME_ONCE, BeginCallBack, EndCallBack, _coroutineID);
        int after = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel + 1  ;
        this.SendCommand(new SetPlaneSpriteLevelCommand( after));
    }



    #region 说明
    private void BeginCallBack()
    {
        _level = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel;
    }
    
    private void EndCallBack()
    {
        _coroutineID = -1;

        this.SendCommand(new SetPlaneSpriteLevelCommand(_level));
        this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_GAME_UPDATE_LEVEL);
        this.GetSystem<IMessageSystem>().RemoveListener(MsgEvent.EVENT_GAME_EXP_LEVEL_UP,LevelUpByExp);
    }
    
    private void LevelUpByExp(object[] paras)
    {
        _level++;
    }
    #endregion

}
