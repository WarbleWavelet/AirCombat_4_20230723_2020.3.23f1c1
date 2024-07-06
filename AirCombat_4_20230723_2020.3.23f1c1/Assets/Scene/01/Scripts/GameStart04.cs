using System;
using System.Collections;
using System.Linq;
using LitJson;
using QFramework;
using QFramework.Example;
using UnityEngine;

namespace QFramework.AirCombat
{ 
public class GameStart04 : MonoBehaviour ,IController
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

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) //确定是json数据初始慢导致强化面板获取数据失败
            {
                OpenPanelFunc();
            }

        }



        private IEnumerator Init()
    {
        yield return TestMgr.Single.Init();
        yield return null;
        //
        Transform mgrTrans = transform.FindTop(GameObjectName.Mgr);
        Transform canvasTrans = transform.FindTop(GameObjectName.Canvas);
            //
        this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Main;

        this.GetSystem<ILifeCycleSystem>().Init();
        this.GetUtility<IStorageUtil>().ClearAll();
        if (IsFirst)
        { 
            this.GetSystem<IGuideUiSystem>().Init();//设置Canvas，因为我把启动脚本放外面，不放在Canvas下
            GuideMgr.Single.InitGuide();        
        }
        yield return new WaitForSeconds(3f);//延时让json数据初始化



        if (true)
        {
            ResKit.Init();
            {
                this.SendCommand(new SetSelectedPlaneIDCommand(0));
                this.GetModel<IAirCombatAppStateModel>().SelectHeroID.Value = 0;
                this.GetModel<IAirCombatAppModel>().PassedLevel.Value = 10;
                this.SendCommand(new OpenSelectLevelPanelCommand(() =>
                {
                }));
            }
        }
    }

        void OpenPanelFunc(  )
        {

            this.SendCommand(new OpenSelectLevelPanelCommand(() =>
            {

            }));
        }



        #region 重写
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion  
    }



}
