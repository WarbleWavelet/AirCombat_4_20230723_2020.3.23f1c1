using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoroutineState = CoroutineLife.CoroutineState;



#region CoroutineSystem


public interface ICoroutineSystem :QFramework.ISystem
{
	/// <summary>idx交给外部</summary>
	int StartOutter(IEnumerator routine);
	void StartOutter(int id);
	void StartDelay(float time, Action callBack);
	void StartInner(IEnumerator routine);
	void Restart(int id);
	void Pause(int id);
	void Continue(int id);
	void Stop(int id);
}

/// <summary>维护了两个同类型字典，一个跑一次，一个存起来
///  <para /> Dictionary &lt;int, CoroutineHandler  &gt;
/// </summary>

public class CoroutineSystem : QFramework.AbstractSystem,ICoroutineSystem  
{


	#region 字属生命
	//以下两个一开始在构造函数中的
	/// <summary></summary>   
	private readonly Dictionary<int, CoroutineHandler> _innerDic = new Dictionary<int, CoroutineHandler>();
	private readonly Dictionary<int, CoroutineHandler> _outterDic = new Dictionary<int, CoroutineHandler>();

	private   MonoBehaviour _mono;
	MonoBehaviour Mono
	{
		get
		{
			if (_mono == null)
			{
				// _mono = new MonoBehaviour(); //这种是不行,为null
				Transform t = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.System_CoroutineSystem);
				_mono = t.GetOrAddComponent<CoroutineSystemMono>(); //这种是不行,为null.readonly只能在和实例和构造时使用
				_mono.CkeckNull();
			}
			return _mono;
		}
	}

	#endregion

	protected override void OnInit()
	{



	}

	#region ICoroutineSystem



	/// <summary>就是WaitForSeconds</summary>
	public void StartDelay(float time, Action callBack)
	{
		Execute(WaitForSeconds(time, callBack));
	}

	public void StartInner(IEnumerator routine)
	{
        var ctrl = new CoroutineHandler(Mono, routine);
        ctrl.DoIfNotNull(ctrl.Start);
        //
        _innerDic.Add(ctrl.ID, ctrl);
    }

	/// <summary>发射子弹等
	/// <para/> 返回ID叫给外部控制权限
	/// <para/>Handler有权柄转移的意思</summary>

	public int StartOutter(IEnumerator routine)
	{
		var ctrl = new CoroutineHandler(Mono, routine);
		ctrl.DoIfNotNull(ctrl.Start);
		//
		_outterDic.Add(ctrl.ID, ctrl);
		return ctrl.ID;
	}
	public void StartOutter(int id)
	{
		var ctrl = GetController(id);
		ctrl.DoIfNotNull(ctrl.Start);
	}	


	public void Restart(int id)
	{
		var ctrl = GetController(id);
		ctrl.DoIfNotNull(ctrl.Restart);
	}



	public void Pause(int id)
	{
		var ctrl = GetController(id);
		ctrl.DoIfNotNull(ctrl.Pause);
	}


	public void Continue(int id)
	{
		var ctrl = GetController(id);
		ctrl.DoIfNotNull(ctrl.Continue);
	}

	public void Stop(int id)
	{
		var ctrl = GetController(id);
		if (ctrl.IsNotNull()) // ctrl.DoIfNotNull(ctrl.Stop);//会报错
		{
			ctrl.Stop();
		}
		//
		if (_outterDic.ContainsKey(id))
			_outterDic.Remove(id);
	}







	#endregion


	#region pri

	/// <summary>在两个个字典中找</summary>
	private int Execute(IEnumerator routine, bool autoStart = true)
	{
		var ctrl = new CoroutineHandler(Mono, routine);

		_innerDic.Add(ctrl.ID, ctrl);
		if (autoStart)
		{
			StartOutter(ctrl.ID);
		}

		return ctrl.ID;
	}
	private CoroutineHandler GetController(int id)
	{
		if (_innerDic.ContainsKey(id))
		{
			return _innerDic[id];
		}
		else if(_outterDic.ContainsKey(id))
		{
			return _outterDic[id];
		}

		return null;
	}


	private IEnumerator WaitForSeconds(float time, Action cb)
	{
		yield return new WaitForSeconds(time);
		cb.DoIfNotNull();
	}


	#endregion

}
#endregion


#region DelayDetalCoroutineSystem


public interface IDelayDetalCoroutineSystem : QFramework.ISystem
{
	int Start(float time, Action begin, Action complete, int id);

	bool IsRunning(int id);
}

public class DelayDetalCoroutineSystem : QFramework.AbstractSystem, IDelayDetalCoroutineSystem
{

	private ObjectPoolNew<DelayDetalCoroutineItem> _itemPool;
	private Dictionary<int, DelayDetalCoroutineItem> _itemDic;
	protected override void OnInit()
	{
		_itemDic = new Dictionary<int, DelayDetalCoroutineItem>();
		_itemPool = new ObjectPoolNew<DelayDetalCoroutineItem>();
		_itemPool.Init(3, true);
	}



