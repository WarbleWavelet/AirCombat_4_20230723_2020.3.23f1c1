using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileWarningView : PlaneLevelView
{
	private Action _endAction;
	private MissileWarningEffectContainer _effect;

	public void Init(float x,int numOfWarning,float eachWarningTime)
	{
		base.Init();
		InitPos(x);
		_effect=_effect.NewIfNull();
		 //
        _effect.Init(transform,numOfWarning,eachWarningTime,_endAction);
		_effect.Begin();
	}

	private void InitPos(float x)
	{
            float cameraPosY = transform.MainOrOtherCamera().transform.position.y;
            transform.SetPosXY(x, cameraPosY);
	}

	public void AddEndListener(Action endAction)
	{
		_endAction = endAction;
	}
}
