using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Random = UnityEngine.Random;
using System.Runtime.InteropServices;
using ShootingEditor2D;
using System.CodeDom;
using SnakeGame;
//using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;

#region ISubEnemyCreatorMgr ( MissileCreatorMgr NormalCreatorMgr BossCreatorMgr ElitesCreatorMgr )EnemyCreatorMgr


public class DomEnemyCreatorMgr : IGetGameProcessTriggerEvents, IGetGameProcessNormalEvents, IDestroy, IUpdate, QFramework.IController
{


	#region 字属
	/// <summary>不同飞类型机的生成Mgr</summary>
	private ISubEnemyCreatorMgr[] _subEnemyCreatorMgrArr =
	{
		new NormalCreatorMgr(),
		new ElitesCreatorMgr(),
		new BossCreatorMgr(),
		new MissileCreatorMgr()
	};

	private Action<DomEnemyCreatorMgr> _dataComplete;
	/// <summary>同时在场的飞机限制数量，LevelData提供</summary>
	private int _enemyActiveNumMax;
	#endregion  



	#region 生命


	public DomEnemyCreatorMgr Init(Action<DomEnemyCreatorMgr> cb)
	{
	   
		_dataComplete = cb;
		_subEnemyCreatorMgrArr.ForEach(subEnemyCreatorMgr =>
		{
			subEnemyCreatorMgr.Init();
		});

		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY, this);
		this.GetSystem<ILoadCreatorDataSystem>().Init(InitCurLevelEnemyCreator);

		return this;
	}


	#endregion


	#region pub IUpdate  IDestroy
	public int Framing { get; set; }

	public int Frame
	{
		get;
	}

	public void FrameUpdate() { }  //_creatorMgrArr各自

	public void DestroyFunc()
	{
		this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);

		foreach (var mgr in _subEnemyCreatorMgrArr)
		{
			if (mgr is IDestroy)
			{
				(mgr as IDestroy).DestroyFunc();
			}
		}
	}
	#endregion  

	public ISubEnemyCreatorMgr[] ReadEnemyCreatorMgrArr()
	{
		if (_subEnemyCreatorMgrArr != null)
		{ 
			return _subEnemyCreatorMgrArr;
		}

		throw new System.Exception("异常,子CreatorMgr数组未初始化");
	}

	#region pub IGameProcessTriggerEvent IGameProcessNormalEvent
	public List<GameProcessTriggerEvent> GetTriggerEventLst()
	{
		List<GameProcessTriggerEvent> triggerLst = new List<GameProcessTriggerEvent>();
		foreach (var mgr in _subEnemyCreatorMgrArr)
		{
			if (mgr is IGetGameProcessTriggerEvents)
			{
				var trigger = mgr as IGetGameProcessTriggerEvents;
				triggerLst.AddRange(trigger.GetTriggerEventLst());   //new了两个event
			}
		}
		return triggerLst;
	}

	public List<GameProcessNormalEvent> GetNormalEventLst()
	{
		List<GameProcessNormalEvent> list = new List<GameProcessNormalEvent>();
		for (int i = 0; i < _subEnemyCreatorMgrArr.Length; i++)
		{
			ISubEnemyCreatorMgr mgr  = _subEnemyCreatorMgrArr[i]; //接口1
			if (mgr is IGetGameProcessNormalEvents)    //接口2
			{
				IGetGameProcessNormalEvents e = mgr as IGetGameProcessNormalEvents;
				list.AddRange(e.GetNormalEventLst());
			}
		}

		return list;
	}
	#endregion


	#region pub

	public int GetActiveNumMax()
	{
		return _enemyActiveNumMax;
	}

	#endregion


	#region pri


	private void InitCurLevelEnemyCreator(AllEnemyData allEnemyData , PathDataMgr pathDataMgr , EnemyLevelData cfg)  //具体在LoadCreaterData
	{
		LevelData curLevelData = this.SendQuery(new CurLevelDataQuery(cfg));
        for (int i = 0; i < _subEnemyCreatorMgrArr.Length; i++)
		{
			 ISubEnemyCreatorMgr subEnemyCreatorMgr = _subEnemyCreatorMgrArr[i];
			 subEnemyCreatorMgr.InitEnemyCreatorLst(allEnemyData, pathDataMgr, curLevelData);
		}
		_enemyActiveNumMax = curLevelData.EnemyNumMax;
		_dataComplete.DoIfNotNull(this);
	}
	#endregion



	#region 重写
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}






	#endregion
}



