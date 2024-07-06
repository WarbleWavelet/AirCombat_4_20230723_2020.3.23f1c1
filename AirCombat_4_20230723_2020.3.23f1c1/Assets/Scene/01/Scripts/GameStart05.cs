using System;
using System.Collections;
using System.Linq;
using LitJson;
using QFramework;
using QFramework.Example;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


namespace QFramework.AirCombat
{ 
    public class GameStart05 : MonoBehaviour ,IController
    {
        // Use this for initialization
        /// <summary>首次登录，新手引导</summary>
        public bool IsFirst = false;


        #region 生命
        private void Awake()
        {
        }

        private void Start()
        {


            StartCoroutine(Init());
            return;
            gameObject.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.Q))
                .First(unit => true)
                .Subscribe(_ => Debug.Log(LifeCycleConfig.LifeCycleFuncDic.Count()))//不订阅就不会带引Do
                .AddTo(gameObject);
            return;


            gameObject.UpdateAsObservable()
                .Where(_ => Input.GetKeyDown(KeyCode.W))
                .First(unit => true)
                .Subscribe(_ => this.GetModel<IAirCombatAppStateModel>().SelectedLevel.Value++)//不订阅就不会带引Do
                .AddTo(gameObject);
        }
        #endregion





        private IEnumerator Init()
        {
            if (false)
            { 
                yield return TestMgr.Single.Init();
                yield return null;            
            }
            //this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Main;
            //this.GetModel<IAirCombatAppStateModel>().E_GameState.Value =GameState.NULL;//NULL!=START触发广播

            this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Game;
            this.SendCommand<GameStateStartCommand>();

            //

            this.GetUtility<IStorageUtil>().ClearAll();
            if (IsFirst)
            {
                this.GetSystem<IGuideUiSystem>().Init();//设置Canvas，因为我把启动脚本放外面，不放在Canvas下
                GuideMgr.Single.InitGuide();
            }
            yield return new WaitForSeconds(2f);//延时让json数据初始化



            if (true)
            {
                ResKit.Init();
                {
                    this.SendCommand<OpenGamePanelCommand>();   
                    //SpawnPlaneMgr.Instance.Log();//调用去跑一下SpawnPlaneMgr 
                }
            }
        }




        #region 重写
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion


    }



}
