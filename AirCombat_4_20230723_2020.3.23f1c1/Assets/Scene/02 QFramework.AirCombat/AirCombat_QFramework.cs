/****************************************************
	文件：QFramework_AirCombat.cs
	作者：lenovo
	邮箱: 
	日期：2023/8/8 20:53:33
	功能：
*****************************************************/

using JetBrains.Annotations;
using QFramework.Example;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static LevelRoot;
using static PropertyItem;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Debug = UnityEngine.Debug;
using QFramework;
using UnityEngine.SceneManagement;
using Vector3 = UnityEngine.Vector3;
using System.ComponentModel;
using LitJson;
using QFramework.AirCombat;
using AirCombatAppModel = QFramework.AirCombat.AirCombatAppModel;
using System.Reflection;
using System.Linq;
using static ResourcesName;
using System.Net.Http.Headers;
using System.Collections;
using Color = UnityEngine.Color;
using System.Numerics;
using System.Security.Cryptography;

namespace QFramework.AirCombat
{
	public class AirCombat_QFramework : MonoBehaviour, IController
	{
		void All()
		{
			Architecture();
			//
			Model();
			System();
			Utility();
			Controller();
			//
			Command();
			Event();
			Manager();
			Ctrl();
			//
			Test();
			ReaderConfig();
			//
			{    //PoolMgr
				{
					BulletPool();
					PlanePool_Null();
					ItemPool_Null();
					FrameAniPool_Null();
					MissilePool_Null();
					LightPool_Null();
				}
				//Game游戏中
				{
					// GameRoot gameRoot;
					GameRoot();
					// GameLayerMgr gameLayerMgr;
					{
						MainCamera();
						Map();
						Effect();
						PlaneEnemy();
					}
					Bullet();
				}
				{//DontDestroyOnLoad 
					Mgr();
					Canvas();
				}
			}
			{//流程 
				SpawnEnemy();
			}
		}

		private void SpawnEnemy()
		{
			string path = ResourcesPath.CONFIG_LEVEL_ENEMY_DATA;
			//LoadCreatorData loadCreaterData;
			{
				// DomEnemyCreatorMgr enemyCreaterMgr = null;
				//enemyCreaterMgr.GetNormalEventLst();
				//EnemyPlaneCreator planeEnemyCreator = null;
				//planeEnemyCreator.SpawnedAQueue();
				//
				//GameProcessMgr gameProcessMgr = null;//sum
				//gameProcessMgr.InitComponentEnemy();//InitComponentEnemy()=>InitEnemyCreatorLst()=>InitEventLst();UpdateNormalLst()

				//
				IGetGameProcessNormalEvents iGameProcessNormalEvent;
				{
					DomEnemyCreatorMgr mgr1;
					NormalCreatorMgr mgr2; //IfChangeCreatorAndSpawnNextQueue
				}
			}
			AllEnemyData allEnemyData;//Init中自定义了数据来源
		}



		#region 辅助 

		void Ctrl()
		{
			MainCameraCtrl mainCameraCtrl;
			MapItemCtrl mapItemCtrl;
			BulletPlayerCtrl playerBulletCtrl;
			PlanePlayerCtrl playerCtrl;
			PlaneEnemyCtrl enemyPlaneCtrl1;
		}
		void Manager()
		{
			//SpawnBulletMgr spawnBulletMgr;
			//SpawnPlaneMgr spawnPlaneMgr;
			ShootCtrlsComponent enemyBulletMgr;
			NormalCreatorMgr normalCreaterMgr;
			ElitesCreatorMgr elitesCreaterMgr;
			MissileCreatorMgr missileCreaterMgr;
			BossCreatorMgr bossCreaterMgr;
		}
		private void Bullet()
		{
			{//PoolMgr 
				{//BulletPool 
					{//BulletEnemyCtrl 
						BulletEffectComponent bulletEffectMgr;
						DespawnBulletComponent bulletBehaviour;
						MoveSelfComponent moveComponent;
						CollideMsgFromBulletComponent bulletCollideMsgComponent;
					}
				}
			}
		}


		private void GameRoot()
		{
			Plane();
			Enemy();
		}
		void Enemy()
		{
			ColliderAdaptRenderComponent renderComponent;
			PlayerEnterAniComponent playerEnterAni;
			CameraMoveSelfComponent cameraMove; //没激活
			EnemyTypeComponent enemyTypeComponent;
			LifeComponent lifeComponent;
			//SubMsgMgr subMsgMgr;
			EnemyDestroyComponent enemyBehaviour;
			MoveSelfComponent moveComponent;
			Trigger2DComponent colliderComponent;
			CollideMsgFromPlaneComponent planeCollideMsgComponent;
			{//BulletRoot 
				ShootCtrlsComponent enemyBulletMgr;
				//SpawnBulletMgr emitBulletMgr;
				BulletBossEventComponent bossBulletEventComponent;
				BulletPointsCalcEllipseComponent spawnBulletPointMgr;
			}
			{//EnemyLife 
				EnemyLifeComponent enemyLifeView;
				{//Item 
					EnemyLifeItem enemyLifeItem;
				}
			}
		}


		void Plane()
		{
			//PlayerView playerView;
			PlayerEnterAniComponent playerEnterAni;
			PlayerController playerController;
			PlayerDestroyComponent playerBehaviour;
			PlayerBuffMgr playerBuffMgr;
			CollideMsgFromPlaneComponent planeCollideMsgComponent;
			CameraMoveSelfComponent cameraMove;
			MoveSelfComponent moveComponent;
			ColliderAdaptRenderComponent renderComponent;
			Trigger2DComponent colliderComponent;
			{//BulletRoot 
				//SpawnBulletMgr emitBulletMgr;
				BulletBossEventComponent bossBulletEventComponent;
				BulletPointsCalcEllipseComponent spawnBulletPointMgr;
			}
			Bullet();
		}

		private void Canvas()
		{
			//
			//
			{
				//GameUIView gameUIView;    //GamePanel代替
				//GameUIController gameUIController;
				UiUtil uiUtil;
				//
				LifeBarComponent life;
				//
				Shield shield;
				ShieldController shieldController;
				ItemEffect itemEffect;
				ItemCDEffect itemCDEffect;
				//
				Power power;
				PowerController powerController;
			}
			//GameResultView
			{
				GameResultController gameResultController;
				GameResultView gameResultView;
				UiUtil uiUtil;
				Empty4Raycast empty4Raycast;
			}
		}
		private void Mgr()
		{
			//CoroutineMgr coroutineMgr;
			//LifeCycleMgr lifeCycleMgr;
			//AudioMgr audioMgr;
		}

		private void LightPool_Null()
		{
			throw new NotImplementedException();
		}

		private void MissilePool_Null()
		{
			throw new NotImplementedException();
		}

		private void FrameAniPool_Null()
		{
			throw new NotImplementedException();
		}

		private void ItemPool_Null()
		{
			throw new NotImplementedException();
		}

		private void PlanePool_Null()
		{
			throw new NotImplementedException();
		}

		private void BulletPool()
		{
			BulletEffectComponent bulletEffectMgr;
			MoveSelfComponent moveComponent;
			CollideMsgFromBulletComponent bulletCollideMsgComponent;
			DespawnBulletComponent bulletBehaviour;
		}

		private void MainCamera()
		{
			CameraMoveSelfComponent cameraMove;
			//GameProcessMgr gameProcessMgr;
			MoveSelfComponent moveComponent;
		}

		private void PlaneEnemy()
		{
			{//Plane节点 
				ColliderAdaptRenderComponent renderComponent;
				CameraMoveSelfComponent cameraMove;
				EnemyTypeComponent enemyTypeComponent;
				LifeComponent lifeComponent;
				//SubMsgMgr subMsgMgr;
				EnemyDestroyComponent enemyBehaviour;
				MoveSelfComponent moveComponent;
				Trigger2DComponent colliderComponent;
				CollideMsgFromPlaneComponent planeCollideMsgComponent;
			}
			{ //Plane的子节点BulletRoot
				ShootCtrlsComponent enemyBulletMgr;
				//SpawnBulletMgr emitBulletMgr;
				BulletBossEventComponent bossBulletEventComponent;
				BulletPointsCalcEllipseComponent spawnBulletPointMgr;
			}
			{////Plane的子节点EnemyLife 
				EnemyLifeComponent enemyLifeView;
			}
			{////EnemyLife的子节点Item_0,1,2,3
				EnemyLifeItem enemyLifeItem;
			}

		}

		private void Effect()
		{
			BulletDestroyAniView bulletDestroyAniView;
			PlaneDestroyAniView planeDestroyAniView;
			FrameAniComponent frameAni;
		}

		private void Map()
		{
			// MapItem mapItem;
			// MapCloud mapCloud;
		}

		void ReaderConfig()
		{
			ReaderConfig readerConfig;
			//ReaderUtil readerMgr;
			string str = ResourcesPath.CONFIG_LEVEL_CONFIG;
			//
			ILoader iLoader;
			ResourceLoader resourceLoader;
			// LoadMgr loadMgr;
		}


		void Test()
		{
			if (Input.GetKeyDown(KeyCode.Q)) this.SendCommand(new ChangeStarCommand(100));
			if (Input.GetKeyDown(KeyCode.W)) this.SendCommand(new ChangeStarCommand(-100));
			if (Input.GetKeyDown(KeyCode.E)) this.SendCommand(new ChangeDiamondCommand(-100));
			if (Input.GetKeyDown(KeyCode.R)) this.SendCommand(new ChangeDiamondCommand(-100));
		}

		void Event()
		{
		 //   ChangeDiamondEvent changeDiamondEvent;
			//
//SelectHeroEvent switchHeroEvent;
		//    SelectPlaneIDEvent switchPlaneEvent;
			//
		  //  IncreasePlanePropertyEvent increasePlanePropertyEvent;
		   // UpgradesPlaneLevelEvent upgradesPlaneLevelEvent;
			//
			CloseSelectLevelPanelEvent openLoadingPanelEvent;
			UpdateLevelSelectableLockStateEvent instantiateLevelSelectableEvent;
			//
			//CameraMoveUpEvent cameraMoveEvent;
			//
			PlaneDestroyAniEvent PlaneDestroyAniEvent;
			BulletDestroyAniEvent BulletDestroyAniEvent;


		}


		void Command()
		{
			ChangeStarCommand increaseStarCommand;
			ChangeDiamondCommand increaseDiamondCommand;
			SetStarCommand setStarCommand;
			SetDiamondCommand setDiamondCommand;
			//
			IncreasePlanePropertyCommand increasePlanePropertyCommand;
			IncreasePlaneAttackCommand increasePlaneAttackCommand;
			IncreasePlaneFireRateCommand increasePlaneFireRateCommand;
			IncreasePlaneLifeCommand increasePlaneLifeCommand;
			//UpgradesPlaneLevelCommand upgradesPlaneLevelCommand;
			//
			SetSelectedPlaneIDCommand initPlaneCommand;
			SwitchPlaneCommand switchPlaneCommand;
			SelectHeroCommand switchHeroCommand;
			SetSelectHeroParaCommand setSelectHeroParaCommand;
			//
			OpenStartGamePanelCommand openStartGamePanelCommand;
			OpenSelectHeroPanelCommand openSelectHeroCommand;
			OpenSelectLevelPanelCommand openSelectLevelPanelCommand;
			OpenStrengthenPanelCommand openStrengthenPanel;
			OpenLoadingPanelCommand openLoadingPanel;
			OpenGamePanelCommand openGamePanel;
			ExitGameCommand exitGameCommand;
			//
			InstantiateLeveItemClickablesCommand instantiateLevelSelectableCommand;
			//
			MoveCommand moveCommand;
			MoveUpCommand moveUpCommand;
			//                                         //
			CollideMsgFromBulletCommand bulletColliderCommand;
			SpawnABulletCommand fireCommand;
			InCameraBorderCommand inCameraBorderCommand;
			PlayerCanControlCommand StartGameCommand;
			InitCameraSpeedCommand InitCameraSpeedCommand;
		}


		void Model()
		{
			IModel iModel;
			AbstractModel abstractModel;

			IPlaneModel iPlaneModel;
			PlaneModel planeModel;
			//
			IAirCombatAppModel iAirCombatAppModel;
			AirCombatAppModel airCombatAppModel;
			IAirCombatAppStateModel iAirCombatAppStateModel;
			AirCombatAppStateModel airCombatAppStateModel;
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
			PlayerPrefsStorage playerPrefsStorage;

		}

		void Controller()
		{
			AirCombatAppController airCombatAppController;
			HeroSelectableCtrl heroSelectable;
			MainCameraCtrl mainCameraCtrl;

			MapCtrl mapCrtl;
			MapItemCtrl mapItemCrtl;
		}

		void Architecture()
		{
			AirCombatApp counterApp;
			AirCombatAppGame airCombatGameApp;
		}

		public IArchitecture GetArchitecture()
		{
			return AirCombatApp.Interface;
		}
		#endregion

	}




	#region Model

	internal class PlaneModel
	{
	}

	internal interface IPlaneModel
	{
	}


	#region AppPlaneSpriteModel

	public interface IAirCombatAppPlaneSpriteModel : IModel
	{
		public abstract void LoadSprite();
		public abstract Sprite GetPlaneSprite(int id, int level);
	}

	public class AirCombatAppPlaneSpriteModel : AbstractModel, IAirCombatAppPlaneSpriteModel, ICanGetSystem
	{

		#region 字属