#endregion


#region ISubEnemyCreatorMgr 及其实例类
public interface ISubEnemyCreatorMgr
{
	void Init();
	void InitEnemyCreatorLst(AllEnemyData allEnemyData, PathDataMgr pathDataMgr, LevelData levelData);
}

/// <summary>普通</summary>
public class NormalCreatorMgr : ISubEnemyCreatorMgr, IGetGameProcessNormalEvents, ICanGetUtility,ICanSendCommand
{
	/// <summary>轨迹不同，类型不同，就会多一种creator</summary>
	private List<IEnemyCreator> _normalCreatorLst;
	private IEnemyCreator _curNormalCreator;


	#region pub  

	public void Init()
	{
		_normalCreatorLst = new List<IEnemyCreator>();
	}

	public void InitEnemyCreatorLst( AllEnemyData allEnemyData, PathDataMgr pathDataMgr, LevelData levelData)
	{
		_normalCreatorLst = this.SendCommand( new InitEnemyCreatorLstCommand(EnemyType.NORMAL, allEnemyData, pathDataMgr, levelData));
	}
	#endregion  


	#region pub IGameProcessNormalEvent
	public List<GameProcessNormalEvent> GetNormalEventLst()
	{
		var normalEventLst = new List<GameProcessNormalEvent>();

		GameProcessNormalEvent e = new GameProcessNormalEvent(IfChangeCreatorAndSpawnNextQueue, GetSpawningNum, GetSpawnNum());
		normalEventLst.Add(e);

		return normalEventLst;
	}
	#endregion


	#region pri

	private void IfChangeCreatorAndSpawnNextQueue()
	{
	   _curNormalCreator = this.SendCommand(new SpawnAQueueEnemyBySpawningPrgCommand( _normalCreatorLst, _curNormalCreator));
   
	}

	private int GetSpawningNum()
	{
		int total = 0;
		_normalCreatorLst.ForEach(normalCreator =>
		{
			total += normalCreator.GetSpawningNum();
		});

		return total;
	}

	private int GetSpawnNum()
	{
		int total = 0;
		_normalCreatorLst.ForEach(normalCreator =>
		{
			total += normalCreator.GetSpawnNum();
		});

		return total;
	}
	#endregion


	#region QF
	public IArchitecture GetArchitecture()                                            
	{
		return AirCombatApp.Interface;
	}
	#endregion

}

/// <summary>精英怪</summary>	
public class ElitesCreatorMgr : ISubEnemyCreatorMgr, IGetGameProcessTriggerEvents, ICanGetUtility  ,ICanSendCommand
{

	private List<IEnemyCreator> _elitesCreatorLst;
	private int _spawnElitesLimit;
	private IEnemyCreator _curElitesCreator;

	#region 实现
	public void Init()
	{
		_elitesCreatorLst = new List<IEnemyCreator>();
	}

	public void InitEnemyCreatorLst(AllEnemyData enemyData, PathDataMgr pathDataMgr, LevelData levelData)
	{
		_elitesCreatorLst = this.SendCommand(new InitEnemyCreatorLstCommand(EnemyType.ELITES, enemyData, pathDataMgr, levelData));
	}

	public List<GameProcessTriggerEvent> GetTriggerEventLst()
	{
		var list = new List<GameProcessTriggerEvent>();
	 
		GameProcessTriggerEvent elitesTrigger1 = this.SendCommand(new NewGameProcessTriggerEventCommand(0.3f, CalcCreatorBySpawningPrg, needPauseProcess: false, JudgeEnd));
		GameProcessTriggerEvent elitesTrigger2 = this.SendCommand(new NewGameProcessTriggerEventCommand(0.6f, CalcCreatorBySpawningPrg, needPauseProcess: false, JudgeEnd));
		list.Add(elitesTrigger1);
		list.Add(elitesTrigger2);

		return list;
	}
	#endregion


	#region pri


