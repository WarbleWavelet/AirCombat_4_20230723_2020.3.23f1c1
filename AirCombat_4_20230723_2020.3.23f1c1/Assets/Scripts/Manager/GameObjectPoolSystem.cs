using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Object=UnityEngine.Object;
using static ResourcesName;
using LitJson;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine.Android;
using UniRx.Toolkit;

#region IScenePathSystem

/// <summary>场景限定问题：有的节点只有在某些场景才会出现</summary>
public interface IScenePathSystem : QFramework.ISystem
{

}
public class ScenePathSystem : AbstractSystem, IScenePathSystem
{
	static Dictionary<string, GameObject> _dic;
	protected override void OnInit()
	{
		_dic = new Dictionary<string, GameObject>();
	}
}
#endregion


#region ISetActiveWhen
/// <summary>表示是对象池对象，这里是GameObjectSystem
/// <br/> spawn时会自动执行SetActive。
/// <br/>所以初始化函数放在OnEable里面
/// <br/>该接口起这样的提示作用</summary>
public interface ISetActiveWhenSpawn
{

}

/// <summary>表示是对象池对象，这里是GameObjectSystem
/// <br/> spawn时会自动执行SetActive。
/// <br/>所以初始化函数放在OnEable里面
/// <br/>该接口起这样的提示作用</summary>
public interface ISetActiveWhenDespawn
{

}
#endregion



#region IObjectPoolSystem
/// <summary>这种一个类型一种，仅仅new</summary>
public interface IObjectPoolInstanceSystem : QFramework.ISystem
{
	BulletEffectContainerBase SpawnBulletEffect(BulletType bulletType); 
}



public class ObjectPoolInstanceSystem : AbstractSystem, IObjectPoolInstanceSystem
{
	protected override void OnInit()
	{
		SafeObjectPool<Boss1BulletEffectContainer>.Instance.Init(10,5);
		SafeObjectPool<Boss1BulletEffectContainer>.Instance.SetFactoryMethod(()=> new Boss1BulletEffectContainer());
		//
	}
	public BulletEffectContainerBase SpawnBulletEffect(BulletType bulletType)
	{
		switch ( bulletType )
		{
			case BulletType.ENEMY_BOSS_1 : return SafeObjectPool<Boss1BulletEffectContainer>.Instance.Allocate(); 
			default: return null;
		}

	}


}
#endregion






#region IGameObjectPoolSystem


#region 接口
public interface IGameObjectPoolVar
{ 
	/// <summary>path一部分有ResourcesPath构成,一部分由枚举类型构成</summary>
	GameObject SpawnByPart2(string path, Transform poolT);
}


/// <summary>PoolKey==gameObject.name</summary>
public interface IGameObjectPoolKeyIsName
{
	/// <summary>是池子里面的就回收,自动Hide</summary>
	bool DespawnWhileKeyIsName(GameObject go, Action cb = null);
	bool DespawnWhileKeyIsName(GameObject go);
	bool DespawnWhileKeyIsName(Transform trans);
	bool DespawnWhileKeyIsName(MonoBehaviour mono);
	bool AddWhileKeyIsName(GameObject prefab, GameObjectPool pool);

}

public interface IGameObjectPoolAdd
{
	/// <summary>有Pool就Spawn,没有就新建一个PoolS</summary>
	GameObject SpawnOrAdd(string path, Transform poolT);
	bool Add(string path, GameObjectPool pool);
}

public interface IGameObjectPoolDelay
{
	void DespawnDelay(GameObject go,string key);
}
#endregion	

public interface IGameObjectPoolSystem  : QFramework.ISystem, IGameObjectPoolAdd, IGameObjectPoolKeyIsName
{
	 void Init(Action callBack);
	/// <summary>小兵数量，判断Boss来临</summary>
	 int GetActiveCount(string path);
	/// <summary>Spawn会自动调用Show,但是自己加的组件有的需要Show触发,所以想要触发就自己在Show一下</summary>
	GameObject Spawn(string path);
	/// <summary>如果想刚生成就修改名字</summary>
	GameObject Spawn(string path,string objectName);

