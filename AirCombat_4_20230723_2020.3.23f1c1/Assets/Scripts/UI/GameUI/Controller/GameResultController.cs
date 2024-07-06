using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BindPrefabAttribute(ResourcesPath.PREFAB_GAME_RESULT_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class GameResultController : ControllerBase, QFramework.IController
{
    protected override void InitChild()
    {
       this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform, GameObjectName.BGButton, BackToMain);
    }



    public override void Show()
    {
        base.Show();
        if (!IsFinishOneLevel())
        {
            StatePAUSE();
        }
        else
        {
            StateNULL();
            this.GetSystem<ICoroutineSystem>().StartDelay(
                Const.DelayBack, this.GetSystem<IUISystem>().Back);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (!IsFinishOneLevel())
        {
            StateNULL();
        }
    }


    #region pri


    private void BackToMain()
    {
        if (!IsFinishOneLevel())
        {
            this.GetSystem<IUISystem>().Back();
            SceneMain();
            this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_LOADING_VIEW);
        }
    }

    bool IsFinishOneLevel()
    {
        return this.GetModel<IAirCombatAppStateModel>().IsFinishOneLevel;
    }

    void StateNULL()
    {
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.NULL;
    }

    void StatePAUSE()
    {
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.PAUSE;
    }

    void SceneMain()
    {
        this.GetModel<IAirCombatAppStateModel>().TarScene.Value = ESceneName.Main;
    }
    #endregion


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