	private void CalcCreatorBySpawningPrg()
	{
		_curElitesCreator = this.SendCommand(new SpawnAQueueEnemyBySpawningPrgCommand(_elitesCreatorLst, _curElitesCreator));
	}



	 bool JudgeEnd()
	{
		foreach (IEnemyCreator creator in _elitesCreatorLst)
		{
			if (!creator.SpawnedAllQueues())
				return false;
		}

		return true;
	}


	#endregion
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}


}


#region Boss 导弹
/// <summary>导弹</summary>
public class MissileCreatorMgr : ISubEnemyCreatorMgr, IGetGameProcessTriggerEvents, ICanSendCommand
{
	private Dictionary<int, List<MissileEnemyCreator>> _enemyCreatorDic;
	/// <summary>区域，块</summary>
	private int _curPatch;

	public void Init()
	{
		_enemyCreatorDic = new Dictionary<int, List<MissileEnemyCreator>>();
		_curPatch = -1;
	}

	public void InitEnemyCreatorLst(AllEnemyData enemyData, PathDataMgr pathDataMgr, LevelData levelData)
	{
		foreach (MissileCreatorData creatorData in levelData.MissileCreaterDatas)
		{
			int batch = creatorData.Batch;
			if (!_enemyCreatorDic.ContainsKey(batch))
				_enemyCreatorDic[batch] = new List<MissileEnemyCreator>();
			MissileEnemyCreator creator;
			if (false)
			{
				//GameObject createrGo = new GameObject(GameObjectName.MissileCreaterMgr);
				//var normalOrEtilesCreator = createrGo.AddComponent<MissileEnemyCreator>();
				//normalOrEtilesCreator.InitComponentEnemy(creatorData,enemyData,pathDataMgr);
				//normalOrEtilesCreator._colliderTrans.SetParent(parent);            
			}
			else
			{
				creator = new MissileEnemyCreator();
				creator.Init(creatorData, enemyData, pathDataMgr);
			}

			_enemyCreatorDic[batch].Add(creator);
		}
	}

	public void Spawn()
	{
		if (_curPatch < 0 || JudgeCurrentPatchEnd())
		{
			_curPatch++;
			if (_enemyCreatorDic.ContainsKey(_curPatch))
			{
				foreach (MissileEnemyCreator creator in _enemyCreatorDic[_curPatch])
				{
					creator.SpawnAQueue();
				}
			}
		}
	}


	/// <summary>到达路径底部</summary>
	private bool JudgeCurrentPatchEnd()
	{
		if (_enemyCreatorDic.ContainsKey(_curPatch))
		{
			foreach (MissileEnemyCreator missileEnemyCreator in _enemyCreatorDic[_curPatch])
			{
				if (!missileEnemyCreator.SpawnedAllQueues())
					return false;
			}

			return true;
		}
		else
		{
			return true;
		}
	}

	public List<GameProcessTriggerEvent> GetTriggerEventLst()
	{
		List<GameProcessTriggerEvent> list = new List<GameProcessTriggerEvent>();
		GameProcessTriggerEvent missileTrigger = this.SendCommand(new NewGameProcessTriggerEventCommand(0.5f, Spawn, true, JudgeCurrentPatchEnd));
		list.Add(missileTrigger);
		return list;
	}


	#region 重写
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
	#endregion  

}



public class BossCreatorMgr : ISubEnemyCreatorMgr, IGetGameProcessTriggerEvents, QFramework.IController, IUpdate, IDestroy ,ICanSendCommand	
{

	private IEnemyCreator _bossEnemyCreator;
	private bool _start;

	public void Init()
	{
		_start = false;
		this.RegisterEvent<AwakeBossCreatorEvent>(_ =>
		{
			_start = true;
		});
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
	}

	public void DestroyFunc()
	{
		this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
	}

	public void InitEnemyCreatorLst( AllEnemyData enemyData, PathDataMgr pathDataMgr, LevelData levelData)
	{
		_bossEnemyCreator = this.SendCommand(new InitEnemyCreatorLstCommand(EnemyType.BOSS, enemyData, pathDataMgr, levelData))[0];
	}

	public void SpawnAQueueBoss()
	{
		_bossEnemyCreator.DoOrThrowNullException(()=> 
		{
			_bossEnemyCreator.SpawnAQueue();
		});
	}