		private Dictionary<int, List<Sprite>> _planeSpriteDic;
		/// <summary>不同ID，等级的飞机</summary>
		public Sprite this[int id, int level] => GetPlaneSprite(id, level);



		/// <summary>不同的飞机数，planeId的数量</summary>
		public int Count
		{
			get
			{
				if (_planeSpriteDic == null)
					return 0;
				return _planeSpriteDic.Count;
			}
		}

		#endregion

		protected override void OnInit()
		{
			LoadSprite();
		}






		public void LoadSprite()
		{
			_planeSpriteDic = new Dictionary<int, List<Sprite>>();
			var sprites = this.GetUtility<ILoadUtil>().LoadAll<Sprite>(ResourcesPath.PICTURE_PLAYER_PICTURE_FOLDER);

			foreach (var sprite in sprites)
			{
				var idData = sprite.name.Split('_');
				var playerId = int.Parse(idData[0]);
				if (!_planeSpriteDic.ContainsKey(playerId))
					_planeSpriteDic[playerId] = new List<Sprite>();

				_planeSpriteDic[playerId].Add(sprite);
			}
		}

		public Sprite GetPlaneSprite(int id, int level)
		{
			int count = _planeSpriteDic[id].Count;
			if (!_planeSpriteDic.ContainsKey(id) || level >= count)
			{
				Debug.LogError("当前id或等级错误,等级" + level);
				level = count - 1;
			}

			return _planeSpriteDic[id][level];
		}
	}
	#endregion


	#region AirCombatAppModel


	// 1. 定义一个 Model 对象

	public interface IAirCombatAppModel : IModel
	{
		BindableProperty<int> Life { get; }
		BindableProperty<int> LifeMax { get; }
		BindableProperty<int> Score { get; }
		BindableProperty<int> Star { get; }
		BindableProperty<int> Diamond { get; }
		BindableProperty<int> ShieldCount { get; }
		//
		//BindableProperty<int> PowerCount { get; } // 以前叫 BombCount
		BindableProperty<int> BombCount { get; }


		/// <summary>选中后变化的颜色</summary>
		BindableProperty<Color> SelectHeroColor { get; }
		BindableProperty<Color> UnSelectHeroColor { get; }
		/// <summary>选中后变化颜色需要的时间</summary>
		BindableProperty<float> SelectHeroColorTime { get; }
		/// <summary>索引,从0开始</summary>
		BindableProperty<int> PassedLevel { get; }

		/// <summary>多少个id</summary>		
		BindableProperty<int> PlaneCount { get; }
		//
		/// <summary>
		/// ,0_0 0_1 0_2 0_3, 一个id四个level图	  ,
		/// 注意和PlaneBulletLevelMax区别开来
		/// </summary>
		BindableProperty<int> SelectedPlaneSpriteLevelMax { get; }
		/// <summary>该你来拿子弹列数的haul,玩家是12345,数组索引是01234,所以等于</summary>
		BindableProperty<int> PlaneBulletLevelMax { get; }

	}
	public class AirCombatAppModel : AbstractModel, IAirCombatAppModel
	{
		public BindableProperty<int> Life { get; } = new BindableProperty<int>();
		public BindableProperty<int> LifeMax { get; } = new BindableProperty<int>();
		public BindableProperty<int> Score { get; } = new BindableProperty<int>();
		public BindableProperty<int> Star { get; } = new BindableProperty<int>();
		public BindableProperty<int> Diamond { get; } = new BindableProperty<int>();
		public BindableProperty<int> ShieldCount { get; } = new BindableProperty<int>();
		public BindableProperty<int> BombCount { get; } = new BindableProperty<int>();
		public BindableProperty<Color> SelectHeroColor { get; } = new BindableProperty<Color>();
		public BindableProperty<Color> UnSelectHeroColor { get; } = new BindableProperty<Color>();
		public BindableProperty<float> SelectHeroColorTime { get; } = new BindableProperty<float>();
		public BindableProperty<int> PassedLevel { get; } = new BindableProperty<int>();
		public BindableProperty<int> PlaneCount { get; } = new BindableProperty<int>();
		public BindableProperty<int> SelectedPlaneSpriteLevelMax { get; } = new BindableProperty<int>();
		public BindableProperty<int> PlaneBulletLevelMax { get; } = new BindableProperty<int>();


		protected override void OnInit()
		{
			IStorageUtil storage = this.GetUtility<IStorageUtil>();

			// 设置初始值（不触发事件）
			LifeMax.Value=100;    
			Life.SetValueWithoutEvent(LifeMax);  //开始满血
			Life.Register(newCount => storage.Set<int>(nameof(Life), newCount));
			LifeMax.Register(newCount => storage.Set<int>(nameof(LifeMax), newCount));
			//
			SelectedPlaneSpriteLevelMax.SetValueWithoutEvent(3);//0_0, 0_1, 0_2, 0_3.不是4,因为从0开始算的
			PlaneBulletLevelMax.SetValueWithoutEvent(4);//01234
			PlaneCount.SetValueWithoutEvent(NSPlaneSpritesModel.Single.Count);

			//
			Score.SetValueWithoutEvent(0);
			Score.Register(newCount => storage.Set<int>(nameof(Score), newCount));
			Star.SetValueWithoutEvent(9999);
			Star.Register(newCount => storage.Set<int>(nameof(Star), newCount));
			Diamond.SetValueWithoutEvent(9999);
			Diamond.Register(newCount => storage.Set<int>(nameof(Diamond), newCount));
			//
			ShieldCount.SetValueWithoutEvent(0);
			ShieldCount.Register(newCount => storage.Set<int>(nameof(ShieldCount), newCount));
			BombCount.SetValueWithoutEvent(0);             
			BombCount.Register(newCount => storage.Set<int>(nameof(BombCount), newCount));
			//
			SelectHeroColorTime.SetValueWithoutEvent(0.5f);
			SelectHeroColor.SetValueWithoutEvent(Color.white);
			UnSelectHeroColor.SetValueWithoutEvent("#8C8C8C".Hex2Color());
			// 当数据变更时 存储数据
			SelectHeroColor.Register(newCount => storage.Set<string>(nameof(SelectHeroColor), newCount.ToString()));
			SelectHeroColorTime.Register(newCount => storage.Set<float>(nameof(SelectHeroColorTime), newCount));
			//
			PassedLevel.SetValueWithoutEvent(1);
			PassedLevel.Register(newCount => storage.Set<int>(nameof(PassedLevel), newCount));

		}


		#region 字属
		/// <summary> 在游戏中的临时等级  </summary>
		int _tmpPlaneLevel;

		void SetTmpPlaneLevel(int value)
		{
			if (value < PlaneBulletLevelMax)
			{
				_tmpPlaneLevel = value;
			}
		}