	/// <summary>路径krey</summary>
	bool Despawn(GameObject go,string key);
}


public class GameObjectPoolSystem :  AbstractSystem, IGameObjectPoolSystem  ,IDestroy
{

	#region 字属构造


	private Action _initComplete;
   
	private Dictionary<string, GameObjectPool> _dic;
	int _defaultPreLoadCnt;
	ResLoader _resLoader;
	#endregion





	#region 生命
	protected override void OnInit()
	{
	   _resLoader= ResLoader.Allocate();
		_defaultPreLoadCnt = 5;

		this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY,this);
		Init();
		// if (this.GetModel<IAirCombatAppStateModel>().CurScene == ESceneName.Game){ }//没用，要监听才行
		this.SendEvent<InitGameObjectPoolSystemEvent>(); //
	}

	private  void Init()
	{
		_dic = new Dictionary<string, GameObjectPool>();
		GameObjectPoolConfig cfg = new GameObjectPoolConfig();
		GameObject prefab = null;
		Transform parent = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectName.Pool);
	 
		foreach (var data in cfg.PoolDataLst)
		{      
			prefab = this.GetSystem<IGameObjectSystem>().Load(data.Path,LoadType.RESOURCES);
			GameObjectPool pool = new GameObjectPool();
			pool.Init(prefab,parent,data.PreloadCount,data.AutoDestroy);
			_dic.Add(data.Path, pool);
		}
		//
		//
		_initComplete.DoIfNotNull();
	   
	}



	public void DestroyFunc()
	{
		_resLoader.Dispose();
	}

	public void Init(Action callBack)
	{
		_initComplete = callBack;
		//InitComponent();
	}
	#endregion


	#region pub

	public GameObject Spawn(string poolKey)
	{

		if (_dic.ContainsKey(poolKey))
		{
			 return SpawnPreProgress(_dic[poolKey].Spawn());
		}


		Debug.LogError("当前预制体没有在对象池的管理中，预制路径:" + poolKey);
		return null;
	}

    public GameObject Spawn(string poolKey, string objectName)
    {
		GameObject go = Spawn(poolKey);
		go.name=objectName;
		return go;
    }


    /// <summary>
    /// 回收
    /// <para/>自定义key
    /// </summary>
    public bool Despawn(GameObject go, string key)
	{
		go = DespawnPreprocess(go);
		foreach (var pair in _dic)
		{
			try
			{
				if (pair.Key.Contains(key))
				{
					pair.Value.Despawn(go);
					return true;
				}
			}
			catch (Exception e)
			{


				throw new System.Exception("回收时，key错误" + key + "/n" + e);
			}
		}

		return false;
	}

	public int GetActiveCount(string path)
	{
		if (_dic.ContainsKey(path))
		{
			return _dic[path].GetActiveCount();
		}
		else
		{
			Debug.LogError("PoolMgr中该MemoryPool\r\n并不存在，path:" + path);
			return 0;
		}
	}

	#endregion


	#region Add

	public bool Add(string poolKey, GameObjectPool pool)
	{
		GameObjectPool res =new GameObjectPool();
		if (_dic.TryGetValue(poolKey, out res))
		{
			return false;
		}
		else
		{
			_dic.Add(poolKey,pool);
			return true;
		}
	}


	public GameObject SpawnOrAdd(string poolKey, Transform poolT)
	{
		if (_dic.ContainsKey(poolKey))
		{
			return SpawnPreProgress(_dic[poolKey].Spawn());

		}
		else
		{
			GameObject prefab = this.GetSystem<IGameObjectSystem>().Load(poolKey, LoadType.RESOURCES);
			if (prefab.IsNotNull())
			{
				GameObjectPool pool = new GameObjectPool();

				pool.Init(prefab,poolT, _defaultPreLoadCnt, false);
				_dic.Add(poolKey, pool);
				return SpawnPreProgress(_dic[poolKey].Spawn());

			}
			Debug.LogErrorFormat($"没有对应的Pool,并且没有Prefab,路径是:{0}" , poolKey);
		}

		return null;
	}


	/// <summary>
	///  合成path= Prefab/Game/Bullet/PLAYER,
	///  <br/>其中 Prefab/Game/Bullet用来向Resouces加载物体
	///   <br/>加上PLAYER是区分对象池
	///   <br/>poolParent父节点
	/// </summary>
	/// <param name="poolKey"></param>
	/// <param name="varName">比如不同枚举类型的,所加的脚本不一样</param>
	/// <param name="poolParent"></param>
	/// <returns></returns>
	public GameObject SpawnByPart2(string poolKey,  Transform poolParent)
	{
		if (_dic.ContainsKey(poolKey))
		{
			return SpawnPreProgress(_dic[poolKey].Spawn());

		}
		else
		{
			string resourcesPath = poolKey.TrimName(TrimNameType.SlashPre); //A/B
			string varName = poolKey.TrimName(TrimNameType.SlashAfter);		  //C
			GameObject prefab = this.GetSystem<IGameObjectSystem>().Load(resourcesPath, LoadType.RESOURCES);
;            if (prefab.IsNotNull())
			{
				GameObjectPool pool = new GameObjectPool();
				pool.Init(prefab, poolParent, _defaultPreLoadCnt, false);
				//
				_dic.Add(poolKey, pool);
				return SpawnPreProgress(_dic[poolKey].Spawn());

			}
			Debug.LogErrorFormat($"没有对应的Pool,并且没有Prefab,路径是:{poolKey}");
		}

		return null;
	}
	#endregion



	#region WhileKeyIsName

	public bool AddWhileKeyIsName(GameObject prefab, GameObjectPool pool)
	{
		string poolKey = prefab.name;
		GameObjectPool res = new GameObjectPool();
		if (_dic.TryGetValue(poolKey, out res))
		{
			return false;
		}
		else
		{
			_dic.Add(poolKey, pool);
			return true;
		}
	}

	/// <summary>
	/// 回收
	/// <para />key是go.name
	/// </summary>
	public bool Despawn(GameObject go,Action cb=null)
	{
		go = DespawnPreprocess(go);

		foreach (var pair in _dic)
		{
			if (pair.Key.Contains(go.name))
			{
				pair.Value.Despawn(go,cb);
				return true;
			}
		}

		return false;
	}


	public bool DespawnWhileKeyIsName(GameObject go, Action cb = null)
	{
		throw new NotImplementedException();
	}

	public bool DespawnWhileKeyIsName(GameObject go)
	{
		string poolKey = go.name;
		return Despawn(go, poolKey);
	}

	public bool DespawnWhileKeyIsName(Transform trans)
	{
		throw new NotImplementedException();
	}

	public bool DespawnWhileKeyIsName(MonoBehaviour mono)
	{
		throw new NotImplementedException();
	}
	#endregion




	#region 未处理	    

	public void DespawnDelay(GameObject go, string key)
	{ 
		  this.GetSystem<ICoroutineSystem>().StartDelay(1f, () => 
		  {
			  Despawn(go, key);
		  });
	}
	#endregion


	#region pri
	/// <summary>预处理</summary>
	GameObject SpawnPreProgress(GameObject go)
	{
		go.name = ResetName(go.name);
		return go;
	}

	string ResetName(string goName)
	{

        goName= goName.Replace(Affixes.CloneWithBracket, Affixes.NullString);
		 return   goName;
	}
	GameObject DespawnPreprocess(GameObject go)
	{
		go.name = ResetName(go.name);
		//go.transform.SetPosY(0f);//Pool没必要
		//go.Hide(); //Pool已经Hide
		return go;
	}
	#endregion

	#region QF
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}




    #endregion
}
#endregion