	#region Unpdate
	public int Framing { get; set; }

	public int Frame
	{
		get { return 30; }
	}

	public void FrameUpdate()
	{
		if (!_start)
			return;
	   //
		if (true)
		//if (this.GetSystem<IGameObjectPoolSystem>().GetActiveCount(ResourcesPath.PREFAB_PLANE) == 0)
		{
			this.GetUtility<IGameUtil>().ShowWarnning();
			SpawnAQueueBoss();

          //  this.GetSystem<ICoroutineSystem>().StartDelay(Const.WAIT_BOSS_TIME, SpawnAQueueBoss);
			_start = false;
		}
	}

	#endregion




	public List<GameProcessTriggerEvent> GetTriggerEventLst()
	{
		var list = new List<GameProcessTriggerEvent>();
		bool needPauseProcess=false;  //方便看
		GameProcessTriggerEvent bossTrigger = new GameProcessTriggerEvent(1,OnTriggerEnter, needPauseProcess, OnTriggerExit);//敌人全生成完了，召唤Boss
		list.Add(bossTrigger);
		return list;
	}

	#region pri

    void OnTriggerEnter()
    {
        _start = true;
    }

    bool OnTriggerExit()
    {
       return  this.GetModel<IAirCombatAppStateModel>().E_GameState == GameState.END;
    }
	#endregion

	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}


}
#endregion  

#endregion




#region IEnemyCreator PlaneEnemyCreator MissileEnemyCreator CreatorUtil
public interface ISpawnAQueueEvent
{
    /// <summary>首次询问</summary>
    bool AwakeSpawnAQueue();
    /// <summary>进行的操作</summary>
    void SpawnAQueue();
    /// <summary>是否进行中</summary>
    bool SpawningAQueue();
    /// <summary>完成了一队 </summary>
    bool SpawnedAQueue();
}

public interface IEnemyCreator	:ISpawnAQueueEvent
{
	void Init(ICreatorData creatorData,AllEnemyData allEnemyData,PathDataMgr pathDataMgr);
	/// <summary>已经生成队伍进度</summary>
	float GetSpawningPrg();
	/// <summary>已经生成队伍乘以飞机数</summary>
	int GetSpawningNum();
	/// <summary>总生成队伍乘以飞机数</summary>
	int GetSpawnNum();
	//

	/// <summary>一波飞机结束的条件</summary>
	bool SpawnedAllQueues();
}

/// <summary>除了导弹外的所有敌人类型</summary>
public class EnemyPlaneCreator :MonoBehaviour, IEnemyCreator, ICanGetSystem, ICanGetUtility, IUpdate, IDestroy ,ICanSendCommand
{
	private int _id;
	private Sprite _sprite;
	private EnemyType _enemyType;
	private EnemyData _enemyData;
	/// <summary> 已经生成的队列数量 </summary>
	private int _spawningQueueNum;
	private PlaneCreatorData _planeCreatorData;
	private PathDataMgr _pathDataMgr;
	private PlaneEnemyCtrl _curEnemy;
	private PlaneEnemyCtrl _lastEnemy;
	/// <summary>波次,用来打印</summary>
	private int _wave;
	/// <summary>统一控制Show,防止头机暴毙,失去位置设置的基准点</summary>
	private List<PlaneEnemyCtrl> _curEnemyLst;
	private Transform _parentTrans;
	/// <summary>当前队列的飞机Idx,用于判断当前队列是否完成</summary>
	private int _idxInQueue;
	private IPathData _pathData;
	/// <summary>用数量重置时有时差</summary>
	private bool _spawingAQueue;


