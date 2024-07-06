using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeedDes = ISpeed.SpeedDes;

public class SlowSpeedEffect : IEffect,IUpdate ,ICanGetSystem
{
    private float _slowSpeed;
    private float _speedLimit;
    private MoveOtherComponent _move;
    private float _speed;
    private Vector2 _axis;




    #region IEffect


    public void Init(Transform trans,Vector2 axis,float startSpeed,float slowSpeed,float speedLimit)
    {
        _axis = axis;
        _speed = startSpeed;
        _slowSpeed = slowSpeed;
        _speedLimit = speedLimit;
        _move = MoveOtherComponent.InitMoveComponentKeepDesption(trans, trans.FindOrNew(GameObjectName.Move), _move, _speed,SpeedDes.EFFECTSPEED);
    }
    
    public void Begin()
    {
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
    }

    public void Stop(Action cb)
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
        cb.DoIfNotNull();
    }

    public void Hide()
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
    }

    public void Clear()
    {
        _speed = 0;
        _slowSpeed = 0;
        _speedLimit = 0;
    }
    #endregion


    #region IUpdate
    public int Framing { get; set; }
    public int Frame { get; }
    public void FrameUpdate()
    {
        if (_move == null)
        { 
            return;
        }
        _speed = (_speed < _speedLimit)   ? _speedLimit  : (_speed-_slowSpeed * UnityEngine.Time.deltaTime);
        _move.InitSpeed(_speed);
        _move.Move(_axis);
    }
    #endregion


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
