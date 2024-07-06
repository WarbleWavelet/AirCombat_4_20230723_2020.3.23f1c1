using QFramework.AirCombat;
using QFramework;
using UnityEngine;

[BindPrefabAttribute(ResourcesPath.PREFAB_LOADING_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class LoadingController : ControllerBase, QFramework.IController
{
    private bool _showEnd;
    protected override void InitChild()
    {
    }

    public override void Show()
    {
        base.Show();
        IAirCombatAppStateModel stateModel = this.GetModel<IAirCombatAppStateModel>();
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.NULL;
        bool load = stateModel.TarScene != stateModel.CurScene && stateModel.TarScene != ESceneName.NULL;


        if (load)
        {
            ESceneName sceneName = stateModel.TarScene;
            this.GetSystem<ISceneSystem>().AsyncLoadScene(sceneName); 
            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
        }

        _showEnd = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(!_showEnd)
            return;
        
        if (this.GetSystem<ISceneSystem>().Process() == 1)
        {
            if (this.GetModel<IAirCombatAppStateModel>().TarScene == ESceneName.Game)
            {
                this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_GAME_UI_VIEW);
            }
            else if (this.GetModel<IAirCombatAppStateModel>().TarScene == ESceneName.Main)
            {
                this.GetSystem<IUISystem>().Back();
            }

            this.GetSystem<IUISystem>().Hide(ResourcesPath.PREFAB_LOADING_VIEW);
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (this.GetModel<IAirCombatAppStateModel>().TarScene != ESceneName.NULL)
        {
            this.GetModel<IAirCombatAppStateModel>().TarScene.Value = ESceneName.NULL;
        }



        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
        _showEnd = false;
    }



    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}