	#region pub IDelayDetalCoroutineSystem 
	public int Start(float time, Action begin, Action onComplete, int id)

	{
		var item = GetItem(id);
		onComplete += () => End(item);
		item.Start(time, begin, onComplete);
		return item.ID;
	}


	public bool IsRunning(int id)
	{
		if (_itemDic.ContainsKey(id))
		{
			if (_itemDic[id] == null)
			{
				_itemDic.Remove(id);
				return false;
			}
			else
			{
				return _itemDic[id].IsRunning;
			}
		}
		else
		{
			return false;
		}
	}
	#endregion




	#region pri
	private void End(DelayDetalCoroutineItem item)
	{
		_itemPool.Despawn(item);
	}

	private DelayDetalCoroutineItem GetItem(int id)
	{
		if (_itemDic.ContainsKey(id))
		{
			if (_itemDic[id] == null)
			{
				return SpawnNew();
			}
			else
			{
				return _itemDic[id];
			}
		}
		else
		{
			return SpawnNew();
		}
	}

	private DelayDetalCoroutineItem SpawnNew()
	{
		var item = _itemPool.Spawn();
		_itemDic[item.ID] = item;
		return item;
	}
	#endregion





}
#endregion


#region CoroutineController


public interface ICoroutineHandler 
{
	void Start();
	void Restart();

	void Stop();
	//

	void Pause();
	void Continue();
}

public class CoroutineHandler : ICoroutineHandler
{

	#region 字属构造


	private static int _id = 1;
	private Coroutine _coroutine;
	private readonly CoroutineLife _item;
	private readonly IEnumerator _routine;
	private readonly  MonoBehaviour _mono;
	/// <summary>初始1，++</summary>
	public int ID { get; private set; }


	public CoroutineHandler(MonoBehaviour mono, IEnumerator routine)
	{

		_item = new CoroutineLife();
		_mono = mono;
		_routine = routine;
		ResetData();
	}
	#endregion


	#region ICoroutineController  
	public void Start()
	{
		_item.State = CoroutineState.RUNNING;
		IEnumerator e = _item.Body(_routine);
		_coroutine = _mono.StartCoroutine( e );
	}



	#region  实际控制代码在_coroutine
	public void Pause()
	{
		_item.State = CoroutineState.PASUED;
	}


	public void Continue()
	{
		_item.State = CoroutineState.RUNNING;
	}



	public void Stop()
	{
		_item.State = CoroutineState.STOP;
	}
	#endregion



	public void Restart()
	{
		if (_coroutine.IsNotNull())
		{
			_mono.StopCoroutine(_coroutine);
		}

		Start();
	}
	#endregion



	#region pri
	private void ResetData()
	{
		ID = _id++;
	}
	#endregion

}
#endregion


#region CoroutineItem
/// <summary>维护一个属性CoroutineState</summary>
public class CoroutineLife
{

	#region 内部枚举
	/// <summary>CoroutineItem的内部枚举</summary>
	public enum CoroutineState
	{
		RUNNING,
		WAITTING,
		PASUED,
		STOP
	}
	#endregion


	public CoroutineState State { get; set; }



	#region pub
	/// <summary>相当于Update,在跑生命</summary>
	public IEnumerator Body(IEnumerator routine)
	{
		while (State == CoroutineState.WAITTING)
		{
			yield return null;
		}

		while (State == CoroutineState.RUNNING)
		{
			if (State == CoroutineState.PASUED)
			{
				yield return null;
			}
			else
			{
				if (routine != null && routine.MoveNext())
				{
					yield return routine.Current;
				}
				else
				{
					State = CoroutineState.STOP;
				}
			}
		}

	}

	#endregion  
}
#endregion  


#region DelayDetalCoroutineItem
public interface IDelayDetalCoroutineItem
{
	void Start(float time, Action onBegin, Action onComplete);


}
public class DelayDetalCoroutineItem : IDelayDetalCoroutineItem ,ICanGetSystem
{

	#region 字属构造


	private float _time;
	private float _timeDetal;
	private DateTime _startTime;
	private static int _idCounter = -1;
	public int ID { get; private set; }
	public bool IsRunning { get; private set; }

	public DelayDetalCoroutineItem()
	{
		_idCounter++;
		ID = _idCounter;
	}
	#endregion


	public void Start(float time, Action onBegin, Action onComplete)
	{
		if (!IsRunning)
		{

			Console.WriteLine(DateTime.Now);
			_startTime = DateTime.Now;
			_time = time;
			IsRunning = true;
			this.GetSystem<ICoroutineSystem>().StartOutter(Wait(onComplete));
			_time = 0;
			_timeDetal = 0;
			onBegin.DoIfNotNull();
		}
		else
		{
			float spendTime = (float)(DateTime.Now - _startTime).TotalSeconds;
			_timeDetal += spendTime;
		}
	}

	private IEnumerator Wait(Action cb)
	{
		while (IsRunning && _time != 0)
		{
			yield return new WaitForSeconds(_time);
			_time = _timeDetal;
			_timeDetal = 0;
		}

		cb.DoIfNotNull();
	}                             

	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}

#endregion	   



