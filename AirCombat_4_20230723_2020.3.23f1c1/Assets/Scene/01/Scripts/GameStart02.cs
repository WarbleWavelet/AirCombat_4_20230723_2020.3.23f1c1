using System;
using System.Collections;
using System.Linq;
using LitJson;
using QFramework;
using QFramework.Example;
using UnityEngine;

namespace QFramework.AirCombat
{ 
    public class GameStart02 : MonoBehaviour  ,IController
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
            Transform mgrTrans = transform.FindTop(GameObjectName.Mgr);
            Transform canvasTrans = transform.FindTop(GameObjectName.Canvas);

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
        if (true)
        {
            ResKit.Init();
            //UIKit.OpenPanel<SelectHeroPanel>();
            
        }
        else
        {
            //this.GetSystem<IUISystem>().InitComponentEnemy(canvasTrans.GetComponent<_common>());
            //this.GetSystem<IUISystem>().Open(Paths.PREFAB_START_VIEW);
        }

        { 
            // 之前因为加了DontDestroyOnLoad。设置无效，尝试强制设定。
            //this.GetSystem<ILifeCycleSystem>().gameObject.SetParent(mgrTrans);
            //CoroutineMgr.Single.gameObject.SetParent(mgrTrans);
            //this.GetSystem<IAudioSystem>().gameObject.SetParent(mgrTrans);        
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
