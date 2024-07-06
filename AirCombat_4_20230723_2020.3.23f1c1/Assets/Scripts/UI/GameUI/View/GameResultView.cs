using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BindPrefabAttribute(ResourcesPath.PREFAB_GAME_RESULT_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class GameResultView : ViewBase, QFramework.IController
{

    private string _lose = Const_CHI.You_Lose;
    private string _win = Const_CHI.You_Win;
    protected override void InitChild()
    {
        
    }

    public override void Show()
    {
        base.Show();
        string result  = this.GetModel<IAirCombatAppStateModel>().IsFinishOneLevel ? _win : _lose;
        UiUtil.Get(GameObjectName.Result).SetText(result);
    }

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