	#region 生命
	public void Init(ICreatorData creatorData, AllEnemyData allEnemyData, PathDataMgr pathDataMgr)
	{
		if (creatorData is PlaneCreatorData)
		{
			_planeCreatorData = creatorData as PlaneCreatorData;
		}
		else
		{
			Debug.LogError("传入数据类型错误，类型为:" + creatorData);
			return;
		}

		//
		_spawningQueueNum = 0;
		_curEnemyLst = new List<PlaneEnemyCtrl>();


		_spawingAQueue = false;
		_id = Random.Range(_planeCreatorData.IdMin, _planeCreatorData.IdMax + 1);
		_enemyType = _planeCreatorData.Type;

		//
		string path = GameObjectPath.Creator_EnemyCreator;
		_parentTrans = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(path);
		_parentTrans.GetOrAddComponent<CameraMoveSelfComponent>();
		//
		var spriteName = string.Format(Const.ENEMY_PREFIX, _enemyType, _id);
		_sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(ResourcesPath.PICTURE_ENEMY_FOLDER + spriteName);
		//
		var enemyDataArr = allEnemyData.GetData(_planeCreatorData.Type);         
		_enemyData = enemyDataArr.FirstOrDefault(u => u.id == _id);
		if (_enemyData.IsNull())
		{
			throw new System.Exception("不存在ID为" + _id + "的敌方单位数据，type:" + _planeCreatorData.Type);
		}
		else
		{     
			_pathDataMgr = pathDataMgr;
			_pathDataMgr.Init(_enemyData.trajectoryID);
		}
		//
		InitPos();
		//
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY, this);
	}


	#endregion




	#region pub IUpdate IDestroy

	public int Framing { get; set; }

	public int Frame
	{
		get { return 30; }
	}

	public void FrameUpdate()
	{
		//if (_curEnemy != null && CurEnemyAllInView())
		//{
		//    _curEnemy = null;
		//}
	}

	public void DestroyFunc()
	{
		this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
	}
    #endregion



    #region pub IEnemyCreator
    public bool AwakeSpawnAQueue()
    {
        return _spawningQueueNum==0 && _spawingAQueue==false;
    }

    /// <summary>生成一队的敌人</summary>
    public void SpawnAQueue()
	{

		if (_lastEnemy != null && LastEnemyAllOutView() == false)
		{
			return;
		}
		_spawingAQueue = true;             

		if (_spawningQueueNum <_planeCreatorData.QueueNum) //有多少队 ,大于,因为从0开始(后面打印从1开始就自己加1)
		{
			UpdateCreator();
			for (int posIdxInQueue = 0; posIdxInQueue < _planeCreatorData.QueuePlaneNum; posIdxInQueue++)//一队飞机
			{
				_idxInQueue = posIdxInQueue;
				string goName= String.Format($"{_wave + 1}波{_spawningQueueNum + 1}队{posIdxInQueue + 1}机{_enemyType}类型"); //穿进去方便调试
				_curEnemy = this.SendCommand(new SpawnAEnemyPlaneCommand(posIdxInQueue, _enemyType, _enemyData, _sprite, _pathData,transform.position, goName));
				_curEnemyLst.Add(_curEnemy);
				if (posIdxInQueue == _planeCreatorData.QueuePlaneNum - 1)
				{
					_lastEnemy = _curEnemy;
				}
			}
			foreach (PlaneEnemyCtrl enemy in _curEnemyLst)
			{
				enemy.Show();
			}             
			 
		
		}		
		_spawingAQueue = false;
		_spawningQueueNum++;
	
	}
    public bool SpawningAQueue()
    {
       return _spawingAQueue;
    }

    /// <summary>
    /// <br/> 一队生成完了.这是进行下一队生成的前置条件
    /// </summary>
    public bool SpawnedAQueue()
	{

		if (_spawingAQueue==false && _spawningQueueNum == 0)  //防止spawing时_spawningQueueNum还未赋值,又跳走了
		{
			return true;
		}
		#region 测试                           
		if (_spawingAQueue)
		{
			return false;
		}
		else if (LastEnemyAllOutView())//到最下面
		{
			return true;
		}
   
		else if (_idxInQueue > _planeCreatorData.QueuePlaneNum - 1)
		{
			throw new System.Exception("异常");
		}
		return false;
		#endregion  

	}


	public bool SpawnedAllQueues()
	{
		return GetSpawningPrg() == 1.0f;
	}

	public int GetSpawningNum()
	{
		return _spawningQueueNum * _planeCreatorData.QueuePlaneNum;
	}

	public int GetSpawnNum()
	{
		return _planeCreatorData.QueuePlaneNum * _planeCreatorData.QueueNum;
	}


	public float GetSpawningPrg()
	{
		return (float)_spawningQueueNum / (float)_planeCreatorData.QueueNum;
	}


	#endregion


	#region pri
	void UpdateCreator()
	{
		_pathData = _pathDataMgr.GetData(_enemyData.trajectoryType);
		_wave = transform.GetSiblingIndex();//不以0开始
		_curEnemyLst.Clear();
		_curEnemy = null;
		_lastEnemy = null;
		_idxInQueue = 0;
	}




	private void InitPos()
	{
		var yMax = this.GetUtility<IGameUtil>().CameraMaxPoint().y;
		var xMin = this.GetUtility<IGameUtil>().CameraMinPoint().x;
		var xMax = this.GetUtility<IGameUtil>().CameraMaxPoint().x;

		var pos = new Vector3();
		float x = (float)_planeCreatorData.X;
		pos.x = (x).Clamp(xMin, xMax);
		pos.y = yMax;
		pos.z = Entity2DLayer.PLANE.Enum2Int(); //按不同类型做了一个分层
		transform.position = pos;
	}


	/// <summary>飞机整个进入相机视图内</summary>
	bool CurEnemyAllInView()
	{
		//右上角的Y小于相机的
		float spriteMaxY = _curEnemy.RendererComponent.SpriteRenderer.BoundsMaxY();
		float cameraMaxY = this.GetUtility<IGameUtil>().CameraMaxPoint().y;
		return spriteMaxY < cameraMaxY;
	}

	bool LastEnemyAllOutView()
	{
		if (_lastEnemy != null)
		{ 
			//右上角的Y小于相机的
			float spriteMaxY = _lastEnemy.RendererComponent.SpriteRenderer.BoundsMaxY();
			float cameraMinY = this.GetUtility<IGameUtil>().CameraMinPoint().y;
			if (spriteMaxY < cameraMinY)
			{
				return true;
			}
		}
		return false;

	}

	#endregion


	#region 重写
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
    #endregion
}