#region ObjectPool ,啥都没有的，New的，Class的

public interface IAutoDestroy
{ 

}
public interface IObjectPool<T>
{
	void Init(int preloadCount, bool autoDestroy);
	T Spawn();
	void Despawn(T o);
}
public class ObjectPoolInstance<T> : IObjectPool<T>   , ISingleton
{


	#region 单例
	public void OnSingletonInit()
	{
		ObjectPoolInstance<IEffectContainer>.Instance.Spawn();
	}

	#endregion
	public static ObjectPoolInstance<T> Instance
	{
		get { return SingletonProperty<ObjectPoolInstance<T>>.Instance; }
	}

	public void Init(int preloadCount, bool autoDestroy)
	{
	  
	}

	public T Spawn()
	{
		throw new NotImplementedException();
	}

	public void Despawn(T o)
	{
		throw new NotImplementedException();
	}


}

public class ObjectPoolSafe<T> : Pool<T>, ISingleton where T : IPoolable, new()
{
	#region Singleton

	void ISingleton.OnSingletonInit()
	{
	}

	protected ObjectPoolSafe()
	{
		mFactory = new DefaultObjectFactory<T>();
	}

	/// <summary>不想让用户通过 SafeObjectPool 来 Allocate 和 Recycle 池对象了，那么 Allocate 和 Recycle 的控制权就要交给池对象来管理</summary>
	public static SafeObjectPool<T> Instance
	{
		get { return SingletonProperty<SafeObjectPool<T>>.Instance; }
	}

