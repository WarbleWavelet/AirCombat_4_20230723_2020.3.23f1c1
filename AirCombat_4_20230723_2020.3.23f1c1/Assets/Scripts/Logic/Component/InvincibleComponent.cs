/****************************************************
	文件：InvincibleComponent.cs
	作者：lenovo
	邮箱: 
	日期：2024/7/3 15:34:14
	功能：
*****************************************************/

using DG.Tweening;
using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class InvincibleComponent : MonoBehaviour  ,IInitParas<InvincibleComponent> ,ICanGetSystem  ,IUpdate	 ,ICanGetModel
{
	#region 属性
	//[SerializeField] private bool _invincible;
	public bool Invincible { get => this.GetModel<IAirCombatAppStateModel>().Invincible; set => this.GetModel<IAirCombatAppStateModel>().Invincible.Value = value; }

	[SerializeField] private float _invincibleTimer;
	[SerializeField] private float _invincibleTime;
	[SerializeField] private SpriteRenderer _sr;
	/// <summary>有点像状态机的Enter</summary>
	[SerializeField] private bool _onInvincibleEnter;
	[SerializeField] private bool _fade;
	   #endregion
	#region 生命
	public InvincibleComponent InitParas(params object[] os)
	{
		if (os != null && os.Length == 2)
		{
			_invincibleTime = (float)os[0];
			_sr = (SpriteRenderer)os[1];
			Invincible =false;
			_onInvincibleEnter = false;
			this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
			return this;
		}

		throw new System.Exception("异常:参数");
	}

	public int Framing { get; set; }
	public int Frame { get; }

	float t2;
	public void FrameUpdate()
	{ 
		if (Invincible)//霸体就进行计时
		{
			if (!_onInvincibleEnter)  //相当OnStateEnter
            { 
				//int loopCnt = 1;
				//float t1 = _invincibleTime ;   //时间或者帧数
				// t2 = (t1/ loopCnt) / 2f;//来回
				_sr.DOFade(0.5f, 0.1f) ;
				_onInvincibleEnter = true;
			}

			//
			_invincibleTimer = this.Timer(_invincibleTimer, _invincibleTime, () =>
			{
				//相当OnStateExit
				_invincibleTimer = 0f;
                Invincible = false;
				_onInvincibleEnter = false;
				_sr.DOFade(1f, 0.1f);
			});

		}  
	}


	#endregion



	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
}



