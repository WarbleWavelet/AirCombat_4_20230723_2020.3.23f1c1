using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : IEffect,IUpdate,ICanGetSystem
{
    private float _offsetAngle;
    private Transform _transform;
    public int Framing { get; set; }
    public int Frame { get; }


    #region 实现
    public void Init(Transform transform, float offsetAngle)
    {
        _offsetAngle = offsetAngle;
        _transform = transform;
    }
    
    public void Begin()
    {
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
    }

    public void Stop(Action callBack)
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
        Clear();
    }

    public void Hide()
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
        Clear();
    }

    public void Clear()
    {
        _offsetAngle = 0;
        _transform = null;
    }

    public void FrameUpdate()
    {
        if(_transform == null)
            return;

        _transform.Rotate(Vector3.forward * _offsetAngle * UnityEngine.Time.deltaTime);
    }
    #endregion

    	 public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface; 
    }

  


}
