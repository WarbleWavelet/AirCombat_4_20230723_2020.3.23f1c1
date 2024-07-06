/****************************************************
    文件：Framework_AirCombat.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/31 20:29:31
	功能： 飞机大战的框架整理汇总
*****************************************************/

using LitJson;
using UnityEngine;
//using static SBindPrefab;
using Random = UnityEngine.Random;


namespace QFramework.AirCombat
{
    #region Framework_AirCombat
    public class AirCombat_Framework : MonoBehaviour   ,QFramework.ICanGetUtility
    {
        #region 属性

        #endregion

        #region 生命


        /// <summary>首次载入且Go激活</summary>
        void Start()
        {

            //UI
            {
                Panel_QF();
                View();
                LifeTime();
                Controller();
                LifeTime_Controller();
                SpecificUI_MainUI();
                SpecificUI_Effect();
                SpecificUI_GameUI();

            }
            Manager();
            Reader();
            Attribute();
            Config();
            Util();
            Loader();
            Data();
            TaskQueue();
            Message();
            Action();
            Pool();
            Coroutine();
            Model();
            Singleton();
            Test();
            //游戏中的
            {
                Input();
                Behaviour();
                Ani();
                Bullet();
                BulletMgr();
                Component();
                Effect();
                EffectView();
                EnemyCreater();
                GameProcess();
                Factory();
                Map();
                InitGame();
                PlayerController();
                Trajectory();
                SpecialState();
                View_Logic();
                Guide();
            }

            {
                GetDataByPlayerPrefersOrJson();

            }
        }


        /// <summary>获取json数据</summary>
        private void GetDataByPlayerPrefersOrJson()
        {
            //LifeCycleMgr lifeCycleMgr;
            //LifeCycleAddConfig lifeCycleAddConfig;
            // ConfigMgr configMgr;  //ConfigSystem替代
            // DataMgr dataMgr;  //StorageUtil替代
            //以下演示操作方式
            this.GetUtility<IStorageUtil>().SetObject("key", new object()); //PlayerPrefers
            this.GetUtility<IStorageUtil>().SetJsonData("key", new JsonData());//DataUtil
        }





        #region 生命 其它
        /// <summary>首次载入</summary>
        void Awake()
        {

        }


        /// <summary>Go激活</summary>
        void OnEnable()
        {

        }

        /// <summary>固定更新</summary>
        void FixedUpdate()
        {

        }

        void Update()
        {

        }

        /// <summary>延迟更新。适用于跟随逻辑</summary>
        void LateUpdae()
        {

        }

        /// <summary> 组件重设为默认值时（只用于编辑状态）</summary>
        void Reset()
        {

        }


        /// <summary>当对象设置为不可用时</summary>
        void OnDisable()
        {

        }


        /// <summary>组件销毁时调用</summary>
        void OnDestroy()
        {

        }
        #endregion
        #endregion

        #region 系统

        #endregion

        #region 辅助
        private void SpecialState()
        {
            LevelUpState levelUpState;
            //
            IBuff iBuff;
            IBuffCarrier iBuffCarrier;
            IDebuff iDeff;
            IDebufferCarrier iDebufferCarrier;
            ISpecialState iSpecialState;
            SpecialStateMgrBase specialStateMgrBase;
            //
            PlayerBuffMgr playerBuffMgr;

        }


        /// <summary>轨迹</summary>
        private void Trajectory()
        {
            IPathCalc iTrajectory;
            EllipsePathCalc ellipseTrajectory;
            RotatePathCalc rotateTrajectory;
            StraightPathCalc straightTrajectory;
            VPathCalc vTrajectory;
            TrajectoryData();
            Path();
        }

        private void PlayerController()
        {
            PlayerController playerController;

        }

        private void InitGame()
        {
           // GameLayerMgr gameLayerMgr;
           // GameRoot gameRoot;
        }

        private void Map()
        {
           // MapCloud mapCloud;
           // MapItem mapItem;
        }

        private void Factory()
        {
           // ItemFactory itemFactory;
           // PathFactory pathFactory;
            BuffFactory specialStateFactory;

        }

        private void EnemyCreater()
        {
            IEnemyCreator iEnemyCreater;
            MissileEnemyCreator missileEnemyCreater;
            //EnemyPlaneCreator planeEnemyCreater;
            //
            IEnemyCreator iSubEnemyCreaterMgr;
            BossCreatorMgr bossCreaterMgr;
            ElitesCreatorMgr elitesCreaterMgr;
            DomEnemyCreatorMgr enemyCreaterMgr;
            MissileCreatorMgr missileCreaterMgr;
            NormalCreatorMgr normalCreaterMgr;
            //
           // LoadCreatorData loadCreaterData;
            GameProcess();
        }

