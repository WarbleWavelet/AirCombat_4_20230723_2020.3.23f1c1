﻿using QFramework.AirCombat;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Example
{
    class Framework
    {

        void All()
        {    
            Architecture();   
            Controller();   
            Command();
            Model();
            System();
            Utility();
        
           
        }


        #region 辅助

        void Command()
        {
            IncreaseCountCommand increaseCountCommand;
            DecreaseCountCommand decreaseCountCommand;
        }
        void Model()
        {
            IModel iModel;
            AbstractModel abstractModel;
            ICounterAppModel iAirCombatAppModel;
            AirCombatAppModel airCombatAppModel;
        }

        void System()
        {
            ISystem iSystem;
            IAchievementSystem iAchievementSystem;
            AchievementSystem achievementSystem;

        }
        void Utility()
        {
            IUtility iIUtility;
            IStorage iStorage;
            Storage storage;

        }

        void Controller()
        {
            CounterAppController counterAppController;
        }

        void Architecture()
        {
            CounterApp counterApp;
        }
        #endregion
    }


    #region Model
    // 1. 定义一个 Model 对象
    public interface ICounterAppModel : IModel
    {
        BindableProperty<int> Count { get; }
    }
    public class AirCombatAppModel : AbstractModel,ICounterAppModel
    {
        public BindableProperty<int> Count { get; } = new BindableProperty<int>();

        protected override void OnInit()
        {
            IStorageUtil storage = this.GetUtility<IStorageUtil>();
            
            // 设置初始值（不触发事件）
            Count.SetValueWithoutEvent(storage.Get<int>(nameof(Count)));

            // 当数据变更时 存储数据
            Count.Register(newCount =>
            {
                storage.Set<int>(nameof(Count),newCount);
            });
        }
    }

    #endregion


    #region System
    public interface IAchievementSystem : ISystem
    {
        
    }

    public class AchievementSystem : AbstractSystem ,IAchievementSystem
    {
        protected override void OnInit()
        {
            this.GetModel<ICounterAppModel>() // -+
                .Count
                .Register(newCount =>
                {
                    if (newCount == 10)
                    {
                        Debug.Log("触发 点击达人 成就");
                    }
                    else if (newCount == 20)
                    {
                        Debug.Log("触发 点击专家 成就");
                    }
                    else if (newCount == -10)
                    {
                        Debug.Log("触发 点击菜鸟 成就");
                    }
                });
        }
    }
    #endregion


    #region Utility

    public interface IStorage : IUtility
    {
        void SaveInt(string key, int value);
        int LoadInt(string key, int defaultValue = 0);
    }
    
    public class Storage : IStorage
    {
        public void SaveInt(string key, int value)
        {
            PlayerPrefs.SetInt(key,value);
        }

        public int LoadInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(key, defaultValue);
        }
    }
    #endregion


    #region Architecture
    // 2.定义一个架构（提供 MVC、分层、模块管理等）
    public class CounterApp : Architecture<CounterApp>
    {
        protected override void Init()
        {
            // 注册 System 
            this.RegisterSystem<IAchievementSystem>(new AchievementSystem()); 
             
            // 注册 Model
            this.RegisterModel<ICounterAppModel>(new AirCombatAppModel());
            
            // 注册存储工具的对象
            this.RegisterUtility<IStorage>(new Storage());
        }


        protected override void ExecuteCommand(ICommand command)
        {
            Debug.Log("Before " + command.GetType().Name + "Execute");
            base.ExecuteCommand(command);
            Debug.Log("After " + command.GetType().Name + "Execute");
        }

        protected override TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            Debug.Log("Before " + command.GetType().Name + "Execute");
            var result =  base.ExecuteCommand(command);
            Debug.Log("After " + command.GetType().Name + "Execute");
            return result;
        }
    }
    #endregion


    #region Command
    // 引入 Command
    public class IncreaseCountCommand : AbstractCommand 
    {
        protected override void OnExecute()
        {
            var model = this.GetModel<ICounterAppModel>();
                
            model.Count.Value++;
        }
    }
    
    public class DecreaseCountCommand : AbstractCommand
    {
        protected override void OnExecute()
        {
            this.GetModel<ICounterAppModel>().Count.Value--; // -+
        }
    }
    #endregion


    #region Controller
    // Controller
    public class CounterAppController : MonoBehaviour , IController /* 3.实现 IController 接口 */
    {
        // View
        private Button mBtnAdd;
        private Button mBtnSub;
        private Text mCountText;
        
        // 4. Model
        private ICounterAppModel mModel;

        void Start()
        {
            // 5. 获取模型
            mModel = this.GetModel<ICounterAppModel>();
            
            // View 组件获取
            mBtnAdd = transform.Find("BtnAdd").GetComponent<Button>();
            mBtnSub = transform.Find("BtnSub").GetComponent<Button>();
            mCountText = transform.Find("CountText").GetComponent<Text>();
            
            
            // 监听输入
            mBtnAdd.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand<IncreaseCountCommand>();
            });
            
            mBtnSub.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand(new DecreaseCountCommand(/* 这里可以传参（如果有） */));
            });

            // 表现逻辑
            mModel.Count.RegisterWithInitValue(newCount => // -+
            {
                UpdateView();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        
        void UpdateView()
        {
            mCountText.text = mModel.Count.ToString();
        }

        // 3.
        public IArchitecture GetArchitecture()
        {
            return CounterApp.Interface;
        }

        private void OnDestroy()
        {
            // 8. 将 Model 设置为空
            mModel = null;
        }
    }
    #endregion

}
