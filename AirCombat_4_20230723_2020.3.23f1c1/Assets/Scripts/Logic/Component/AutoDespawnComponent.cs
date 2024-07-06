using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>超出视图就回收</summary>
public class AutoDespawnOtherComponent : MonoBehaviour, IUpdate, QFramework.ICanGetSystem, ICanSendCommand 
{

    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private string _despawnKey;
    /// <summary>_sr未赋值好，就跑Update，导致报错</summary>
    [SerializeField] bool _inited = false;
    [SerializeField] Transform _despawnTrans;
    /// <summary>排除掉该方向上的边界销毁</summary>

    [SerializeField]  EDir[] _excludeDirs;


    #region 生命

    private void Start()
    {
            
    }


    //public void InitComponentEnemy(params object[] os)因为一下两个错误
    /// <summary>会受到对象池需要poolKey</summary>
    public AutoDespawnOtherComponent Init( Transform despawnTrans, string poolKey, SpriteRenderer sr,EDir[] excludeDirs=null)
    {
        _inited = false;
        //string poolKey = os[0].ToString(); //这种报空错误
        //string poolKey = (string)os[0];//这种报空错误
        _despawnKey = poolKey;
        _despawnTrans = despawnTrans;
        _sr = sr;
        _excludeDirs = excludeDirs;
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
		_inited = true;

        return this;
    }

    private void OnDisable()
	{
        _inited = false;
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
	}
    #endregion


    #region IUpdate
    public int Framing { get; set; }

	public int Frame
	{
        get;
	}

	public void FrameUpdate()
	{
        if (!_inited)
        {
			return;
        }
        this.SendCommand(new AutoDespawnOtherCollideCameraBorderCommand(_despawnKey, _sr,_despawnTrans,_excludeDirs));
	}
	#endregion


    public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }


}