        private void GameProcess()
        {
            //GameProcessMgr gameProcessMgr;
            GameProcessNormalEvent gameProcessNormalEvent;
            GameProcessTriggerEvent gameProcessTriggerEvent;
            IGetGameProcessNormalEvents iGameProcessNormalEvent;
            IGetGameProcessTriggerEvents iGameProcessTriggerEvent;

        }
        private void Component()
        {
            DelayDestroy autoDestroyComponent;
            //
            BulletBossEventComponent bossBulletEventComponent;
            CollideMsgFromBulletComponent bulletCollideMsgComponent;
            CameraMoveSelfComponent cameraMove;
            Trigger2DComponent colliderComponent;
            EnemyTypeComponent enemyTypeComponent;
            ICollideMsg iColliderMsg;
            IComponent iComponent;
            CollideMsgFromItemComponent itemCollideMsgComponent;
            LifeComponent lifeComponent;
            MoveSelfComponent moveComponent;
            CollideMsgFromPlaneComponent planeCollideMsgComponent;
            ColliderAdaptRenderComponent renderComponent;

        }

        private void BulletMgr()
        {
            BulletEffectComponent bulletEffectMgr;
            //SpawnBulletMgr emitBulletMgr;
            ShootCtrlsComponent enemyBulletMgr;
            BulletPointsCalcEllipseComponent spawnBulletPointMgr;
        }

        private void Bullet()
        {
            IBullet iBullet;
            IBulletModel iBulletModel;
            IEnemyBulletModel iEnemyBulletModel;
            IEnemyBossBulletModel iEnemyBossBulletModel;
            //                      
            BulletOwner bulletOwner;
            BulletType bulletType;
            BulletName bulletName;
            BulletEventType bulletEventType;
            //
            BulletEnemyCtrl bullet;
            BulletMgr();
        }

        private void Behaviour()
        {
            IDespawnCase iBehaviour;
            PlayerDestroyComponent playerBehaviour;
            EnemyDestroyComponent enemyBehaviour;
            DespawnBulletComponent bulletBehaviour;

        }

        private void Path()
        {
            IPathBase iPath;
            PathBase pathBase;
            EllipsePath ellipsePath;
            StayOnTopPath stayOnTopPath;
            StraightPath straightPath;
            WPath wPath;
            //PathMgr pathMgr;
            //
            IEnterPath iEnetrPath;
            EnterPathMgr enterPath;
            EnterPathMgr.MoveDir moveDirection;
            Right2LeftEnterPath rightToLeft;
            Left2RightEnterPath leftToRight;
            Up2DownEnterPath upToDown;
        }

        private void Test()
        {
            BulletConfigTest bulletConfigTest;
            ITest iTest;
            MsgEventTest msgEventTest;
            ShowFPS showFPS;
        }

        private void Manager()
        {
           // AudioMgr audioMgr;
            //ConfigMgr configMgr; 
           // CoroutineMgr coroutineMgr;
            //DataMgr dataMgr;
           //DelayDetalCoroutineMgr delayDetalCoroutineMgr;
            //InputMgr inputMgr;
            //LifeCycleMgr lifeCycleMgr;
            //LoadMgr loadMgr;
            //MessageMgr messageMgr;
            //PoolMgr poolMgr;
            //ReaderUtil readerMgr;
            //SceneMgr sceneMgr;
            //SubMsgMgr subMsgMgr;
            TaskQueueMgr taskQueueMgr;
            TestMgr testMgr;
            //UILayerMgr uiLayerMgr;
            ////UIManager uiManager;
            //
            //PathMgr pathMgr;
            BulletMgr();
            //
            GameDataMgr gameDataMgr;
            //

            GuideMgr guideMgr;

        }

        private void SpecificUI_GameUI()
        {
            GameResultController gameResultController;
            //GameUIController gameUIController;
            PowerController powerController;
            SettingController settingController;
            ShieldController shieldController;
            //
            GameResultView gameResultView;
            //GameUIView gameUIView;
            LifeBarComponent life;
            LifeItemComponent lifeItem;
            Power power;
            SettingView settingView;
            Shield shield;
            Warning warning;


        }
        private void Ani()
        {
            //AniMgr aniMgr;
            AniSystem aniSystem;
            FrameAniComponent frameAni;
            PlayerEnterAniComponent playerEnterAni;
        }

        private void SpecificUI_Effect()
        {
            ItemCDEffect itemCDEffect;
            ItemEffect itemEffect;
        }