		public int GetLifeMax()
		{

			var key = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.value
				, PlaneProperty.Property.life.Enum2String());
			return this.GetUtility<IStorageUtil>().Get<int>(key);
		}
		#endregion


	}
	#endregion


	#region AirCombatAppStateModel
	/// <summary>想要区别开来,有的属性在进入战斗界面你才有用,平时不需要加载</summary>
	public interface IAirCombatAppStateModel : IModel
	{
		BindableProperty<HandMode> HandMode { get; }
		BindableProperty<ESceneName> CurScene { get; }
		BindableProperty<ESceneName> TarScene { get; }
		BindableProperty<GameState> E_GameState { get; } //变量名优先级大于枚举，导致枚举用不出来 ，所以加E_
		BindableProperty<LevelState> E_LevelState { get; } //变量名优先级大于枚举，导致枚举用不出来 ，所以加E_
		BindableProperty<Hero> SelectedHero { get; }

		/// <summary>用SetSelectedPlaneIDCommand</summary>
		BindableProperty<int> SelectedPlaneID { get; }


		BindableProperty<int> SelectedPlaneSpriteLevel { get; }

		/// <summary>
		/// 子弹样式.游戏中吃到buff会升的那种级.
		/// 看看使用时对应的是索引,01234,所以等于4
		/// </summary>
		BindableProperty<int> PlaneBulletLevel { get; }

		//
		BindableProperty<float> PlaneSpeed { get; }
		BindableProperty<float> CameraSpeed { get; }



		/// <summary>实际使用的是在父节点中的索引</summary>
		BindableProperty<int> SelectHeroID { get; }

		/// <summary>实际使用的是在父节点中的索引</summary>
		BindableProperty<int> SelectedLevel { get; }
		BindableProperty<int> CurLevel { get; }  
		//
		BindableProperty<int> CurHp { get; }
		/// <summary>等级对应的HpMax不同</summary>
		BindableProperty<int> CurHpMax { get; }  
		//
		/// <summary>已经通关数.+1就是最大的可以解锁的关卡</summary>
		/// <summary>Boss死亡结算面板的文字显示</summary>
		BindableProperty<bool> IsFinishOneLevel { get; }
		/// <summary>玩家刚被撞的懵逼状态</summary>
		BindableProperty<bool> Invincible { get; }


		public int GetMoney(string key);
		public void SetMoney(string key, int money);

	}

	public class AirCombatAppStateModel : AbstractModel, IAirCombatAppStateModel, ICanGetSystem, ICanSendQuery,ICanGetModel
	{
		public BindableProperty<HandMode> HandMode { get; } = new BindableProperty<HandMode>();
		public BindableProperty<ESceneName> CurScene { get; } = new BindableProperty<ESceneName>();
		public BindableProperty<ESceneName> TarScene { get; } = new BindableProperty<ESceneName>();
		public BindableProperty<GameState> E_GameState { get; } = new BindableProperty<GameState>();
		public BindableProperty<LevelState> E_LevelState { get; } = new BindableProperty<LevelState>();
		public BindableProperty<Hero> SelectedHero { get; } = new BindableProperty<Hero>();
		public BindableProperty<float> PlaneSpeed { get; } = new BindableProperty<float>();
		public BindableProperty<float> CameraSpeed { get; } = new BindableProperty<float>();
		public BindableProperty<int> SelectedPlaneID { get; } = new BindableProperty<int>();
		public BindableProperty<int> SelectedPlaneSpriteLevel { get; } = new BindableProperty<int>();
		public BindableProperty<int> PlaneBulletLevel { get; } = new BindableProperty<int>();


		//
		/// <summary>看了代码这个对应关卡选择界面的Index</summary>
		public BindableProperty<int> SelectedLevel { get; } = new BindableProperty<int>();
		/// <summary>看了代码这个影响场景对应的Bg</summary>
		public BindableProperty<int> CurLevel { get; } = new BindableProperty<int>();
		/// <summary>看了代码这个对应英雄选择界面的Index</summary>
		public BindableProperty<int> SelectHeroID { get; } = new BindableProperty<int>();
		//
		public BindableProperty<int> CurHp { get; } = new BindableProperty<int>();
		/// <summary>没做,相对应每个等级的HpMax</summary>
		public BindableProperty<int> CurHpMax { get; } = new BindableProperty<int>();
		//
		public BindableProperty<bool> IsFinishOneLevel { get; } = new BindableProperty<bool>();
		/// <summary>战斗时被撞了一次的懵逼状态,不能再被撞,也不能撞别人</summary>        
		public BindableProperty<bool> Invincible { get; } = new BindableProperty<bool>();



		protected override void OnInit()
		{
			IStorageUtil storage = this.GetUtility<IStorageUtil>();

			HandMode.SetValueWithoutEvent(this.GetUtility<IStorageUtil>().Get<int>(DataKeys.HAND_MODE).Int2Enum<HandMode>());  //开始满血
			TarScene.SetValueWithoutEvent(ESceneName.NULL);
			//

			//
			CurHpMax.SetValueWithoutEvent(100);
			CurHp.SetValueWithoutEvent(CurHpMax);
			//
			SelectedHero.SetValueWithoutEvent(Hero.Player_0);
			SelectedHero.SetValueWithoutEvent(Hero.Player_0);
			PlaneBulletLevel.SetValueWithoutEvent(0);  //一开始都是一类子弹,索引是0
			PlaneSpeed.SetValueWithoutEvent(5.0f);
			CameraSpeed.SetValueWithoutEvent(1.0f);

			//
			CurScene.SetValueWithoutEvent(ESceneName.Game);

			//PassedLevel.SetValueWithoutEvent(storage.GetOut<int>(nameof(PassedLevel)));
			//
			SelectHeroID.SetValueWithoutEvent(0);
			SelectHeroID.Register(newCount => storage.Set<int>(nameof(SelectHeroID), newCount));
			//
			HandMode.Register(newCount => storage.Set(nameof(HandMode), newCount.ToString()));
			CurScene.Register(newCount => storage.Set(nameof(CurScene), newCount.ToString()));
			TarScene.Register(newCount => storage.Set(nameof(TarScene), newCount.ToString()));
			//
			E_GameState.SetValueWithoutEvent(GameState.NULL);
			E_GameState.Register(newCount => storage.Set(nameof(E_GameState), newCount.ToString()));
			E_GameState.Register(value =>
			{
				switch (value)
				{
					case GameState.START:
						//外部直接监听E_GameState
						break;
					case GameState.PAUSE:
						//外部直接监听E_GameState
						break;
					case GameState.CONTINUE:
						//外部直接监听E_GameState
						break;
					case GameState.END:
						//外部直接监听E_GameState
						break;
				}

			});
			//
			E_LevelState.SetValueWithoutEvent(LevelState.NULL);
			//
			SelectedHero.Register(newCount => storage.Set(nameof(SelectedHero), newCount.Enum2String()));
			SelectedPlaneID.SetValueWithoutEvent(0);//刚进来显示首架飞机
			SelectedPlaneID.RegisterWithInitValue(id=>SelectedPlaneSpriteLevel.Value= this.SendQuery(new SelectedPlaneLevelQuery()));//根据换飞机id更新等级 
			//

			SelectedPlaneSpriteLevel.Register(newCount => storage.Set<int>(nameof(SelectedPlaneSpriteLevel), newCount));   //升级时存储
			//
			SelectedLevel.SetValueWithoutEvent(0);
			SelectedLevel.Register(newCount => storage.Set<int>(nameof(SelectedLevel), newCount));
			SelectedLevel.Register(value => CurLevel.Value = value);//我也不知道这有多了个CurLevel的作用
			SelectedLevel.Register(value => ChangePropertyData());//我也不知道这有多了个CurLevel的作用
			CurLevel.SetValueWithoutEvent(0);//当前是所选
			//CurLevel.Register(newCount => storage.Set<int>(nameof(CurLevel), newCount));

			//
			//
		   //
			IsFinishOneLevel.SetValueWithoutEvent(false);
			IsFinishOneLevel.Register(newCount => storage.Set<string>(nameof(IsFinishOneLevel), newCount.ToString()));
			//
			Invincible.SetValueWithoutEvent(false);
		}


		#region Money


		#region GetMoney
		/// <summary>star,diamond</summary>
		public int GetMoney(string key)
		{
			var money = 0;
			switch (key)
			{
				case DataKeys.STAR:
					money = this.GetUtility<IStorageUtil>().Get<int>(DataKeys.STAR);
					break;
				case DataKeys.DIAMOND:
					money = this.GetUtility<IStorageUtil>().Get<int>(DataKeys.DIAMOND);
					break;
				default:
					Debug.LogError("当前传入的key无法识别，key：" + key);
					break;
			}

			return money;
		}

		public int GetStar()
		{
			return GetMoney(DataKeys.STAR);
		}

		public int GetDiamond()
		{

			return GetMoney(DataKeys.DIAMOND);
		}

		public int GetMoney(DataKeys datakey)
		{
			string key = datakey.ToString();
			return GetMoney(key);
		}
		#endregion

		#region SetMoney


		/// <summary></summary>
		public void SetMoney(string key, int money)
		{
			switch (key)
			{
				case DataKeys.STAR:
					this.GetUtility<IStorageUtil>().Set(DataKeys.STAR, money);
					break;
				case DataKeys.DIAMOND:
					this.GetUtility<IStorageUtil>().Set(DataKeys.DIAMOND, money);
					break;
				default:
					Debug.LogError("当前传入的key无法识别，key：" + key);
					break;
			}
		}
		public void SetStar(int money)
		{
			SetMoney(DataKeys.STAR, money);
		}
		public void SetDiamond(int money)
		{
			SetMoney(DataKeys.DIAMOND, money);
		}

		public bool ChangeStar(int change)
		{
			int after = GetStar() + change;
			if (after < 0)
			{
				return false;
			}
			SetStar(after);
			return true;
		}
		public bool ChangeDiamond(int change)
		{
			int after = GetStar() + change;
			if (after < 0)
			{
				return false;
			}
			SetDiamond(after);
			return true;
		}
		#endregion


		#endregion


		#region PlaneLevel 
		/**
		 "planes": [
		 {
			 "planeId": 0,
			 "levelValue": 0,
			 "attackTime":1,
			 "attack": {"name":"攻击","propertyValue":5,"costValue":180,"costUnit":"star","grouth":10,"maxVaue": 500},
			 "fireRate": {"name":"攻速","propertyValue":80,"costValue":200,"costUnit":"star","grouth":1,"maxVaue": 100},
			 "life": {"name":"生命","propertyValue":100,"costValue":200,"costUnit":"star","grouth":50,"maxVaue": 1000},
			 "upgrades": {"name":"升级","coefficient": 2,"max":4,"0": 100,"1": 200,"2": 300,"3": 400,"costUnit":"diamond"}
		 },
		*/
		private void ChangePropertyData()
		{
			//获取升级系数，修改数据
			var ratioKey = this.GetUtility<IKeysUtil>().GetPropertyKeysWithoutPlaneID(DataKeys.UPGRADES_RATIO);
			var ratioValue = this.GetUtility<IStorageUtil>().Get<int>(ratioKey);

			ChangeData(ratioValue, PropertyItem.ItemKey.value, PlaneProperty.Property.attack);
			ChangeData(ratioValue, PropertyItem.ItemKey.maxVaue, PlaneProperty.Property.attack);
			ChangeData(ratioValue, PropertyItem.ItemKey.grouth, PlaneProperty.Property.attack);
			//
			ChangeData(ratioValue, PropertyItem.ItemKey.value, PlaneProperty.Property.life);
			ChangeData(ratioValue, PropertyItem.ItemKey.maxVaue, PlaneProperty.Property.life);
			ChangeData(ratioValue, PropertyItem.ItemKey.grouth, PlaneProperty.Property.life);
			//
			ChangeData(ratioValue, PropertyItem.ItemKey.value, PlaneProperty.Property.fireRate);
			ChangeData(ratioValue, PropertyItem.ItemKey.maxVaue, PlaneProperty.Property.fireRate);
			ChangeData(ratioValue, PropertyItem.ItemKey.grouth, PlaneProperty.Property.fireRate);
		}



		/// <summary>暂时设定为 grouth和max乘以系数</summary>
		private void ChangeData(int ratio, PropertyItem.ItemKey itemKey, PlaneProperty.Property property)
		{
			int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			var propertyKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property.ToString() + itemKey);
			var propertyValue = this.GetUtility<IStorageUtil>().Get<int>(propertyKey);
			propertyValue *= ratio;
			this.GetUtility<IStorageUtil>().Set(propertyKey, propertyValue);
		}

		#endregion  
	}



	#endregion
	#endregion


	#region Command



	public class EndLevelCommand : AbstractCommand
	{
		protected override void OnExecute()
		{

		}
	}


	public class AwakeBossCreatorEventCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			Debug.Log("AwakeBossCreatorEventCommand");
			this.SendEvent<AwakeBossCreatorEvent>();
		}
	}
	public class OpenGamePanelCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			Debug.Log("OpenGamePanelCommand");


			this.SendEvent<EnterGameSceneEvent>();

			{   //UI 
				IUISystem sys = this.GetSystem<IUISystem>(); //报过空
				sys.Open(ResourcesPath.PREFAB_GAME_UI_PANEL);
			}

		}
	}
	/// <summary></summary>
	public class MoveComponentForSpriteRendererCommand : AbstractCommand
	{
		MoveOtherComponent _moveCpt;
		Vector2 _dir;
		SpriteRenderer _sr;

		public MoveComponentForSpriteRendererCommand(MoveOtherComponent moveCpt, Vector2 dir, SpriteRenderer sr)
		{
			_moveCpt = moveCpt;
			_dir = dir;
			_sr = sr;
		}

		protected override void OnExecute()
		{

			if (this.SendCommand(new InCameraBorderCommand(_dir, _sr)))
			{
				_moveCpt.Move(_dir);

			}
		}
	}

	public class InCameraBorderCommand : AbstractCommand<bool>
	{
		Camera _camera;
		Vector2 _dir;
		SpriteRenderer _sr;


		public InCameraBorderCommand(Vector2 dir, SpriteRenderer sr)
		{

			if (ExtendJudge.IsOnceNull(dir, sr))
			{
				Debug.LogErrorFormat($"{dir},{sr}");
			}
			_sr = sr;
			_dir = dir;
			_camera = _sr.transform.MainOrOtherCamera();

		}

		protected override bool OnExecute()
		{
			if (_dir == Vector2.up) return _sr.bounds.min.y <= _camera.CameraSizeMax().y; //up
			if (_dir == Vector2.down) return _sr.bounds.min.y >= _camera.CameraSizeMin().y; //down
			if (_dir == Vector2.right) return _sr.bounds.max.x <= _camera.CameraSizeMax().x; //right
			if (_dir == Vector2.left) return _sr.bounds.min.x >= _camera.CameraSizeMin().x; //left
			return true;
		}
	}



	#region Star 钻石
	public class ChangeStarCommand : AbstractCommand<bool>
	{
		int _change = 0;

		public ChangeStarCommand(int change)
		{
			_change = change;
		}



		protected override bool OnExecute()
		{
			var model = this.GetModel<IAirCombatAppModel>();
			int cur = this.GetModel<IAirCombatAppModel>().Star;
			int after = cur + _change;

			if (after >= 0)//有钱&&上升空间
			{
				this.GetModel<IAirCombatAppModel>().Star.Value = after;
				"ChangeStarCommand".LogInfo();
				return true;
			}
			else
			{
				string str = "你没星星了！";
				str += "需要" + _change;
				str.LogInfo();
				// this.GetSystem<IUISystem>().ShowDialog("你没星星了！");
				return false;
			}
		}
	}


	public class SetStarCommand : AbstractCommand
	{
		int _after = 0;

		public SetStarCommand(int after)
		{
			_after = after;
		}
		protected override void OnExecute()
		{
			this.GetModel<IAirCombatAppModel>().Star.Value = _after;
		}
	}

	public class SetDiamondCommand : AbstractCommand
	{
		int _after = 0;


		public SetDiamondCommand(int change)
		{
			_after = change;
		}
		protected override void OnExecute()
		{
			this.GetModel<IAirCombatAppModel>().Diamond.Value = _after;
		}
	}

	public class ChangeDiamondCommand : AbstractCommand<bool>
	{
		int _change = 0;


		/// <summary>增加正，减少负</summary>
		public ChangeDiamondCommand(int change)
		{
			_change = change;
		}

		protected override bool OnExecute()
		{
			var model = this.GetModel<IAirCombatAppModel>();
			int cur = model.Diamond;
			int after = cur + _change;

			if (after >= 0)//有钱&&上升空间
			{
				model.Diamond.Value = after;
				return true;
			}
			else
			{
				string str = "你没钻石了！";
				str += "需要" + _change;
				str.LogInfo();
				// this.GetSystem<IUISystem>().ShowDialog("你没星星了！");
				return false;
			}
		}
	}
	#endregion
	// 引入 Command


	public class IncreasePlanePropertyCommand : AbstractCommand
	{
		RectTransform _rect;

		public IncreasePlanePropertyCommand( RectTransform rect)
		{
			_rect = rect;
		}



		protected override void OnExecute()
		{
			string property = (_rect.name).TrimName(TrimNameType.DashAfter).LowerFirstLetter();//PropertyItem_Attack=>attack,fireRate,life
			int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			string pre = planeID + property;
			//
			string costKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(pre + PropertyItem.ItemKey.cost);
			int cost = this.GetUtility<IStorageUtil>().Get<int>(costKey);
			//
			string maxKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property + PropertyItem.ItemKey.maxVaue);
			string curKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property + PropertyItem.ItemKey.value);
			string grouthKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property + PropertyItem.ItemKey.grouth);
			int max = this.GetUtility<IStorageUtil>().Get<int>(maxKey);
			int cur = this.GetUtility<IStorageUtil>().Get<int>(curKey);
			int grouth = this.GetUtility<IStorageUtil>().Get<int>(grouthKey);
			int after = cur + grouth;


			if (cur < max )//有上升空间
			{
				 bool canAfford = this.SendCommand(new ChangeStarCommand(-cost));
				if (!canAfford) return;
				//
				if (after >= max)//到顶了
				{
					after = max;
				}
			   
				string valueKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property + ItemKey.value);
				this.GetUtility<IStorageUtil>().Set<int>(valueKey, after);

				Debug.LogFormat("IncreasePlanePropertyCommand {0}:{1} ", valueKey, after);
				this.SendEvent<IncreasePlanePropertyEvent>();
			}
			else
			{
				("数值到顶了！").LogInfo();
				// this.GetSystem<IUISystem>().ShowDialog("你没星星了！");
			}


		}
	}
	public class IncreasePlaneAttackCommand : AbstractCommand
	{
		int _change;

		public IncreasePlaneAttackCommand(int change)
		{
			_change = change;
		}

		protected override void OnExecute()
		{
			int id = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			string pre = id + DataKeys.ATTACK;
			//
			//
			int max = this.GetUtility<IStorageUtil>().Get<int>(pre + ItemKey.maxVaue);
			int cur = this.GetUtility<IStorageUtil>().Get<int>(pre + ItemKey.value);
			int after = cur + _change;


			if (after >= 0 && cur < max)//有上升空间
			{
				if (after >= max)//到顶了
				{
					after = max;
				}

				this.GetUtility<IStorageUtil>().SetObject(pre + ItemKey.value, after);
			}
			else
			{
				("数值到顶了！").LogInfo();
				// this.GetSystem<IUISystem>().ShowDialog("你没星星了！");
			}

		}
	}
	public class IncreasePlaneFireRateCommand : AbstractCommand
	{
		int _value;

		public IncreasePlaneFireRateCommand(int value)
		{
			_value = value;
		}

		protected override void OnExecute()
		{


		}
	}
	public class IncreasePlaneLifeCommand : AbstractCommand
	{
		int _value;

		public IncreasePlaneLifeCommand(int value)
		{
			_value = value;
		}

		protected override void OnExecute()
		{


		}
	}
	public class AddPlaneLevelCommand : AbstractCommand<bool>
	{
		protected override bool OnExecute()
		{
		   int after =   this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID+1;
		   return this.SendCommand(new SetPlaneSpriteLevelCommand(after));
		}
	}
	public class SetPlaneSpriteLevelCommand : AbstractCommand<bool>
	{
		int _afterPlaneLevel;

		public SetPlaneSpriteLevelCommand(int afterPlaneLevel)
		{
			_afterPlaneLevel = afterPlaneLevel;
		}

		protected override bool OnExecute()
		{
			int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			string levelKey = this.GetUtility<IKeysUtil>().GetPropertyKeys( planeID , DataKeys.LEVEL);
			int levelValue = this.GetUtility<IStorageUtil>().Get<int>(levelKey); //0level
			string costKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + DataKeys.UPGRADES + levelValue);
			int costValue = this.GetUtility<IStorageUtil>().Get<int>(costKey);
		  
			//
			int planeLevelMax = this.GetModel<IAirCombatAppModel>().SelectedPlaneSpriteLevelMax;
			if ( _afterPlaneLevel <= planeLevelMax)
			{   bool canAfford = this.SendCommand(new ChangeDiamondCommand(-costValue));
				if (canAfford)//需要要素共同待定,共同增减,共同回退,所以不能&&
				{ 
					this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel.Value = _afterPlaneLevel;
					this.GetUtility<IStorageUtil>().Set(levelKey, _afterPlaneLevel);//}("0level",{levelValue),id为0的飞机level={levelValue}

					return true;
				
				}

			}
			return false;
		}
	}



	public class SelectHeroCommand : AbstractCommand
	{


		protected override void OnExecute()
		{
			int cur = this.GetModel<IAirCombatAppStateModel>().SelectHeroID.Value;
			// this.GetSystem<IAudioSystem>().Stop(cur.Int2String<Hero>());
			this.SendEvent<SelectHeroEvent>();

		}
	}

	/// <summary>设置任务选择时颜色的参数</summary>
	public class SetSelectHeroParaCommand : AbstractCommand
	{
		Color _color;
		float _time;

		public SetSelectHeroParaCommand(Color color, float time)
		{
			_color = color;
			_time = time;
		}

		protected override void OnExecute()
		{
			this.GetModel<IAirCombatAppModel>().SelectHeroColor.Value = _color;
			this.GetModel<IAirCombatAppModel>().SelectHeroColorTime.Value = _time;
		}
	}


	public class SwitchPlaneCommand : AbstractCommand
	{
		EDir _dir = 0;

		public SwitchPlaneCommand(EDir dir)
		{
			_dir = dir;
		}

		protected override void OnExecute()
		{
			int change = _dir == EDir.LEFT ? -1 : 1;
			if (change != 1 && change != -1) //飞机的切换是顺序的
			{
				return;
			}
			int max = this.GetModel<IAirCombatAppModel>().PlaneCount;
			int id = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			id += change;
			id = id.Clamp(0, max - 1);
			STest.LogSwitchPlane(id,change);
			//
			this.SendCommand(new SetSelectedPlaneIDCommand(id));
		}
	}

	public class SetSelectedPlaneIDCommand : AbstractCommand
	{
		int _planeID = 0;

		public SetSelectedPlaneIDCommand(int planeID)
		{
			_planeID = planeID;
		}

		protected override void OnExecute()
		{
			int max = this.GetModel<IAirCombatAppModel>().PlaneCount;
			if (_planeID < 0 || _planeID > max - 1)
			{
				return;
			}
			//
			this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID.Value = _planeID;
		}
	}

	public class OpenSelectHeroPanelCommand : AbstractCommand
	{

		protected override void OnExecute()
		{
			RectTransform rect = this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_SELECTED_HERO_PANEL);
		}
	}

	public class OpenStartGamePanelCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_STARTGAME_PANEL);
		}
	}

	public class OpenStrengthenPanelCommand : AbstractCommand
	{
		Action _onComplete;

		/// <summary>比如设置了金币，但是对金币的监听是在打开面板是才注册的，所以onComplete才好触发</summary>
		public OpenStrengthenPanelCommand(Action onComplete)
		{
			_onComplete = onComplete;
		}

		public OpenStrengthenPanelCommand()
		{
		}
		protected override void OnExecute()
		{
			//这块是上个界面，选择飞机的界面来做的（如果再直接打开升级界面邮箱使用返回按钮的情况下可以用） 
			this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_STRENGTHEN_PANEL);
			"打开升级面板".LogInfo();
			//
			_onComplete.DoIfNotNull();
		}
	}

	public class OpenSelectLevelPanelCommand : AbstractCommand
	{
		Action _onComplete;

		/// <summary>比如设置了金币，但是对金币的监听是在打开面板是才注册的，所以onComplete才好触发</summary>
		public OpenSelectLevelPanelCommand(Action onComplete)
		{
			_onComplete = onComplete;
		}

		public OpenSelectLevelPanelCommand()
		{
		}

		protected override void OnExecute()
		{
			//这块是上个界面，选择飞机的界面来做的（如果再直接打开升级界面邮箱使用返回按钮的情况下可以用） 
			this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_SELECTLEVEL_PANEL);
			"打开关卡选择面板".LogInfo();
			//
			_onComplete.DoIfNotNull();
		}
	}

	public class OpenLoadingPanelCommand : AbstractCommand
	{
		int _levelIndex = 0;


		public OpenLoadingPanelCommand(int level)
		{
			_levelIndex = level;
		}

		protected override void OnExecute()
		{
			this.GetModel<IAirCombatAppStateModel>().SelectedLevel.Value = _levelIndex;
			//
			this.GetSystem<IAudioSystem>().PauseMusic();
			this.GetSystem<IAudioSystem>().StopVoice();
			this.GetSystem<IAudioSystem>().PlaySound(AudioUI.UI_Loading);

			//
			UnityEngine.AsyncOperation loadScene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);//Game
			loadScene.allowSceneActivation = false;
			//
			this.SendEvent<CloseSelectLevelPanelEvent>();//关闭关卡选择界面会注册事件
			//TODO 设置场景状态
			RectTransform panel = this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_LOADING_PANEL);
			Slider slider = panel.GetComponentInChildren<Slider>();
			MonoBehaviour mono = panel.GetComponent<MonoBehaviour>();
			Text text = panel.GetComponentInChildren<Text>();
			//
			mono.PlayPrgSmooth(slider
			 , text
			 , step: 0.5f
			 , stepTime: 0.5f
			 , stepSmooth: 0.05f
			 , () => OnLoadedSecen(loadScene));
		}

		void OnLoadedSecen(UnityEngine.AsyncOperation tarScene)
		{
			SceneManager.UnloadSceneAsync(StSceneIndex.Main);
			tarScene.allowSceneActivation = true;
			this.SendCommand<OnLoadGameSceneCommand>();

		}

	}



	public class OnLoadGameSceneCommand : AbstractCommand
	{
		protected override void OnExecute()
		{

			this.GetModel<IAirCombatAppStateModel>().CurScene.Value = ESceneName.Game;
			this.GetModel<IAirCombatAppStateModel>().TarScene.Value = ESceneName.Game;
			//Assets / StreamingAssets / Config / InitPlane.json   //方便调试
			this.SendCommand(new SetSelectedPlaneIDCommand(1)); //0123
			this.GetModel<IAirCombatAppStateModel>().PlaneBulletLevel.Value = 0;
			this.SendCommand(new SetPlaneSpriteLevelCommand(3));
			//
			this.GetSystem<IAudioSystem>().ResumeMusic();
			this.SendCommand<OpenGamePanelCommand>();
			this.SendCommand<GameStateStartCommand>();
		}
	}

	/// <summary>改变状态有时需要等待其他模块的加载</summary> 
	public class GameStateStartCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
		  this.GetSystem<ICoroutineSystem>().StartInner(GameStart());
		}


		IEnumerator GameStart()
		{             
			//第一种写法
			while ( !NSOrderSystem.Single.GameDataMgrInited)//等待条件
			{
				yield return new WaitForEndOfFrame(); //等一帧 
			}
			//上面条件改变跳出后,执行后面的
			this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.START;//NULL!=START触发广播    
	   
		}
	}


	public class InitShootCtrlPlayerCommand : AbstractCommand<ShootCtrl>
	{
		Transform _muzzle;
		BulletType _bulletType;



		public InitShootCtrlPlayerCommand(Transform bulletRoot, BulletType bulletType)
		{
			_muzzle = bulletRoot ?? throw new ArgumentNullException(nameof(bulletRoot));
			_bulletType = bulletType;
		}

		protected override ShootCtrl OnExecute()
		{
			return _muzzle.GetOrAddComponent<ShootCtrl>().InitComponentPlayer(_bulletType, _muzzle);
		}
	}



	#region  NewGameProcessNormalEvent
	/// <summary>
	/// 生成事件类
	/// <br/>就一个实例不知道为什么单独摘出来
	/// </summary>
	/// <param name="spawnAction">生成回调</param>
	/// <param name="spawningNum">已经生成</param>
	/// <param name="spawnNum">总生成</param>
	/// <returns></returns>
	public class NewGameProcessNormalEventCommand : AbstractCommand<GameProcessNormalEvent>
	{
		private Action _spawnAction;
		private Func<int> _spawningNum;
		private int _spawnNum;

		public NewGameProcessNormalEventCommand(Action spawnAction, Func<int> spawningNum, int spawnNum)
		{
			_spawnAction = spawnAction;
			_spawningNum = spawningNum;
			_spawnNum = spawnNum;
		}

		protected override GameProcessNormalEvent OnExecute()
		{
			GameProcessNormalEvent e = new GameProcessNormalEvent(_spawnAction, _spawningNum, _spawnNum);
			return e;
		}
	}




	#region New
	/// <summary>系统之间不要用Command</summary>
	public class UpdateGameProcessNormalEventCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>系统之间不要用Command</summary>
	public class UpdateGameProcessTriggerEventCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			throw new NotImplementedException();
		}
	}

	public class NewGameProcessTriggerEventCommand : AbstractCommand<GameProcessTriggerEvent>
	{
		private float _prg;
		private Action _action;
		private bool _needPauseProcess;
		private Func<bool> _isEnd;

		public NewGameProcessTriggerEventCommand(float prg, Action action, bool needPauseProcess, Func<bool> isEnd)
		{
			_prg = prg;
			_action = action;
			_needPauseProcess = needPauseProcess;
			_isEnd = isEnd;
		}

		protected override GameProcessTriggerEvent OnExecute()
		{
			GameProcessTriggerEvent trigger = new GameProcessTriggerEvent(_prg, _action, _needPauseProcess, _isEnd);
			return trigger;
		}
	}

	public class NewGameProcessTriggerEventListCommand : AbstractCommand<List<GameProcessTriggerEvent>>
	{
		protected override List<GameProcessTriggerEvent> OnExecute()
		{

			return null;

		}
	}
	#endregion  

	#endregion


	public class InitedPlayerCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			this.SendEvent<InitPlayerEvent>();
		}
	}
	public class InitEnemyCreatorLstCommand : AbstractCommand<List<IEnemyCreator>>
	{
		private EnemyType _enemyType;
		private AllEnemyData _allEnemyData;
		private PathDataMgr _pathDataMgr;
		private LevelData _levelData;

		public InitEnemyCreatorLstCommand(EnemyType enemyType, AllEnemyData allEnemyData, PathDataMgr pathDataMgr, LevelData levelData)
		{
			_enemyType = enemyType;
			_allEnemyData = allEnemyData;
			_pathDataMgr = pathDataMgr;
			_levelData = levelData;
		}

		protected override List<IEnemyCreator> OnExecute()
		{
			if (ExtendJudge.IsOnceNull(_allEnemyData, _pathDataMgr, _levelData))
			{
				Debug.LogError("生成Creator数据缺失，类型：" + _enemyType);
			}
			List<IEnemyCreator> lst = InitEnemyCreatorLst(_enemyType, _allEnemyData, _pathDataMgr, _levelData);
			Debug.Log($"InitEnemyCreatorLstCommand，类型:{_enemyType}，长度（波次）：{lst.Count}");
			return lst;
		}



		#region Pri
		List<IEnemyCreator> InitEnemyCreatorLst(EnemyType enemyType, AllEnemyData allEnemyData, PathDataMgr pathDataMgr, LevelData levelData)
		{
			List<IEnemyCreator> list = new List<IEnemyCreator>();
			foreach (PlaneCreatorData data in levelData.PlaneCreaterDatas) //这里可以到LoadCreaterData这看
			{
				if (data.Type == enemyType) //ever error;data都是normal
				{
					list.Add(SpawnCreator(data, allEnemyData, pathDataMgr));
				}
				//Debug.Log($"_pathData.EnemyType == enemyType=>{_pathData.E_BulletType}=={enemyType}");
			}
			if (list.IsNotNull() && list.Count > 0)
			{
				return list;
			}
			else
			{
				throw new System.Exception($"Creator初始化失败：{enemyType}");
			}
		}


		private IEnemyCreator SpawnCreator(PlaneCreatorData data, AllEnemyData allEnemyData, PathDataMgr pathDataMgr)
		{
			var go = new GameObject();
			Transform parentTrans = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.Creator_EnemyCreator);
			go.SetParent(parentTrans);
			//这里我需要区别不同节点（比如X不同）的同类型creator   。这里用的hisGetSiblingIndex()
			//放在后面为了要用时方便SubString
			go.name = data.Type.Enum2String() + GameObjectName.Creator + go.transform.GetSiblingIndex();
			var creator = go.GetOrAddComponent<EnemyPlaneCreator>();
			creator.Init(data, allEnemyData, pathDataMgr);

			return creator;
		}
		#endregion

	}


	/// <summary>如果同一个预制体,想要因为类型不同,新建不同的Pool</summary>
	public class AddEnemyGameObjectPoolCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			//飞机预制体不需要按类型细分，模型数据才需要
			JsonData jsonData = ResourcesPath.CONFIG_ENEMY.JsonPath2JsonData_JsonMapper();
			GameObject spawnTrans = Camera.main.transform.FindTopOrNewPath(GameObjectPath.GameRoot_PLANE).gameObject;
			Transform poolParent = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectName.Pool);
			for (int i = 0; i < EnemyType.COUNT.Enum2Int(); i++)
			{
				if (jsonData.Keys.Contains(i.Int2String<EnemyType>())) //不同类型
				{
					for (int j = 0; j < jsonData[i].Count; j++) //不同id
					{

						GameObject prefab = this.GetSystem<IGameObjectSystem>().Instantiate(ABName.Plane, LoadType.AB);
						GameObjectPool pool = new GameObjectPool();
						pool.Init(prefab, poolParent.transform, 5, false);
						//
						this.GetSystem<IGameObjectPoolSystem>().Add(ResourcesPath.PREFAB_BULLET, pool);
						//string newKey = this.GetSystem<IJsonSystem>().GetEnemyPlaneKey(i.Int2Enum<EnemyType>(), j);
						//this.GetSystem<IGameObjectPoolSystem>().InitParas(newKey, pool);
					}
				}
			}
		}
	}


	#region Spawn


	public class SpawnEnemyLifeViewCommand : AbstractCommand<EnemyLifeComponent>
	{
		Transform _lifeTrans;
		LifeComponent _lifeComponent;
		SpriteRenderer _sr;

		public SpawnEnemyLifeViewCommand(Transform lifeTrans, LifeComponent lifeComponent, SpriteRenderer sr)
		{
			_lifeTrans = lifeTrans ?? throw new ArgumentNullException(nameof(lifeTrans));
			_lifeComponent = lifeComponent ?? throw new ArgumentNullException(nameof(lifeComponent));
			_sr = sr ?? throw new ArgumentNullException(nameof(sr));
		}

		protected override EnemyLifeComponent OnExecute()
		{
			Transform poolT = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.Pool_LifePool);
			GameObject go = this.GetSystem<IGameObjectPoolSystem>().SpawnOrAdd(ResourcesPath.PREFAB_ENEMY_LIFE, poolT);
			go.SetParent(_lifeTrans);
			EnemyLifeComponent enemyLifeCpt = go.GetOrAddComponent<EnemyLifeComponent>().Init(_lifeComponent, _sr);
			return enemyLifeCpt;
		}
	}




	public class SpawnStarViewsCommand : AbstractCommand
	{
		int _num;
		Vector3 _pos;

		public SpawnStarViewsCommand(int num, Vector3 pos)
		{
			_num = num;
			_pos = pos;
		}

		protected override void OnExecute()
		{
			for (int i = 0; i < _num; i++)
			{
				GameObject go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.PREFAB_ITEM_ITEM, GameObjectName.Star); //底层是GameObjectPool
				go.GetOrAddComponent<StarItemView>().InitParas(_pos);
			}
		}
	}

	/// <summary>随机生成随机的奖励</summary>
	public class SpawnRandomItemRewardsViewCommand : AbstractCommand
	{
		EnemyData _enemyData;
		Vector3 _pos;

		public SpawnRandomItemRewardsViewCommand(EnemyData enemyData, Vector3 pos)
		{
			_enemyData = enemyData;
			_pos = pos;
		}

		protected override void OnExecute()
		{
			for (int i = 0; i < _enemyData.itemCount; i++)
			{
				if (CanSpawn(_enemyData.itemProbability))
				{
					var type = GetItemType(_enemyData.itemRange);
					var view = this.SendCommand(new SpawnARewardCommand(type,_pos));
				}
			}
		}


		#region pri
		private bool CanSpawn(int ratio)
		{
			int index = Random.Range(0, 101);
			return index < ratio;
		}


		private RewardType GetItemType(RewardType[] itemRange)
		{
			if (itemRange.Length == 2)
			{
				int index =  ((int)itemRange[0], (int)itemRange[1] + 1).RR();
				return index.Int2Enum<RewardType>();
			}
			else
			{
				return RewardType.ADD_BULLET;
			}
		}
		#endregion
	}


	public class SpawnBulletEffectViewCommand : AbstractCommand<BulletEffectContainerBase>
	{
		BulletType _bulletType;
		public SpawnBulletEffectViewCommand(BulletType bulletType)
		{
			_bulletType = bulletType;
		}

		protected override BulletEffectContainerBase OnExecute()
		{
			BulletEffectContainerBase effectView = this.GetSystem<IObjectPoolInstanceSystem>().SpawnBulletEffect(_bulletType);
			return effectView;
		}
	}


	/// <summary>根据Creator的生成队伍进度来切换Creator</summary>
	public class SpawnAQueueEnemyBySpawningPrgCommand : AbstractCommand<IEnemyCreator>
	{
		List<IEnemyCreator> _creatorLst;
		IEnemyCreator _curCreator;

		public SpawnAQueueEnemyBySpawningPrgCommand(List<IEnemyCreator> creatorLst, IEnemyCreator curCreator)
		{
			this._creatorLst = creatorLst;
			this._curCreator = curCreator;
		}

		protected override IEnemyCreator OnExecute()
		{
			_curCreator = ChangeCreator(_creatorLst, _curCreator); //写明白方便查看
			Debug.LogFormat($"PlaneCreator索引{_creatorLst.IndexOf(_curCreator)}");
			_curCreator = SpawnNextQueue();


			return _curCreator;
		}



		#region pri

		IEnemyCreator SpawnNextQueue()
		{
			if (_curCreator.IsNotNull())
			{
				if (_curCreator.AwakeSpawnAQueue()) //首次
				{
					_curCreator.SpawnAQueue();
				}
				else if (_curCreator.AwakeSpawnAQueue() == false && _curCreator.SpawningAQueue() == false) //他次
				{
					_curCreator.SpawnAQueue();
				}
				else   //有时上个creator还没生成好一队,这次让它继续生成
				{

				}
			}

			return _curCreator;
		}

		/// <summary>按进度生产下一队的Creator</summary>
		IEnemyCreator ChangeCreator(List<IEnemyCreator> creatorLst, IEnemyCreator curCreator)
		{
			foreach (IEnemyCreator tmp in creatorLst)
			{
				if (curCreator == tmp)//不能自己和自己做对比
				{
					continue;
				}
				bool changeCreator = false;//是否切换当前的Creator

				{  //空或有小的prg都换，写这么长是为了方便打点 
					if (curCreator == null)//初始化状态
					{
						changeCreator = true;//无中换有
					}
					else
					{
						//不能生成一半又被切换点
						//等于也换,均匀点
						if (_curCreator.SpawningAQueue() == false)
						{
							changeCreator = curCreator.GetSpawningPrg() >= tmp.GetSpawningPrg();
						}
					}
				}




				if (changeCreator) //换了
				{
					curCreator = tmp;
					return curCreator;
				}

			}
			return curCreator;//没换
		}

		#endregion
	}
	public class FireCommand : AbstractCommand
	{
		Transform _muzzleTrans;
		BulletPointsCalcEllipseComponent _bulletPointsCalcEllipseComponent;
		IBulletModel _bulletModel;
		float _boundsSizeX;
		string _despawnKey;

		public FireCommand(Transform muzzle, BulletPointsCalcEllipseComponent bulletPointSpawnComponent, IBulletModel bulletModel, float boundsSizeX, string despawnKey)
		{
			_muzzleTrans = muzzle ?? throw new ArgumentNullException(nameof(muzzle));
			_bulletPointsCalcEllipseComponent = bulletPointSpawnComponent ?? throw new ArgumentNullException(nameof(bulletPointSpawnComponent));
			_bulletModel = bulletModel ?? throw new ArgumentNullException(nameof(bulletModel));
			_boundsSizeX = boundsSizeX;
			_despawnKey = despawnKey;
		}

		protected override void OnExecute()
		{

			Vector3[] spawnPosArr;
			Vector3 spawnPos;
			float curSpeed = 0f;
			BulletType bulletType = _bulletModel.E_BulletType;
			//	玩家向上,所以加上;敌人向下,所以减去
			//Vector3 muzzleOffset = _muzzlesTrans.position;

			STest.NotPlayerBullet(bulletType);
			_bulletModel.OnGotBulletSpeed(speed => curSpeed = speed);
			_bulletModel.OnGotPathCalcArr(pathCalc =>
			{
				Vector3 muzzlePos = _muzzleTrans.position;
				EDir muzzleDir = bulletType == BulletType.PLAYER ? EDir.UP : EDir.DOWN;
				spawnPosArr = _bulletPointsCalcEllipseComponent.GetPointArr(pathCalc.Length, muzzlePos, _boundsSizeX, muzzleDir);


				for (int i = 0; i < pathCalc.Length; i++)
				{
					spawnPos = spawnPosArr[i];
					this.SendCommand(new SpawnABulletCommand(spawnPos, curSpeed, _bulletModel.E_BulletType, pathCalc[i], _despawnKey));
				}

			});
		}
	}


	public class SpawnABulletCommand : AbstractCommand
	{
		BulletType _bulletType;
		float _bulletSpeed;
		IBulletModel _bulletModel;
		/// <summary>已经经过子弹路径计算来的初始位置</summary>
		Vector3 _spawnPos;
		IPathCalc _pathCalc;
		string _despawnKey;

		public SpawnABulletCommand(Vector3 initPos, float bulletSpeed, BulletType bulletType, IPathCalc pathCalc, string despawnKey)
		{
			_spawnPos = initPos;
			_bulletType = bulletType;
			_bulletSpeed = bulletSpeed;
			_pathCalc = pathCalc;
			_despawnKey = despawnKey;
		}

		protected override void OnExecute()
		{
			GameObject go = this.GetSystem<IGameObjectPoolSystem>().Spawn(_despawnKey);//默认是Show
			go.transform.position = _spawnPos;
			go.name = _bulletType.Enum2String();

			//
			IBulletCtrl bullet;
			if (_bulletType == BulletType.PLAYER)
			{
				bullet = go.GetOrAddComponent<BulletPlayerCtrl>().Init(_bulletType, _bulletSpeed, _pathCalc); //直接Add防止多个
			}
			else
			{
				bullet = go.GetOrAddComponent<BulletEnemyCtrl>().Init(_bulletType, _bulletSpeed, _pathCalc);
			}
			go.Show();
		}
	}


	/// <summary>生成奖励,排除Star</summary>`    

	public class SpawnARewardCommand : AbstractCommand<PlaneLevelView>
	{
		RewardType e_ItemType;
		Vector3 _pos;

		public SpawnARewardCommand(RewardType e_ItemType , Vector3 pos)
		{
			this.e_ItemType = e_ItemType;
			_pos = pos;
		}

		protected override PlaneLevelView OnExecute()
		{
			GameObject go= GetItem(e_ItemType).gameObject;
			go.name = e_ItemType.Enum2String(); ;
			go.transform.position = _pos;
			return go.GetComponent<PlaneLevelView>();
		}
		public PlaneLevelView GetItem(RewardType type)
		{
			switch (type)
			{
				case RewardType.ADD_BULLET:
					return Spawn<AddBulletItemView>();
				case RewardType.ADD_EXP:
					return Spawn<AddExpItemView>();
				case RewardType.SHIELD:
					return Spawn<ShieldItemView>();
				case RewardType.POWER:
					return Spawn<PowerItemView>();
			}

			return null;
		}

		PlaneLevelView Spawn<T>() where T : PlaneLevelView
		{
			GameObject go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.PREFAB_ITEM_ITEM);
			return go.GetOrAddComponent<T>();
		}
	}

	#endregion

	#region Pool
	public class DespawnABulletCpmmand : AbstractCommand
	{
		GameObject _go;
		string _despawnKey;

		public DespawnABulletCpmmand(GameObject go, string despawnKey)
		{
			_go = go ?? throw new ArgumentNullException(nameof(go));
			_despawnKey = despawnKey ?? throw new ArgumentNullException(nameof(despawnKey));
		}

		protected override void OnExecute()
		{

			this.GetSystem<IGameObjectPoolSystem>().Despawn(_go, _despawnKey);
			_go.Hide();
		}
	}





	public class DeadPlayerCommand : AbstractCommand
	{
		Transform _planeTrans;

		public DeadPlayerCommand(Transform transform)
		{
			this._planeTrans = transform ?? throw new ArgumentNullException(nameof(transform));
		}

		protected override void OnExecute()
		{
			var life = this.GetModel<IAirCombatAppModel>().Life.Value;

			if (life < 0)
			{
				return;
			}

			//标记已经执行过死亡方法

			this.GetModel<IAirCombatAppModel>().Life.Value = 0;
			this.GetModel<IAirCombatAppStateModel>().E_GameState.Value = GameState.END;
			this.GetSystem<IAniSystem>().OnPlaneDestroyAni(new PlaneDestroyAniEvent { pos = _planeTrans.position });
			GameObject.Destroy(_planeTrans.gameObject);
			this.GetSystem<IAudioSystem>().PlaySound(AudioGame.Explode_Plane);
		}

	}


	/// <summary>参数过多，不写</summary>
	public class SpawnAQueueEnemyPlaneCommand : AbstractCommand
	{
		//PathDataMgr _pathDataMgr;
		//EnemyData _enemyData;

		//IPathData pathData;
		//Sprite _sprite;
		//EnemyData _enemyData;
		protected override void OnExecute()
		{
		}

		void SpawnAQueueEnemy()
		{

		}
	}

	public class SpawnAEnemyPlaneCommand : AbstractCommand<PlaneEnemyCtrl>
	{
		private int _posIdxInQueue;
		private EnemyType _enemyType;
		private EnemyData _enemyData;
		private Sprite _sprite;
		private IPathData _pathData;
		private Vector3 _creatorPos;
		private string _goName;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="posIdxInQueue"></param>
		/// <param name="enemyType"></param>
		/// <param name="enemyData"></param>
		/// <param name="sprite"></param>
		/// <param name="pathData"></param>
		/// <param name="creatorPos">是不变的,不然越离谱,所以设置的也是LocalPos</param>
		/// <exception cref="ArgumentNullException"></exception>
		public SpawnAEnemyPlaneCommand(int posIdxInQueue, EnemyType enemyType, EnemyData enemyData, Sprite sprite, IPathData pathData, Vector3 creatorPos, string goName)
		{
			_posIdxInQueue = posIdxInQueue;
			_enemyType = enemyType;
			_enemyData = enemyData ?? throw new ArgumentNullException(nameof(enemyData));
			_sprite = sprite ?? throw new ArgumentNullException(nameof(sprite));
			_pathData = pathData ?? throw new ArgumentNullException(nameof(pathData));
			_creatorPos = creatorPos;
			_goName = goName;
		}

		protected override PlaneEnemyCtrl OnExecute()
		{
			string key = ResourcesPath.PREFAB_PLANE;
			GameObject go = this.GetSystem<IGameObjectPoolSystem>().Spawn(key);
			Transform parent = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.GameRoot_PLANE);
			//需要传入creator的位置 ,_posIdxInQueue + _pathData只是局部坐标
			go.SetParent(parent);
			go.name = _goName;
			PlaneEnemyCtrl enemy = go.GetOrAddComponent<PlaneEnemyCtrl>().Init(_posIdxInQueue, _enemyType, _enemyData, _sprite, _pathData, key, _creatorPos);

			//GameProgressSystem会通过NormalEvent驱动CreatorMgr，再次进入循环
			// go.Show(); //一show就跑,所以交给其它控制
			return enemy;

		}
	}




	public class DeadEnemyCommand : AbstractCommand
	{
		Transform _planeTrans;
		EnemyType _enemyType;
		EnemyData _enemyData;

		public DeadEnemyCommand(Transform transform, EnemyType enemyType, EnemyData enemyData)
		{
			_planeTrans = transform ?? throw new ArgumentNullException(nameof(transform));
			_enemyType = enemyType;
			_enemyData = enemyData ?? throw new ArgumentNullException(nameof(enemyData));
		}

		protected override void OnExecute()
		{
			this.GetSystem<IAniSystem>().OnPlaneDestroyAni(new PlaneDestroyAniEvent { pos = _planeTrans.position });
			this.GetModel<IAirCombatAppModel>().Score.Value += _enemyData.score;
			this.GetSystem<IAudioSystem>().PlaySound(AudioGame.Explode_Plane);
			//
			if (_enemyType == EnemyType.BOSS)
			{
				this.GetModel<IAirCombatAppStateModel>().E_LevelState.Value = LevelState.END;// MsgEvent.EVENT_LEVEL_END由IAirCombatAppStateModel操作
			}
			//
			this.SendCommand(new SpawnStarViewsCommand(_enemyData.starNum, _planeTrans.position));
			this.SendCommand(new SpawnRandomItemRewardsViewCommand(_enemyData, _planeTrans.position));
			_planeTrans.Hide(); //不Hide移动组件在跑会导致初始位置
			this.GetSystem<IGameObjectPoolSystem>().Despawn(_planeTrans.gameObject, ResourcesPath.PREFAB_PLANE);
		}
	}
	#endregion

	#region  Instantiate


	public class InstantiateLeveItemClickablesCommand : AbstractCommand
	{
		RectTransform _levelsTrans;


		public InstantiateLeveItemClickablesCommand(RectTransform levelsTrans)
		{
			_levelsTrans = levelsTrans;
		}

		protected override void OnExecute()
		{

			#region Level
			/*
			{
			  "levelCount": 24,
			  "eachRow": 4
			}             
			*/
			#endregion
			//原本用到的脚本
			{

				LevelsView levelsView;
				LevelsController levelsController;
				LevelRoot levelRoot;
				LevelRootController levelRootController;
				LevelItem levelItem;
				global::LevelItemController levelItemController;
			}


			//数量和行列
			{

				IReader reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_LEVEL_CONFIG);
				// int levelCount=  reader[LevelRoot.Name.levelCount.Enum2String()].GetOut<>;

			}
			float leftPadding = 40f;//整体与左Padding的大小
			int levelCount = 24;
			int eachRow = 4; //一行多少个
							 //实例
			{
				//GameObject go = this.GetUtility<ILoadUtil>().LoadPrefabAndInstantiate(Paths.PREFAB_LEVEL_ITEM, _levelsTrans);
				GameObject prefab = this.GetSystem<IGameObjectSystem>().Load(ResourcesPath.PREFAB_LEVEL_ITEM, LoadType.RESOURCES);

				for (int i = 0; i < levelCount; i++)
				{
					GameObject levelItem = GameObject.Instantiate(prefab, _levelsTrans);
					levelItem.Rect().localScale = new Vector2(0.35f, 0.35f);
					//
				}
			}
			//排列
			{
				for (int idx = 0; idx < levelCount; idx++)
				{
					Transform levelSelectable = _levelsTrans.GetChild(idx);
					SetPos(levelSelectable, GetGrid(idx, eachRow), leftPadding);
				}

			}
			// TODO 组件
			{
				for (int idx = 0; idx < levelCount; idx++)
				{
					Transform levelSelectable = _levelsTrans.GetChild(idx);
					levelSelectable.GetOrAddComponent<LevelItemClickable>().Init();
				}
			}
			this.SendEvent<UpdateLevelSelectableLockStateEvent>();
		}


		#region 辅助 设置位置
		/// <summary>行，6行4列</summary>
		private Vector2 GetGrid(int id, int eachRow)
		{
			var y = id / eachRow; //id/4第几行 
			var x = id % eachRow; //id%4第几个
			return new Vector2(x, y);
		}

		private void SetPos(Transform t, Vector2 grid,float leftPadding)
		{
	 
			// (0,0)(0,1)(0,2)(0,3)
			int _leftOffet = 50; //上下行左右错开的值 ,传说中的错落感
			int _lineSpacing = 20;
			int _offset = 10;
			//
			var width = t.Rect().rect.width * t.localScale.x;
			var height = t.Rect().rect.height * t.localScale.y;
			//
			var indention = grid.y % 2 == 0 ? _leftOffet : 0;  //(凹痕,锯齿)一行不加offset，一行额外加offset
			//
			var x = indention + width * 0.5f + (_offset + width) * grid.x;
			var y = height * 0.5f + (_lineSpacing + height) * grid.y;
			t.Rect().anchoredPosition = new Vector2(leftPadding + x, -y);
		}

		#endregion
	}


	/// <summary>没必要用Pool</summary>
	public class InstantiateLevelItemClickableCommand : AbstractCommand
	{
		Transform _trans;

		public InstantiateLevelItemClickableCommand(Transform trans)
		{
			_trans = trans;
		}

		protected override void OnExecute()
		{
			int cur = _trans.GetSiblingIndex();
			Button maskBtn = _trans.GetButtonDeep(GameObjectName.MaskBtn);
			Button enterBtn = _trans.GetButtonDeep(GameObjectName.EnterBtn);
			int index = _trans.GetSiblingIndex();
			_trans.gameObject.name = GameObjectName.LevelClickable + CharMark.UnderScore + (cur);
			_trans.FindChildDeep(GameObjectName.Text).SetText(cur + 1);//现实的话不从0开始
			{
				//Enter 
				//Enter Text
				//Mask
				enterBtn.onClick.AddListenerAfterRemoveAll(() =>
				{
					Debug.Log("LevelSelectable 进入关卡" + cur);
					this.GetSystem<IUISystem>().ClosePopPanel();
                    this.SendCommand(new OpenLoadingPanelCommand(cur));
				});
				maskBtn.onClick.AddListenerAfterRemoveAll(() =>
				{
					Debug.Log("LevelSelectable 当前关卡未开放");
					this.SendCommand(new OpenDialogPanelCommand("当前关卡未开放"));
				});
			}
		}
	}
	#endregion


	#region Show

	public class OpenDialogPanelCommand : AbstractCommand<DialogPanel>
	{
		string _content;
		Action _trueAction;
		Action _falseAction;
		public OpenDialogPanelCommand(string content)
		{
			_content = content;
		}

		public OpenDialogPanelCommand(string content, Action trueAction, Action falseAction)
		{
			_content = content;
			_trueAction = trueAction;
			_falseAction = falseAction;
		}

		protected override DialogPanel OnExecute()
		{
			RectTransform rect = this.GetSystem<IUISystem>().PopPanel(ResourcesPath.PREFAB_DIALOG_PANEL);
			// 参数太多，这种就不自动完成
			DialogPanel dialogView = rect.gameObject.GetOrAddComponent<DialogPanel>().InitParas(_content, _trueAction, _falseAction);

			return dialogView;
		}
	}

	#endregion  


	public class AutoDespawnOtherCollideCameraBorderCommand : AbstractCommand
	{
		/// <summary>回收到pool的key，这里用path</summary>
		string _poolKey;
		/// <summary>因为一般以图片消失视觉最近人</summary>
		SpriteRenderer _sr;
		/// <summary>需要despawn的物体</summary>
		Transform _despawnTrans;
		/// <summary>有的物体的方向不需要进行自动销毁. 一般敌人忽略顶部,Bosss略全部</summary>
		EDir[] _excludeDirs;
		EDir _dir;

		public AutoDespawnOtherCollideCameraBorderCommand(string path, SpriteRenderer sr, Transform despawnTrans, EDir[] excludeDirs = null)
		{
			_poolKey = path;
			_sr = sr;
			_despawnTrans = despawnTrans;
			_excludeDirs = excludeDirs;
		}

		protected override void OnExecute()
		{

			if (JudgeBeyondBorder())
			{
				//this.GetSystem<IGameObjectPoolSystem>().DespawnWhileKeyIsName(_despawnTrans.gameObject, _despawnKey)  ;
				this.GetSystem<IGameObjectPoolSystem>().Despawn(_despawnTrans.gameObject, _poolKey);
			}
		}



		#region pri

		private bool JudgeBeyondBorder()
		{
			if (_sr.IsNull())
			{
				return true;

			}
			//判断底边界限
			//因为都是运动的,有偏差
			if (_excludeDirs != null)
			{
				if (!_excludeDirs.Contains(EDir.TOP)) if (OutTop()) { _dir = EDir.TOP; return true; }  //这条没有
				if (!_excludeDirs.Contains(EDir.BOTTOM)) if (OutBottom()) { _dir = EDir.BOTTOM; return true; }
				if (!_excludeDirs.Contains(EDir.LEFT)) if (OutLeft()) { _dir = EDir.LEFT; return true; }
				if (!_excludeDirs.Contains(EDir.RIGHT)) if (OutRight()) { _dir = EDir.RIGHT; return true; }
			}
			else  //一碰就死
			{
				if (OutTop()) { _dir = EDir.TOP; return true; }  //这条没有
				if (OutBottom()) { _dir = EDir.BOTTOM; return true; }
				if (OutLeft()) { _dir = EDir.LEFT; return true; }
				if (OutRight()) { _dir = EDir.RIGHT; return true; }
			}



			return false;
		}


		bool OutLeft()
		{
			float x1 = _sr.BoundsMinX();
			float x2 = this.GetUtility<IGameUtil>().CameraMinPoint().x- _sr.BoundsSize().x;     //左右要多一个身位,玩家看不到再销毁
            if (x1 < x2)
			{
				//Debug.Log(x1 + "," + x2);
				return true;
			}
			return false;

		}

		bool OutRight()
		{
			float x1 = _sr.BoundsMaxX();
			float x2 = this.GetUtility<IGameUtil>().CameraMaxPoint().x+_sr.BoundsSize().x;	//左右要多一个身位,玩家看不到再销毁
			if (x1 > x2)
			{
				//Debug.Log(x1 + "," + x2);
				return true;
			}
			return false;

		}
		bool OutTop()
		{
			float y1 = _sr.BoundsMinY();
			float y2 = this.GetUtility<IGameUtil>().CameraMaxPoint().y;
			if (y1 > y2)
			{
				//Debug.Log(y1+","+y2);
				return true;
			}
			return false;

		}


		bool OutBottom()
		{
			float y1 = _sr.BoundsMaxY();
			float y2 = this.GetUtility<IGameUtil>().CameraMinPoint().y;
			if (y1 < y2)
			{
				//Debug.Log(y1 + "," + y2);
				return true;
			}
			return false;

		}

		#endregion
	}



	public class ExitGameCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			Application.Quit();
			"退出游戏".LogInfo();
		}
	}


	/// <summary>
	/// 声音放在子弹前面合适点
	/// </summary>   
	public class PlaySoundFireCommand : AbstractCommand
	{
		private int _fireSoundPara;  //子弹容量内音效次数
		private int _bulletCnting;   //子弹存量
		private int _bulletCnt;   //子弹存量

		public PlaySoundFireCommand(int bulletCnting, int bulletCnt, int fireSoundPara)
		{
			_fireSoundPara = fireSoundPara;
			_bulletCnting = bulletCnting;
			_bulletCnt = bulletCnt;
		}

		protected override void OnExecute()
		{
			this.GetSystem<IAudioSystem>().PlaySound(ResourcesPath.AUDIO_FIRE);
			return;
			if (_bulletCnting == 0 || _fireSoundPara == 0) return;
			if (_fireSoundPara > _bulletCnt) return; //没必要一轮过去都不响
			if (_bulletCnting % _fireSoundPara == 0)  //声音次数多太吵了
			{
				this.GetSystem<IAudioSystem>().PlaySound(ResourcesPath.AUDIO_FIRE);
			}
		}
	}

	public class ModelPlanelLevelTnpCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			throw new NotImplementedException();
		}
	}
	public class GetPlaneSpriteCommand : AbstractCommand<Sprite>
	{

		protected override Sprite OnExecute()
		{
			var planeId = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			var planeLevel = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel;
			Sprite planeSprite = NSPlaneSpritesModel.Single[planeId, planeLevel];
			return planeSprite;
		}
	}


	/// <summary>挂在Bullet的CollideMsg</summary>
	public class CollideMsgFromBulletCommand : AbstractCommand
	{

		private IDespawnCase _selfDestroyCase;
		private IBullet _selfBullet;
		Transform _other;

		public CollideMsgFromBulletCommand(IDespawnCase destroyCase, IBullet selfBullet, Transform other)
		{
			_selfDestroyCase = destroyCase;
			_selfBullet = selfBullet;
			_other = other;
		}

		protected override void OnExecute()
		{
			IBullet otherBullet = _other.GetComponentInChildren<IBullet>(); //分开节点方便看,这个不是子弹是飞机上的子弹信息节点
			if (_selfBullet == null || otherBullet==null)
			{
				return;
			}
			if (_other.gameObject.CompareTag(Tags.BULLET))	//子弹不能打子弹
			{
				return;
			}
			if (_selfBullet.Owner == otherBullet.Owner)  // 01自己的子弹不能打自己 02初始位置时就触发了自己
			{
				return;
			}



			//只要是飞机类(),都会受子弹攻击
			if (otherBullet != null   //敌人或者玩家等可以碰撞的
				&& otherBullet.ContainsDamageTo(_selfBullet.Owner)) //伤了
			{
				_selfDestroyCase.DoIfNotNull(() =>
				{
					_selfDestroyCase.Injure(-otherBullet.GetAttack()); //扣血,所以减
				});
			}
			else if (_selfBullet.ContainsDeadFrom(_other.tag))  //死了
			{
				_selfDestroyCase.DoIfNotNull(() =>
				{
					_selfDestroyCase.Dead();
				});
			}
		}
	}



	/// <summary>挂在Plane(敌人,玩家)的CollideMsg</summary>
	public class CollideMsgFromPlaneCommand : AbstractCommand
	{
		private IDespawnCase _destroyCase;
		private IBullet _selfBullet;
		//挂Tag的几节点
		Transform _other;
		Transform _self;
		InvincibleComponent _invincibleComponent;

		public CollideMsgFromPlaneCommand(IDespawnCase destroyCase, IBullet selfBullet, Transform self, Transform other, InvincibleComponent invincibleComponent=null)
		{
			_destroyCase = destroyCase ?? throw new ArgumentNullException(nameof(destroyCase));
			_selfBullet = selfBullet ?? throw new ArgumentNullException(nameof(selfBullet));
			_other = other ?? throw new ArgumentNullException(nameof(other));
			_self = other ?? throw new ArgumentNullException(nameof(self));
			_invincibleComponent = invincibleComponent ;
		}

		protected override void OnExecute()
		{
			IBullet otherBullet = _other.GetComponentInChildren<BulletModelComponent>(); //
			if (_other.tag == Tags.BULLET && otherBullet != null && _selfBullet != null
				&& otherBullet.ContainsDamageTo(_selfBullet.Owner)) //BulletEnemyCtrl//受伤
			{
				_destroyCase.DoIfNotNull(() =>
				{
					if (_self.CompareTag(Tags.PLAYER)) STest.PlayerCollidedByBulletCnt++;
					// STest.Injure(_destroyCase, otherBullet, _other, _invincibleComponent);//也就是以下
					_destroyCase.DoIfNotNull(() =>
					{
						if (_invincibleComponent != null)
						{
							//_destroyCase.Injure(-otherBullet.GetAttack());
							_destroyCase.Injure(-1); //太菜了
							_invincibleComponent.Invincible = true;
						}
						else
						{
							_destroyCase.Injure(-otherBullet.GetAttack());

						}
					});
					return;
					_destroyCase.Injure(-otherBullet.GetAttack());
				});
			}
			else if (_selfBullet != null && _selfBullet.ContainsDeadFrom(_other.tag)) //死亡
			{
				if (_self.CompareTag(Tags.PLAYER)) STest.PlayerCollidedByPlaneCnt++;
				STest.IsBossPlane(_other.GetComponentInChildren<BulletModelComponent>()); //);
				bool invincible = this.GetModel<IAirCombatAppStateModel>().Invincible;
				_destroyCase.DoIfNotNull(() =>
				{
					if (_other.CompareTag(Tags.ENEMY))//other是敌人,所以这是玩家
					{
						//_destroyCase.Injure(-otherBullet.GetAttack());石
						_destroyCase.Injure(-1);
						this.GetModel<IAirCombatAppStateModel>().Invincible.Value = true;


					}
					else if (_other.CompareTag(Tags.PLAYER))//敌人
					{
						if (invincible)//玩家无敌闪避,撞不死敌人
						{
							return;
						}
						_destroyCase.Dead();//敌人直接死

					}
				});
				return;//测试太菜了,不能直接死
				_destroyCase.DoIfNotNull(() =>
				{
					_destroyCase.Dead();
				});
			}
			else if (_other.tag == Tags.ITEM)   //Buff debuff
			{
				this.SendCommand(new CollideItemCommand(_other));
			}
		}
	}

	/// <summary>处理逻辑,特效等在ViwBase中处理</summary>
	public class CollideItemCommand : AbstractCommand
	{
		Transform _self;

		public CollideItemCommand(Transform self)
		{
			_self = self ?? throw new ArgumentNullException(nameof(self));
		}

		protected override void OnExecute()
		{
			var buff = _self.GetComponent<IBuffCarrier>();
			var debuff = _self.GetComponent<IDebufferCarrier>();
			if (buff != null)
			{
				this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_BUFF, buff.Type);
			}
			if (debuff != null)
			{
				this.GetSystem<IMessageSystem>().SendMsg(MsgEvent.EVENT_DEBUFF, debuff.Type);
			}
		}
	}

	public class MoveCommand : AbstractCommand
	{
		Transform _t;
		Vector2 _dir;
		float _speed;
		SpriteRenderer _sr;


		public MoveCommand(Transform t, Vector2 dir, float speed)
		{
			_t = t;
			_dir = dir;
			_speed = speed;
		}
		public MoveCommand(Transform t, Vector2 dir, float speed, SpriteRenderer sr)
		{
			_t = t;
			_dir = dir;
			_speed = speed;
			_sr = sr;
		}

		public MoveCommand(Transform t, Vector2 dir, double speed)
		{
			_t = t;
			_dir = dir;
			_speed = (float)speed;
		}


		protected override void OnExecute()
		{
			if (this.SendCommand(new InCameraBorderCommand(_dir, _sr)))
			{
				_t.Translate(_dir * _speed * Time.deltaTime, Space.World);

			}
		}
	}

	public class MovingCommand : AbstractCommand<bool>
	{
		Transform _t;
		Vector2 _dir;
		float _speed;
		SpriteRenderer _sr;


		public MovingCommand(Transform t, Vector2 dir, float speed)
		{
			_t = t;
			_dir = dir;
			_speed = speed;
		}

		public MovingCommand(Transform t, Vector2 dir, float speed, SpriteRenderer sr)
		{
			_t = t;
			_dir = dir;
			_speed = speed;
			_sr = sr;
		}
		public MovingCommand(Transform t, Vector2 dir, double speed)
		{
			_t = t;
			_dir = dir;
			_speed = (float)speed;
		}


		protected override bool OnExecute()
		{
			if (this.SendCommand(new InCameraBorderCommand(_dir, _sr)))
			{
				_t.Translate(_dir * _speed * Time.deltaTime, Space.World);
				return true;

			}
			else
			{
				return false;
			}
		}
	}






	public class CameraMoveUpCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			this.SendEvent(new CameraMoveUpEvent());
		}
	}

	public class MoveUpCommand : AbstractCommand
	{
		Transform _t;
		float _speed;


		public MoveUpCommand(Transform t, float speed)
		{
			_t = t;
			_speed = speed;
		}


		protected override void OnExecute()
		{
			_t.Translate(Vector2.up * _speed * Time.deltaTime, Space.World);
		}
	}

	public class MoveDownCommand : AbstractCommand
	{
		Transform _t;
		float _speed;


		public MoveDownCommand(Transform t, float speed)
		{
			_t = t;
			_speed = speed;
		}


		protected override void OnExecute()
		{
			_t.Translate(Vector2.down * _speed * Time.deltaTime, Space.World);
		}
	}





	public class InitCameraSpeedCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			Debug.Log("InitCameraSpeedCommand ");
			this.SendEvent(new InitCameraSpeedEvent());
		}
	}
	public class PlayerCanControlCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			Debug.Log("PlayerCanControlCommand ");
			this.SendEvent(new PlayerCanControlEvent());
		}
	}

	public class UseShieldCommand : AbstractCommand
	{
		protected override void OnExecute()
		{
			if (this.GetModel<IAirCombatAppModel>().ShieldCount > 0)
			{
				this.GetModel<IAirCombatAppModel>().ShieldCount.Value--;
				this.SendEvent(new UseShieldEvent() );
			}
		}
	}
   public struct UseShieldEvent{}

	/// <summary>监听Player</summary>
	public struct InitPlayerEvent{}

	#endregion


	#region Query


	#region 飞机强化界面
	public class SelectedPlaneSpriteQuery : QFramework.AbstractQuery<Sprite>, ICanGetUtility
	{
		protected override Sprite OnDo()
		{
			int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			int level = this.SendQuery(new SelectedPlaneLevelQuery());
			Sprite planeSprite = NSPlaneSpritesModel.Single[planeID, level];
			return planeSprite;
		}
	}
	public class SelectedPlaneLevelQuery : QFramework.AbstractQuery<int>, ICanGetUtility
	{
		protected override int OnDo()
		{
			int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
			string levelKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + DataKeys.LEVEL);
			int levelValue = this.GetUtility<IStorageUtil>().Get<int>(levelKey);
			return levelValue;
		}
	}
	#endregion

	public class CurLevelDataQuery : QFramework.AbstractQuery<LevelData>, ICanGetUtility
	{
		EnemyLevelData _enemyLevelData;

		public CurLevelDataQuery(EnemyLevelData nemyLevelData)
		{
			_enemyLevelData = nemyLevelData ?? throw new ArgumentNullException(nameof(nemyLevelData));
		}

		protected override LevelData OnDo()
		{
			int curLevel = this.GetModel<IAirCombatAppStateModel>().CurLevel;
			LevelData curLevelData = new LevelData(); ;
			if (curLevel > _enemyLevelData.LevelDatas.Length - 1)
			{
				SBug.IndexOutside("关卡数量(2)优先,未配置配置");
			}
			else
			{ 
				  curLevelData = _enemyLevelData.LevelDatas[curLevel];
			}

			return curLevelData;
		}
	}


	public class CameraSpeedQuery : QFramework.AbstractQuery<float>, ICanGetUtility
	{
		float _speed;
		protected override float OnDo()
		{
			var reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_GAME_CONFIG);
			reader[ReaderKey.cameraSpeed].Get<float>(cameraSpeed =>
			{
				_speed = cameraSpeed;
			});
			return _speed;
		}
	}

	#endregion


	#region Utility


	#region 接口

	public interface IContainsKey
	{
		bool ContainsKey(string key);
	}

	public interface IClear
	{
		void Clear();
	}
	public interface IClearByKey
	{
		void Clear(string key);
	}

	public interface IClearAll
	{
		void ClearAll();
	}

	public interface IGet
	{
		T Get<T>(T key);
	}
	public interface ISet
	{
		void Set<T>(T key, T value);
	}

	public interface IGetBool
	{
		bool GetBool<T>(T key);
	}


	public interface ISetBool
	{
		void SetBool<T>(T key, bool value);
	}
	public interface ISetInt
	{
		void SetInt<T>(T key, int value);
	}


	/// <summary>Key值，Value值，两个Equals</summary>
	public interface ISetIntKEV
	{
		void SetInt(int value);
	}
	public interface ISetJsonData
	{
		void SetJsonData<T>(T key, JsonData value);
	}

	#endregion




	#region IStorage


	public interface IStorageUtil : IUtility, ISetJsonData
	{
		//老版的定义
		//void SaveInt(string propertyKey, int propertyValue);
		//int LoadInt(string propertyKey, int defaultValue = 0);
		//void SaveString(string propertyKey, string propertyValue);
		//string LoadString(string propertyKey, string defaultValue = "");
		//void SaveFloat(string propertyKey, float propertyValue);
		//float LoadFloat(string propertyKey, float defaultValue = 0.0f);


		T Get<T>(string key);
		void Set<T>(string key, T value);
		object GetObject(string key);
		void SetObject(string key, object value);
		void Clear(string key);
		void ClearAll();
		bool ContainsKey(string key);
	}


	public class StorageUitl : IStorageUtil
	{
		#region 字属
		private static readonly Dictionary<Type, object> _defaultValueDic = new Dictionary<Type, object>
		{
			{typeof(int), default(int)},
			{typeof(string), ""},
			{typeof(float), default(float)}
		};
		//这两个原来是readonly，但我需要在OnInit中体现出初始赋值的过程（有可能别的初始读取方式），所以改了static
		// 但是因为Util（System不能在Model中使用，不用Systm就没有Init重写），所以又改回来
		private readonly Dictionary<Type, Func<string, object>> _dataGetterDic = new Dictionary<Type, Func<string, object>>
			{
				{typeof(int), key => PlayerPrefs.GetInt(key, (int) _defaultValueDic[typeof(int)])},
				{typeof(string), key => PlayerPrefs.GetString(key, (string) _defaultValueDic[typeof(string)])},
				{typeof(float), key => PlayerPrefs.GetFloat(key, (float) _defaultValueDic[typeof(float)])}
			};
		private readonly Dictionary<Type, Action<string, object>> _dataSetterDic = new Dictionary<Type, Action<string, object>>
			{
				{typeof(int), (key, value) => PlayerPrefs.SetInt(key, (int) value)},
				{typeof(string), (key, value) => PlayerPrefs.SetString(key, (string) value)},
				{typeof(float), (key, value) => PlayerPrefs.SetFloat(key, (float) value)}
			};

		private string _className = "PlayerPrefsMemory";



		PlayerPrefsStorage _pp = new PlayerPrefsStorage();
		//JsonStorage _json; //采用接口的方式



		#endregion


		#region 实现

		public T Get<T>(string key) //0level
		{
			var type = typeof(T);
			var td = TypeDescriptor.GetConverter(type);

			if (_dataGetterDic.ContainsKey(type))
			{
				//根据字符串找类型
				T res=    (T)td.ConvertTo(_dataGetterDic[type](key), type);//0 .拆开看方便调试
				return res;
			}

			Debug.LogError(_className + "中无此类型数据，类型名：" + typeof(T).Name);
			return default(T);
		}


		public object GetObject(string key)
		{
			if (ContainsKey(key))
			{
				foreach (var pair in _dataGetterDic)
				{
					var value = pair.Value(key);
					if (!value.Equals(_defaultValueDic[pair.Key]))
					{
						return value;
					}
				}
			}
			else
			{
				//Debug.Log(_className + "内不包含对于键值（所以改数据会转Json）：" + propertyKey);
			}


			return null;
		}
		public void Set<T>(string key, T value)
		{
			var type = typeof(T);

			if (_dataSetterDic.ContainsKey(type))
				_dataSetterDic[type](key, value); //0level,0
			else
				Debug.LogError(_className + "中无此类型数据，数据为 key:" + key + " value:" + value);
		}


		public void SetObject(string key, object value)
		{
			var success = false;
			foreach (var pair in _dataSetterDic)
			{
				if (value.GetType() == pair.Key)
				{
					pair.Value(key, value);
					success = true;
				}
			}

			if (!success)
			{
				Debug.LogError(_className + "未找到当前值的类型，赋值失败，value：" + value);
			}
		}


		public void Clear(string key)
		{
			_pp.Clear(key);
			// _json.Clear(propertyKey);
		}

		public void ClearAll()
		{
			_pp.ClearAll();
			//_json.ClearAll();
		}

		public bool ContainsKey(string key)
		{
			return _pp.ContainsKey(key);//|| _json.ContainsKey(propertyKey);
		}
		#endregion



		#region 实现 ISetJsonData
		public void SetJsonData<T>(T key, JsonData data)
		{

			#region 说明
			/**
			{
				"planes": [
				  {
					"planeId": 0,
					"levelValue": 0,
					"attackTime":1,
					"attack": { "name":"攻击","propertyValue":5,"costValue":200,"costUnit":"star","grouth":10,"maxVaue": 500},
					"fireRate": { "name":"攻速","propertyValue":80,"costValue":200,"costUnit":"star","grouth":1,"maxVaue": 100},
					"life": { "name":"生命","propertyValue":100,"costValue":200,"costUnit":"star","grouth":50,"maxVaue": 1000},
					"upgrades": { "name":"升级","coefficient": 2,"max":4,"0": 100,"1": 200,"2": 300,"3": 400,"costUnit":"diamond"}
						}, ......
				],
				"planeSpeed": 1.2
				} 
			**/
			#endregion
			//propertyKey=0level,0attackTime(0就是planeId)
			IJsonWrapper jsonWrapper = data;
			switch (data.GetJsonType())
			{
				case JsonType.None:
					Debug.Log("当前jsondata数据为空");
					break;
				case JsonType.Object:
					SetObjectData(key, data);
					break;
				case JsonType.String:
					Set(key.ToString(), jsonWrapper.GetString());
					break;
				case JsonType.Int:
					Set(key.ToString(), jsonWrapper.GetInt()); //0level ,0
					break;
				case JsonType.Long:
					Set(key.ToString(), (int)jsonWrapper.GetLong());
					break;
				case JsonType.Double:
					Set(key.ToString(), (float)jsonWrapper.GetDouble());
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}


		private void SetObjectData<T>(T oldkey, JsonData data)
		{
			foreach (var key in data.Keys)
			{
				var newKey = oldkey + key;
				if (!ContainsKey(newKey))
				{
					SetJsonData(newKey, data[key]);
				}
			}
		}
		#endregion  

	}
	public class PlayerPrefsStorage : IClearByKey, IClearAll, IContainsKey
	{

		#region 实现



		public void Clear(string key)
		{
			PlayerPrefs.DeleteKey(key);
		}

		public void ClearAll()
		{
			PlayerPrefs.DeleteAll();
		}

		public bool ContainsKey(string key)
		{
			bool has = PlayerPrefs.HasKey(key);
			return has;
		}
		#endregion
	}

	#endregion





	#endregion


	#region Event
	/// <summary>调试Boss用的</summary>
	public struct AwakeBossCreatorEvent
	{ 
	
	}
	public struct EnterGameSceneEvent
	{

	}
	public struct BulletDestroyAniEvent
	{
		public Vector3 pos;
	}
	public struct PlaneDestroyAniEvent

	{
		public Vector3 pos;
	}

	/// <summary>
	/// 给玩家飞机以相机的移动同步
	///  <br/>到StateModel读取，这里不保存Speed
	/// </summary>
	public struct InitCameraSpeedEvent
	{

	}
	public struct PlayerArriveEvent
	{

	}

	/// <summary>进场动画完成后控制</summary>
	public struct PlayerCanControlEvent
	{

	}
	public struct CameraMoveUpEvent
	{

	}

	/// <summary>这块是时常变动的</summary>

	public class UpdateLevelSelectableLockStateEvent
	{

	}
	public class CloseSelectLevelPanelEvent
	{

	}

	public class UpgradesPlaneLevelEvent
	{

	}
	public class IncreasePlanePropertyEvent
	{

	}

	public class ChangeStarEvent
	{

	}
	public class ChangeDiamondEvent
	{

	}

	public class SelectHeroEvent
	{

	}

	public class SelectPlaneIDEvent
	{
																	   
	}
	public struct InitGameObjectPoolSystemEvent { }
	public struct AirCombatAppLoadedEvent { }
	#endregion


	#region Architecture
	public class AirCombatApp : Architecture<AirCombatApp>
	{
		protected override void Init()//01 02分类型没设呢么用,依赖顺序导致不能System,Model等一类放在一类
		{
		   
			{
				NSOrderSystem.Single.Init();
			   this.RegisterUtility<IBulletModelUtil>(new BulletModelUtil());
			   this.RegisterUtility<IBindPrefabUtil>(new BindPrefabUtil());
			   new StCustomAttributes().Init();

			}

			{// 01 注册 Model 
				this.RegisterSystem<IDontDestroyOnLoadSystem>(new DontDestroyOnLoadSystem());           //01
				this.RegisterModel<IAirCombatAppModel>(new AirCombatAppModel());
				this.RegisterModel<IAirCombatAppStateModel>(new AirCombatAppStateModel());//02
				this.RegisterModel<IAirCombatAppPlaneSpriteModel>(new AirCombatAppPlaneSpriteModel());
			}
			{ //02 System
				this.RegisterSystem<IMessageSystem>(new MessageSystem());                               //01
				{                                                                                       //顺序严格
					this.RegisterSystem<ICoroutineSystem>(new CoroutineSystem());                       //00
					this.RegisterSystem<IDelayDetalCoroutineSystem>(new DelayDetalCoroutineSystem());   //00
					this.RegisterSystem<ILifeCycleSystem>(new LifeCycleSystem());                       //02
					this.RegisterSystem<IAudioSystem>(new AudioSystem());                               //02
					this.RegisterSystem<IConfigSystem>(new ConfigSystem());                             //02
					this.RegisterSystem<IJsonSystem>(new JsonSystem());    
					this.RegisterSystem<IObjectPoolInstanceSystem>(new ObjectPoolInstanceSystem());             //03 
					this.RegisterSystem<IGameObjectSystem>(new GameObjectSystem());           //04
					this.RegisterSystem<IGameObjectPoolSystem>(new GameObjectPoolSystem());             //03 
					this.RegisterSystem<ISpriteSystem>(new SpriteSystem());             //03 

				}
			}

			{// 注册通用方法。Util不用初始化，直接扔前面
				this.RegisterUtility<ILoadUtil>(new LoadUtil()); //01
				NSPlaneSpritesModel.Single.Init();
				GameDataMgr.Single.Init();
				this.RegisterUtility<IReaderUtil>(new ReaderUtil());//02
				this.RegisterUtility<IBattleUtil>(new BattleUtil());
				this.RegisterUtility<IKeysUtil>(new KeysUtil());
				this.RegisterUtility<IStorageUtil>(new StorageUitl());
				this.RegisterUtility<IGameUtil>(new GameUtil());
				this.RegisterUtility<IButtonUtil>(new ButtonUtil());

			}

			{
				//LifeCycleAddConfig
				this.RegisterSystem<ISceneSystem>(new SceneSystem());
				this.RegisterSystem<ISceneConfigSystem>(new SceneConfigSystem());
				this.RegisterSystem<IUILevelSystem>(new UILevelSystem());
				this.RegisterSystem<IUISystem>(new UISystem());
				//this.RegisterSystem<IGameProcessSystem>(new GameProcessSystem());            
			}

			///////////////////////////////////
			{//stateModel 
			 // 注册 System 
			 //this.RegisterSystem<IAchievementSystem>(new AchievementSystem());
			 // this.RegisterEvent()
				this.RegisterSystem<IAniSystem>(new AniSystem());
				this.RegisterSystem<IGameLayerSystem>(new EntyityLayerSystem());
				this.RegisterSystem<ILoadCreatorDataSystem>(new LoadCreatorDataSystem()); //01
				this.RegisterSystem<IGameProcessSystem>(new GameProcessSystem());  //02
				this.RegisterUtility<IStorageUtil>(new StorageUitl()); // 注册存储工具的对象
			}
				this.SendEvent<AirCombatAppLoadedEvent>();

		}


		protected override void ExecuteCommand(ICommand command)
		{
			//Debug.Log("Before " + command.GetType().Name + "Execute");
			base.ExecuteCommand(command);
			//Debug.Log("After " + command.GetType().Name + "Execute");
		}

		protected override TResult ExecuteCommand<TResult>(ICommand<TResult> command)
		{
			//Debug.Log("Before " + command.GetType().Name + "Execute");
			var result = base.ExecuteCommand(command);
			//Debug.Log("After " + command.GetType().Name + "Execute");

			return result;

		}
	}


	/// <summary>战斗中</summary> 
	public class AirCombatAppGame : Architecture<AirCombatAppGame>
	{
		protected override TResult ExecuteCommand<TResult>(ICommand<TResult> command)
		{
			return base.ExecuteCommand(command);
		}

		protected override void ExecuteCommand(ICommand command)
		{
			base.ExecuteCommand(command);
		}

		protected override void Init()
		{

		}
	}


	#endregion


	#region SC静态类

	public static class SCGameObjectPoolKey 
	{

		public static string GetPoolName(string prefabName)
		{
			string poolName;
			poolName = prefabName.Replace(Affixes.CloneWithBracket, Affixes.NullString);
			poolName = poolName + GameObjectName.Pool;

			return poolName;
		}

	}

	public static class SCBulletTrajectoryData
	{

		public static IPathCalc[] GetStraightPathCalcArr(int[] pointAngleArr)
		{
			IPathCalc[] pathCalcArr = new IPathCalc[pointAngleArr.Length];

			for (int i = 0; i < pointAngleArr.Length; i++)
			{
				var straightPathCalc = new StraightPathCalc();
				straightPathCalc.Init(pointAngleArr[i]);
				pathCalcArr[i] = straightPathCalc;
			}
			return pathCalcArr;
		}
	}
	#endregion
}




