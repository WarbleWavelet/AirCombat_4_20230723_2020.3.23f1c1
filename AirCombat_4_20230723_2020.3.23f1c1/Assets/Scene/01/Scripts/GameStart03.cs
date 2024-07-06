using System;
using System.Collections;
using System.Linq;
using LitJson;
using QFramework;
using QFramework.Example;
using UnityEngine;

namespace QFramework.AirCombat
{ 
public class GameStart03 : MonoBehaviour ,IController
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
                this.SendCommand<OpenStrengthenPanelCommand>();
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
        yield return new WaitForSeconds(5f);//延时让json数据初始化


        { //弄点数据
           

        }    
        if (true)
        {
            ResKit.Init();
            {
                this.SendCommand( new OpenStrengthenPanelCommand(() => 
                {
                    //this.GetUtility<IStorageUtil>().Set(DataKeys.STAR,666);                
                    //this.GetUtility<IStorageUtil>().Set(DataKeys.DIAMOND,1000);
                    //按他的设置来设置的话
                    //GameStateModel.Single.SetStar(800);//有的还没改，注释掉会报错
                    //GameStateModel.Single.SetDiamond(1000);
                    this.SendCommand(new SetStarCommand(1002)); //此时强化面板为打开
                    this.SendCommand(new SetDiamondCommand(1002));
                }));
            }
        }
    }



        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
    }



}