        /// <summary>具体的面板(View和Controller分开的话不好复制)</summary>
        private void SpecificUI_MainUI()
        {   
            HeroItem heroItem;
            HeroItemCtrl heroItemController;
            SelectHero selectHero;
            SelectHeroController selectHeroController;
            SelectedHeroView selectedHeroView;
            SelectedHeroController selectedHeroController;
            SwitchPlayerView switchPlayer;
            SwitchPlayerController switchPlayerController;
            //

            LevelRoot levelRoot;
            LevelRootController levelRootController;
            LevelsView levelsView;
            LevelsController levelsController;
            LevelItem levelItem;
            global::LevelItemController levelItemController;

            //
            LoadingView loadingView;
            LoadingController loadingController;
            //
            PlaneProperty planeProperty;
            PlanePropertyController planePropertyController;
            PropertyItem propertyItem;
            PropertyItemController propertyItemController;
            StrengthenView strengthenView;
            StrengthenController strengthenController;
             //
            StartController startController;
            //
            //
           // DialogPanel dialogView;
            Money money;
        }

        void SelectHeroPanel()
        { 
        
        }

        private void Singleton()
        {
            // MonoSingleton<AMono> monoSingleton;
            NormalSingleton<A> normalSingleton;

        }

        private void LifeTime_Controller()
        {
            IControllerInit iControllerInit;
            IControllerShow iControllerShow;
            IControllerHide iControllerHide;
            IControllerUpdate iControllerUpdate;
            LifeTime();
        }

        private void Controller()
        {

            LifeTime_Controller();
            IController iController;
            ControllerBase controllerBase;
        }

        private void Model()
        {
            //GameModel gameModel;
            NSPlaneSpritesModel planeSpritesModel;
            //
            IBullet iBullet;
            IBulletModel iBulletModel;
            IEnemyBulletModel iEnemyBulletModel;
            IEnemyBossBulletModel iEnemyBossBulletModel;
            EnemyBossBulluetModelBase enemyBossBulluetModelBase;
            EnemyBoss0BulluetModel enemyBoss0BulluetModel;
            EnemyBoss1BulluetModel enemyBoss1BulluetModel;

            EnemyNormalBulluetModel enemyBulluetModel;
            PlayerBulletModel playerBulletModel;
            PowerBulletModel powerBulletModel;


        }

        private void Coroutine()
        {
            CoroutineLife.CoroutineState coroutineState;
            CoroutineLife coroutineItem;
            DelayDetalCoroutineItem delayDetalCoroutineItem;
            CoroutineHandler coroutineController;
        }

        private void Pool()
        {

            GameObjectPool gameObjectPool;
            ObjectPoolNew<A> objectPool;
        }

        private void Action()
        {
            //ActionMgr actionMgr;
        }

        private void Message()
        {
            IMessageSystem iMessageSystem;
            //MessageSystem messageSystem;
            //
            //ActionMgr actionMgr;
        }

        private void Input()
        {
            //IInputModule iInputModule;
            //InputModule inputModule;
            //KeyState inputState;
            //InputMgr inputMgr;

        }

        private void Panel_QF()
        {              
            StartGamePanel startGamePanel;
            StrengthenPanel strengthenPanel;
            SelectHeroPanel selectHeroPanel;
        }
        private void View()
        {
            LifeTime();
            IEntity2D iView;
            ViewBase viewBase;
            //
            LevelRoot levelRoot;
            View_Logic();
        }

        void View_Logic()
        {
            // Enemy
            EnemyLifeItem enemyLifeItem;
            EnemyLifeComponent enemyLifeView;
            MissileView missileView;
            MissileWarningView missileWarning;
            // ItemView
            AddBulletItemView addBulletView;
            AddExpItemView addExpView;
            PlaneLevelView itemViewBase;
            PowerItemView powerItemView;
            ShieldItemView shieldItemView;
            StarItemView starView;
            //
            BulletDestroyAniView bulletDestroyAniView;
            IEntity2D iGameView;
            PlaneDestroyAniView planeDestroyAniView;
           // PlayerView playerView;
            BulletPowerComponent powerView;
            ShieldComponent shieldView;

        }

        private void TaskQueue()
        {
            TaskQueue taskQueue;
            TaskQueueMgr taskQueueMgr;
            LevelRoot levelRoot;
        }

        private void Data()
        {
           // IStorage iDataMemory;
            PlayerPrefsStorage playerPrefsMemory;
            //JsonMemory jsonMemory;
            //DataMgr dataMgr;
            //
            //Logic/Data
            {
                AllEnemyData allEnemyData;
                EnemyData enemyData;
                //
                AllBulletData allBulletData;
                BulletData bulletData;
                NormalBulletData normalBulletData;
                BossBulletData bossBulletData;
                Boss0BulletData boss0BulletData;
                Boss1BulletData boss1BulletData;
                BulletEvent bulletEvent;
                BulletEventData bulletEventData;
                ChangeSpeedData changeSpeedData;
                ChangeTrajectoryData changeTrajectoryData;
                //
                EnemyLevelData enemyCreaterConfigData;
                LevelData levelData;
                ICreatorData iCreaterData;
                PlaneCreatorData planeCreaterData;
                MissileCreatorData missileCreaterData;
                //
                GameDataMgr gameDataMgr;
            }

            //




        }