	public void Dispose()
	{
		SingletonProperty<SafeObjectPool<T>>.Dispose();
	}

	#endregion

	/// <summary>
	/// InitComponentEnemy the specified maxCount and initCount.
	/// </summary>
	/// <param name="maxCount">Max Cache count.</param>
	/// <param name="initCount">InitComponentEnemy Cache count.</param>
	public void Init(int maxCount, int initCount)
	{
		MaxCacheCount = maxCount;

		if (maxCount > 0)
		{
			initCount = Math.Min(maxCount, initCount);
		}

		if (CurCount < initCount)
		{
			for (var i = CurCount; i < initCount; ++i)
			{
				Recycle(new T());
			}
		}
	}

	/// <summary>
	/// Gets or sets the max cache count.
	/// </summary>
	/// <value>The max cache count.</value>
	public int MaxCacheCount
	{
		get { return mMaxCount; }
		set
		{
			mMaxCount = value;

			if (mCacheStack != null)
			{
				if (mMaxCount > 0)
				{
					if (mMaxCount < mCacheStack.Count)
					{
						int removeCount = mCacheStack.Count - mMaxCount;
						while (removeCount > 0)
						{
							mCacheStack.Pop();
							--removeCount;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Allocate T1 instance.
	/// </summary>
	public override T Allocate()
	{
		var result = base.Allocate();
		result.IsRecycled = false;
		return result;
	}



	#region 实现


	/// <summary>
	/// Recycle the T1 instance
	/// </summary>
	/// <param name="t">T1.</param>
	public override bool Recycle(T t)
	{
		if (t == null || t.IsRecycled)
		{
			return false;
		}

		if (mMaxCount > 0)
		{
			if (mCacheStack.Count >= mMaxCount)
			{
				t.OnRecycled();
				return false;
			}
		}

		t.IsRecycled = true;
		t.OnRecycled();
		mCacheStack.Push(t);

		return true;
	}
	#endregion

}

public class ObjectPoolClass<T> : IObjectPool<T> where T : class
{
	public void Despawn(T o)
	{
		throw new NotImplementedException();
	}

	public void Init(int preloadCount, bool autoDestroy)
	{
		throw new NotImplementedException();
	}

	public T Spawn()
	{
		throw new NotImplementedException();
	}
}
public class ObjectPoolNew<T> : IObjectPool<T>  where T:new()
{
	private List<T> _activeLst;
	private List<T> _inactiveLst;
	/// <summary>闲置时限</summary>
	private float _freeTimeMax = 5;
	private DateTime _spawnTime;
	private bool _isDestroying;



	#region pub IObjectPool


	public void Init(int preloadCount, bool autoDestroy)
	{
		_activeLst = new List<T>();
		_inactiveLst = new List<T>();
		Preload(preloadCount);
		AutoDestroy(preloadCount, autoDestroy);
	}


	public T Spawn()
	{
		_spawnTime = DateTime.Now;

		if (_inactiveLst.Count > 0)
		{
			var o = _inactiveLst[0];
			_activeLst.Add(o);
			return o;
		}

		return SpawnNew();
	}
	public void Despawn(T o)
	{
		if (_activeLst.Contains(o))
		{
			_activeLst.Remove(o);
			_inactiveLst.Add(o);
		}
	}
	#endregion

	#region pri
	private void Preload(int preloadCount)
	{
		for (int i = 0; i < preloadCount; i++)
		{
			SpawnNew();
		}
	}

	private async void AutoDestroy(int preloadCount, bool autoDestroy)
	{
		while (true)
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
			var spendTime = (_spawnTime - DateTime.Now).Seconds;
			if (spendTime >= _freeTimeMax && !_isDestroying)
			{
				_isDestroying = true;
				StartDestroy(preloadCount);
			}
		}
	}

	private async void StartDestroy(int preloadCount)
	{
		while (_inactiveLst.Count > preloadCount)
		{
			await Task.Delay(100);
			_inactiveLst.RemoveAt(0);
		}

		_isDestroying = false;
	}

	private T SpawnNew()
	{
		var o =  new T();
		_activeLst.Add(o);
		return o;
	}

	#endregion
}
#endregion


#region GameObjectPool
public interface IGameObjectPool
{
	public void Init(GameObject prefab, Transform parent, int preloadCount, bool autoDestroy);
	public GameObject Spawn();

	/// <summary>也就是Recycle</summary>
	public void Despawn(GameObject go, Action cb = null);

	public int GetActiveCount();

}

/// <summary>
/// 01 有Show和Hide的区别(),维护了这两个列表
/// <br/>有recycleTrans</summary>
public class GameObjectPool :IGameObjectPool 
{
	private List<GameObject> _acticveLst;
	private List<GameObject> _inactiveLst;
	private bool _isDestroying;
	/// <summary>长时间不用回归</summary>
	private readonly int _freeTimeMax = 10;
	private readonly int _freeTimSpan = 5;
	/// <summary>长时间不用也保留着的个数</summary>
	private int _preInactiveNum;

	private GameObject _prefab;

	/// <summary>用处计算自动销毁的时间</summary>
	private DateTime _spawnTime;
	//
	/// <summary>BulletPool</summary>
	private Transform _spawnTrans;
	/// <summary>BulletPool</summary>
	private Transform _despawnTrans;
	//
	private string _poolName;
	/// <summary>回收时对象的名字复原</summary>
	private string _objectName;



	/// <summary>
	///  poolParent 对象池的父节点
	/// </summary>
	/// <param name="prefab"></param>
	/// <param name="poolParent"></param>
	/// <param name="preloadCount"></param>
	/// <param name="autoDestroy"></param>
	public void Init(GameObject prefab, Transform poolParent, int preloadCount, bool autoDestroy )
	{
		_isDestroying = false;
		_preInactiveNum = preloadCount;
		//
		_acticveLst = new List<GameObject>();
        _inactiveLst = new List<GameObject>(); //preloadCount 用在这里是容量扩容,不是Count
                                     
        _prefab = prefab;
		_poolName = SCGameObjectPoolKey.GetPoolName(prefab.name);
		_objectName = prefab.name;
		//
		Transform t = poolParent.FindContains(_poolName);
		_despawnTrans = ((t.IsNotNullObject()) ? t.gameObject : new GameObject(_poolName)).transform; //BulletPool
		_despawnTrans.SetParent(poolParent);
		_spawnTrans = _despawnTrans;
		PreSpawnInactive(preloadCount);

		if (autoDestroy)
		{
			AutoDestroy();
		}
	}


	#region pub IGameObjectPool





	/// <summary>
	/// <br/>会执行SetActive(true) Show
	/// <br/>实际调用SetActive,对应在OnEnable写内容
	/// </summary>
	public GameObject Spawn()
	{
        _spawnTime = DateTime.Now; //
		GameObject go = null;
		if (_inactiveLst.Count > 0)
		{
			go = _inactiveLst[0];
			_inactiveLst.Remove(go);
		}
		else
		{
			go = InstantiateNew();
		}
		_acticveLst.Add(go);
		go.Show(); 
		SetPoolName();
		return go;
	}

    private void PreSpawnInactive(int count)
    {
        for (var i = 0; i < count; i++)
        {
            //_spawnTime = DateTime.Now;
            GameObject go = InstantiateNew();
            OnDespawn(go);
        }
    }


	#endregion

	#region Ot
	/// <summary>也就是Recycle
	/// <br/>会执行SetActive(false) Hide ,没什么用,因为
	/// </summary>
	public void Despawn(GameObject go,Action cb=null)
	{
		if (_acticveLst.Contains(go))
		{
			_acticveLst.Remove(go);
			OnDespawn(go);
			cb.DoIfNotNull();//位置重置啊之类 	

		}
	}

	public int GetActiveCount()
	{
		return _acticveLst.Count;
	}

    public int GetInactiveCount()
    {
        return _inactiveLst.Count;
    }

    public int GetTotalCount()
    {
        return _inactiveLst.Count + _acticveLst.Count;
    }

	#endregion





    #region pri
    void OnDespawn(GameObject go)
	{
        _inactiveLst.Add(go);
        go.Hide();
        go.transform.SetParent(_despawnTrans);
        go.transform.SetPosXYZ(0, 0, 0); //这里比较奇怪,作为回调交出去,老有偏差.直接在这里设置就不会
		go.name = _objectName;
        SetPoolName();
    }


    private void SetPoolName()
	{
		_despawnTrans.gameObject.name = String.Format($"{_poolName}_{_inactiveLst.Count + _acticveLst.Count}_{_acticveLst.Count}");//记录,好看点

	}

	private async void AutoDestroy()
	{
		while (true)
		{
			await Task.Delay(TimeSpan.FromSeconds(_freeTimSpan));
			var spendTime = (_spawnTime - DateTime.Now).Seconds;
			if (spendTime >= _freeTimeMax && !_isDestroying)
			{
				_isDestroying = true;
				StartDestroy();
			}
		}
	}


	private GameObject InstantiateNew()
	{
		GameObject go= GameObject.Instantiate(_prefab, _despawnTrans.transform);
		go.name = go.name.Replace(Affixes.CloneWithBracket, Affixes.NullString);
		return go;
	}

	private async void StartDestroy()
	{
		GameObject go = null;
		while (_inactiveLst.Count > _preInactiveNum)
		{
			await Task.Delay(100);
			go = _inactiveLst[0];
			_inactiveLst.Remove(go);
			GameObject.Destroy(go);
		}

		_isDestroying = false;
	}


	#endregion
}
#endregion


#region PoolData PoolConfig
public class PoolData
{
	/// <summary>资源路径
	/// <para/>Resources，就只取Resources后面的</summary>
	public string Path { get; set; }
	/// <summary>预加载总数</summary>
	public int PreloadCount { get; set; }
	public bool AutoDestroy { get; set; }
}

public class GameObjectPoolConfig
{
	public List<PoolData> PoolDataLst = new List<PoolData>
	{
		new PoolData
		{
			Path = ResourcesPath.PREFAB_BULLET,
			PreloadCount = 10, //TODO:这里子弹太少会出现未倒边界就没了,奇怪 .=>Spawn时调用了Hide
			AutoDestroy = false
		},
		new PoolData
		{
			Path = ResourcesPath.PREFAB_PLANE,
			PreloadCount = 5,//15
			AutoDestroy = false
		},
		new PoolData
		{
			Path = ResourcesPath.PREFAB_ITEM_ITEM,
			PreloadCount = 10,
			AutoDestroy = false
		},
		new PoolData
		{
			Path = ResourcesPath.EFFECT_FRAME_ANI,
			PreloadCount = 15,
			AutoDestroy = false
		},
		new PoolData
		{
			Path = ResourcesPath.PREFAB_ENEMY_MISSILE,
			PreloadCount = 3,
			AutoDestroy = true
		},
		new PoolData
		{
			Path = ResourcesPath.PREFAB_ITEM_LIGHT,
			PreloadCount = 3,
			AutoDestroy = true
		}  ,
		new PoolData
		{
			Path = ResourcesPath.PREFAB_MAP_CTRL,
			PreloadCount = 3,
			AutoDestroy = false
		}
	};
}

#endregion



#region PoolObjectBase

#endregion
