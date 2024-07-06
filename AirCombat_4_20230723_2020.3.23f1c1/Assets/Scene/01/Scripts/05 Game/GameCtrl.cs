/****************************************************
    文件：MainCameraCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace QFramework.AirCombat
{
    public class GameCtrl : MonoBehaviour,QFramework.IController ,IInitStrictly
    {
        public int SelectedLevel;
        public int CurLevel;
        public int PassedLevel;
        #region 生命


        private void Start()
        {

            this.GetModel<IAirCombatAppStateModel>().SelectedLevel.Register(level => SelectedLevel = level); 
            this.GetModel<IAirCombatAppStateModel>().CurLevel.Register(level => CurLevel = level); 
            this.GetModel<IAirCombatAppModel>().PassedLevel.Register(level => PassedLevel = level); 

            this.RegisterEvent<InitGameObjectPoolSystemEvent>(_ =>
            {
                //this.SendCommand(new AddEnemyGameObjectPoolCommand()); //
            });
            //
            InitStrictly();


        }

        public void InitStrictly()
        {
           this.RegisterEvent<InitCameraSpeedEvent>(_ =>
            {
                InitMap();
                InitPlayer();
                InitGameState();
            });
            //
            Camera.main.transform.GetOrAddComponent<MainCameraCtrl>().Init();
        }
        #endregion


        #region pri         
        
        private void InitGameState()
        {
            this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Game;
            this.SendCommand<GameStateStartCommand>();
        }





        void InitMap()
        { 
               GameObject go= this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_MAP_CTRL,LoadType.RESOURCES);
                go.name=GameObjectName.MapCtrl;
                go.GetOrAddComponent<MapCtrl>().Init();
        
        }
        void InitPlayer()
        {
                                                    
            GameObject go = this.GetSystem<IGameObjectSystem>().Instantiate(ResourcesPath.PREFAB_PLANE, LoadType.RESOURCES);
            go.name=GameObjectName.Player;
         
            go.SetParent(   this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.GameRoot_PLANE) );
            go.GetOrAddComponent<PlanePlayerCtrl>().InitStrictly(BulletType.PLAYER);
            this.SendCommand<InitedPlayerCommand>();
        }
        #endregion

        #region 实现
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }

                                         
        #endregion

    }
}