        void TrajectoryData()
        {
            PathDataMgr enemyTrajectoryDataMgr;
            IPathData iTrajectoryData;
            StraightPathData straightTrajectoryData;
            VPathData vTrajectoryData;
            EllipsePathData ellipseData;
            RotatePathData rotateData;
        }

        private void EffectView()
        {

            EffectLevelView effectViewBase;
            MissileWarningEffectContainer missileWarningEffectView;
            StarEffectContainer starEffectView;

        }

        private void Effect()
        {
            IEffect iEffect;
            IEffectContainer iEffectView;
            EffectLevelView effectViewBase;

            FlyIntoUIEffect flyIntoUI;
            RotateEffect rotateEffect;
            ScaleEffect scaleEffect;
            ShowAndHideEffect showAndHideEffect;
            SlowSpeedEffect slowSpeedEffect;
            //
        }

        private void Loader()
        {
            ILoader loader;
            ABLoader abLoader;
            ResourceLoader resourceLoader;
        }

        private void Util()
        {
            //SBindPrefab bindUtil;
            //BulletModelUtil bulletUtil;
            this.GetUtility<IStorageUtil>().SetJsonData("key", new JsonData());//DataUtil
            Empty4Raycast empty4Raycast;
            this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,"xxx", () => { });
            GameUtil gameUtil;
            JsonUtil jsonUtil;
            KeysUtil keysUtil;
            UiUtil uiUtil;
            //
            //
            Guide_Util();

        }
        void Guide_Util()
        {
            GuideGenerateIDTool guideGenerateIDTool;
        }

        void Guide()
        {
            Guide_Util();
            //
            Guide_Frame();
            Guide_Business();
            GuideMgr guideMgr;
        }

        void Guide_Frame()
        {
            IGuide iGuide;
            IGuideRoot iGuideRoot;
            GuideBase guideBase;
            //
            IGuideBehaviour iGuideBehaviour;
            GuideBehaviourBase guideBehaviourBase;
            //
            IGuideGroup iGuideGroup;
            GuideGroupBase<IGuideGroup> guideGroupBase;
            //
            GuideMgr guideMgr;
        }
        void Guide_Business()
        {
            //Module
            ExplainDialogModule explainDialogModule;
            HandModule handModule;
            HighLightModule highLightModule;
            //Guide
            SelectedHeroViewGuide selectedHeroViewGuide;
            StartViewGuide startViewGuide;
            //Group
            ShowHerosGroup showHerosGroup;
            StartViewGroup startViewGroup;
            //Demo
            TestDemo.StartViewGuide testDemo_StartViewGuide;
            TestDemo.DemoGuideGroupA testDemo_DemoGuideGroupA;
            TestDemo.Demo1GuideGroup testDemo_Demo1GuideGroup;
            TestDemo.Demo2GuideGroup testDemo_Demo2GuideGroup;
            TestDemo.ClickButtonA testDemo_ClickButtonA;
            //
            TestDemo.GuideMgr testDemo_GuideMgr;
            //Behaviour
            ShowHerosBehaviour showHerosBehaviour;
            StartGameBehaviour startGameBehaviour;
            WelcomeGuideBehaviour welcomeGuideBehaviour;




        }

        private void Config()
        {
           // BulletEffectPoolConfig bulletEffectPoolConfig;  //SafeObjectPool代替
           // LifeCycleAddConfig lifeCycleAddConfig;
            LifeCycleConfig lifeCycleConfig;
            GameObjectPoolConfig poolConfig;
            ReaderConfig readerConfig;
            SceneConfig sceneConfig;
            UILevelConfigSingleton uiLayerConfig;
        }
        void Reader()
        {
            IReader reader;
            JsonReader jsonReader;
            //ReaderUtil readerMgr;
            ReaderConfig readerConfig;
            // 数据单位
            IKey iKey;
            Key key;
            KeyQueue keyQueue;
        }


        void Attribute()
        {
            StCustomAttributes initCustomAttributes;
            BindPrefabAttribute bindPrefabAttribute;
            BulletAttribute bulletAttribute;

            //
            //SBindPrefab bindUtil;
            //BindPriorityComparer bindPriorityComparer;


        }
        void LifeTime()
        {
            IInit init;
            IInitParent iInitParent;
            IShow show;
            IHide hide;
            IUpdate update;
            IDestroy destroy;
        }

        public IArchitecture GetArchitecture()
        {
            throw new System.NotImplementedException();
        }

        #endregion


        #region 内部类（举例，没有实际用处）
        class A
        { }
        class AMono : MonoBehaviour
        { }
        #endregion
    }

    #endregion
}





