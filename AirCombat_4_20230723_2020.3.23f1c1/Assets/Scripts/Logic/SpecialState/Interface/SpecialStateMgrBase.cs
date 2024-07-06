using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpecialStateMgrBase : MonoBehaviour ,ICanGetUtility ,ICanGetSystem
{

	#region 字属构造
	private Dictionary<BuffType,IBuff> _buffDic;
	private Dictionary<DebuffType,IDebuff> _debuffDic;
	private HashSet<BuffType> _canBuffHsh;
	private HashSet<DebuffType> _canDebuffHsh;
	

	#endregion


	#region 生命
	void Start ()
	{
		_buffDic = new Dictionary<BuffType, IBuff>();
		_debuffDic = new Dictionary<DebuffType, IDebuff>();
		_canBuffHsh = GetCanBuffHsh();
		_canDebuffHsh = GetCanDebuffHsh();
		AddListener();
	}

	private void OnDestroy()
	{
		RemoveListener();
	}
	#endregion


    #region abstract
    protected abstract HashSet<BuffType> GetCanBuffHsh();
    protected abstract HashSet<DebuffType> GetCanDebuffHsh();
    #endregion


    #region 辅助 pri
    private IBuff GetBuffObject(BuffType type)
    {
        if (_buffDic.ContainsKey(type))
        {
            return _buffDic[type];
        }
        else
        {
            IBuff buff = BuffFactory.GetBuff(type);
            _buffDic.Add(type, buff);
            return buff;
        }
    }

    private IDebuff GetDebuffObject(DebuffType type)
    {
        if (_debuffDic.ContainsKey(type))
        {
            return _debuffDic[type];
        }
        else
        {
            IDebuff debuff = BuffFactory.GetDebuff(type);
            _debuffDic.Add(type, debuff);
            return debuff;
        }
    }

    private void Buff(object[] paras)
    {
        BuffType type = (BuffType)paras[0];
        ExecuteBuff(type);
    }

    private void Debuff(object[] paras)
    {
        DebuffType type = (DebuffType)paras[0];
        ExecuteDebuff(type);
    }
	#endregion


	#region  pro
	protected virtual void AddListener()
	{
		this.GetSystem<IMessageSystem>().AddListener(MsgEvent.EVENT_BUFF,Buff);
        this.GetSystem<IMessageSystem>().AddListener(MsgEvent.EVENT_DEBUFF,Debuff);
	}

	protected virtual void RemoveListener()
	{
        this.GetSystem<IMessageSystem>().RemoveListener(MsgEvent.EVENT_BUFF,Buff);
        this.GetSystem<IMessageSystem>().RemoveListener(MsgEvent.EVENT_DEBUFF,Debuff);
	}



	protected virtual void ExecuteBuff(BuffType type)
	{
		if (_canBuffHsh.Contains(type))
		{
			var buff = GetBuffObject(type);
			if(buff != null)
				buff.Start();
		}
	}


	
	protected virtual void ExecuteDebuff(DebuffType type)
	{
		if (_canDebuffHsh.Contains(type))
		{
			var debuff = GetDebuffObject(type);
			if(debuff != null)
				debuff.Start();
		}
	}
	#endregion


	#region QF
	public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }
	#endregion

}
