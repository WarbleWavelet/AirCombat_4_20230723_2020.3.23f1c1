using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class ShowAndHideEffect : IEffect
{
	private SpriteRenderer _sr;
	private float _duration;
	private int _executeTimes;
	private Action _onEnd;
	private int _timing;



    #region IEffectLife


    public void Init(Transform transform,float duration,int executeTimes,Action endCallback)
	{
		_timing = 0;
		_sr = transform.GetComponent<SpriteRenderer>();
		_duration = duration;
		_executeTimes = executeTimes;
		_onEnd = endCallback;
	}

	public void Begin()
	{
		if (_sr == null)
		{
			Debug.LogError("当前组件未初始化");
			return;
		}

        _sr.SetAlpha(0);
        _sr.DOFade(1, _duration / 2)
			.OnComplete(Hide);
	}



	public void Stop(Action callBack)
	{
		_sr.SetAlpha(0);
		callBack.DoIfNotNull();
	}

	public void Hide()
	{
		if (_sr == null)
		{
			Debug.LogError("当前组件未初始化");
			return;
		}
		
		_sr.DOFade(0f, _duration / 2).OnComplete(() =>
		{
			_timing++;
			if (_timing == _executeTimes)
			{
				_onEnd.DoIfNotNull();
			}
			else if (_timing < _executeTimes)
			{
				Begin();
			}
		});
	}

	public void Clear()
	{
		_onEnd = null;
		_duration = 0;
		_sr = null;
	}
	#endregion	

}
