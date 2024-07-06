/****************************************************
    文件：ADWSMoveOtherComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/1/14 19:6:56
	功能：
*****************************************************/

using DG.Tweening;
using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using SpeedDes = ISpeed.SpeedDes;
 

public class ADWSMoveOtherComponent : MonoBehaviour ,ICanSendCommand ,IUpdate  ,ICanGetSystem 
  
{

    [SerializeField]  float _speed;
    [SerializeField] MoveOtherComponent _moveCpt;
    [SerializeField] SpriteRenderer _sr;



    /// <summary>挂在子节点下的方式</summary>
    public ADWSMoveOtherComponent Init(Transform t,float speed)
    {
        GameObject plane = t.gameObject;
        _sr = plane.GetComponent<SpriteRenderer>();
        _speed = speed;
        _moveCpt = MoveOtherComponent.InitMoveComponentKeepDesption(plane, gameObject, _moveCpt, _speed, SpeedDes.PLANESPEED);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
        return this;
    }

    #region IUpdate
    public int Framing { get; set; }
    public int Frame { get; }
    public void FrameUpdate()
    {
        UpdateMove();
    }
    #endregion


    #region pri
    void UpdateMove()
    {
        if (_moveCpt.IsNullObject())
        {
            return;
        }
        float easeTime = 1.0f;
        float poseAngle = 50f;
        Vector3 poseAngleAdapt=Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            this.SendCommand(new MoveComponentForSpriteRendererCommand(_moveCpt, Vector2.up, _sr));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            poseAngleAdapt.AddX(-poseAngle);
            this.SendCommand(new MoveComponentForSpriteRendererCommand(_moveCpt, Vector2.down, _sr));
        }
        if (Input.GetKey(KeyCode.A))
        {
            poseAngleAdapt.AddY(-poseAngle);
            this.SendCommand(new MoveComponentForSpriteRendererCommand(_moveCpt, Vector2.left, _sr));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            poseAngleAdapt.AddY(poseAngle);
            this.SendCommand(new MoveComponentForSpriteRendererCommand(_moveCpt, Vector2.right, _sr));
        }
        if (poseAngleAdapt != Vector3.zero)
        {
            transform.parent.DOLocalRotate(poseAngleAdapt, easeTime);

        }
        else
        {
            transform.parent.DOLocalRotate(Vector3.zero, easeTime);
        }
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}