/// <summary>导弹，大飞弹</summary>
public class MissileEnemyCreator : IEnemyCreator, ICanGetSystem
{
	private MissileCreatorData _creatorData;
	private bool _isSpawning;
	private int _spawnNum;
	private int _spawningNum;

	public void Init(ICreatorData data, AllEnemyData enemyData, PathDataMgr pathDataMgr)
	{
		if (data is MissileCreatorData)
		{
			_creatorData = data as MissileCreatorData;
		}
		else
		{
			Debug.LogError("当前传入参数错误，参数类型为：" + data);
			return;
		}

		_isSpawning = false;
		_spawnNum = 0;
		_spawningNum = 0;
	}



	#region pub IEnemyCreator
	public float GetSpawningPrg()
	{
		if (_creatorData.IsNull())
		{
			Debug.LogError("当前数据未初始化");
			return 1;
		}
		else
		{
			return GetSpawningNum() / (float)GetSpawnNum();
		}
	}

	public int GetSpawningNum()
	{
		return _spawningNum;
	}

	public int GetSpawnNum()
	{
		if (_creatorData.IsNull())
		{
			Debug.LogError("当前数据未初始化");
			return 0;
		}
		else
		{
			return _creatorData.SpawnCount;
		}
	}      
	
	public void SpawnAQueue()
	{
		if (!_isSpawning)
		{
			_isSpawning = true;
			SpawnNew();
		}
	}
	public bool SpawnedAQueue()
	{
		return _isSpawning;
	}
	public bool SpawnedAllQueues()
	{
		return _spawnNum >= _creatorData.SpawnCount;
	}
	#endregion


	#region pri
	private void SpawnNew()
	{
		_spawningNum++;
		var go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.PREFAB_ENEMY_MISSILE);
		MissileView missile = go.GetOrAddComponent<MissileView>();
		missile.Init((float)_creatorData.X
			, _creatorData.NumOfWarning
			, (float)_creatorData.EachWarningTime
			, (float)_creatorData.Speed);
		missile.AddEndListener(MissileEnd);
	}


	private void MissileEnd()
	{
		_spawnNum++;

		if (!SpawnedAllQueues())
		{
			SpawnNew();
		}

		_isSpawning = !SpawnedAllQueues();
	}
	#endregion


	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}

    public bool AwakeSpawnAQueue()
    {
        throw new NotImplementedException();
    }

    public bool SpawningAQueue()
    {
        throw new NotImplementedException();
    }
}

#endregion












