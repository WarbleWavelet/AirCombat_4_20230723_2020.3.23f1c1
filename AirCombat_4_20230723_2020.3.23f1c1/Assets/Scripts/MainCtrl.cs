using System;
using System.Collections;
using System.Linq;
using Common.CharacterRelationship_Sinmple;
using LitJson;
using QFramework;
using QFramework.AirCombat;
using QFramework.Example;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>一个假设在Main场景为起点的一个节点,切换正常走流程,还是直接战斗</summary>

public class MainCtrl : MonoBehaviour,QFramework.IController  ,ICanRegisterEvent
{
    public bool SpawnPlayer;
    // Use this for initialization
    /// <summary>首次登录，新手引导</summary>
    public bool IsFirst = false;




    private void Start()
    {
        if (gameObject.DestroyIfMoreThanOne<MainCtrl>())
        {
            return;
        }
        //


        //this.RegisterEvent<AirCombatAppLoadedEvent>(_=>
        //{ 
            SceneMain();
            //SceneGame();
        // });

        
        GameObject.DontDestroyOnLoad(gameObject);

    }

    private void Update()
    {


    }


    #region pri

    /// <summary>直接跳到Game</summary>
    private void SceneGame()
    {
        //UnityEngine.AsyncOperation loadScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);//Game
        //UnityEngine.AsyncOperation loadScene = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);//Game
        UnityEngine.AsyncOperation loadScene = SceneManager.LoadSceneAsync(StScenePath.Scene_Game, LoadSceneMode.Single);//Game
        this.SendCommand<OnLoadGameSceneCommand>();
    }

    private void  SceneMain()
    {
        //yield return TestMgr.Single.InitComponentEnemy();
        //yield return null;
        //
        ResKit.Init();
        {
            this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Main;
        }

        //
        if (IsFirst)
        { 
            GuideMgr.Single.InitGuide();        
        }
        this.SendCommand<OpenStartGamePanelCommand>();
    }
    #endregion  




    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
