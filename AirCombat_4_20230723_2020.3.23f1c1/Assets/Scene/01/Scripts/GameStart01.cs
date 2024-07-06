using System;
using System.Collections;
using System.Linq;
using LitJson;
using QFramework;
using QFramework.Example;
using UnityEngine;
namespace QFramework.AirCombat
{ 
    public class GameStart01 : MonoBehaviour ,IController
    {
        // Use this for initialization
        /// <summary>首次登录，新手引导</summary>
        public bool IsFirst = false;
        private void Start()
        {
            if (FindObjectsOfType<MainCtrl>().Length > 1)
            {
                Destroy(gameObject);
                return;
            }
        
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            //yield return TestMgr.Single.InitComponentEnemy();
            yield return null;
            //
            Transform mgrTrans = GameObject.Find(GameObjectName.Mgr).transform;
            Transform canvasTrans = GameObject.Find(GameObjectName.Canvas).transform;
            //
            this.GetUtility<IStorageUtil>().ClearAll();
            this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Main;
            //
            this.GetSystem<ILifeCycleSystem>().Init();
            //
            if (IsFirst)
            { 
                this.GetSystem<IGuideUiSystem>().Init();//设置Canvas，因为我把启动脚本放外面，不放在Canvas下
                GuideMgr.Single.InitGuide();        
            }

            //
            ResKit.Init();
            this.SendCommand<OpenStartGamePanelCommand>();
        }

        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
    }

}
