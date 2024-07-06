/****************************************************
    文件：CameraMoveOtherComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/6 19:30:8
	功能：
*****************************************************/

using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ISpeed;
using Random = UnityEngine.Random;

public class CameraMoveOtherComponent : MonoBehaviour
    , IUpdate ,ICanGetSystem
{


    #region 字属

    [SerializeField] private MoveOtherComponent _moveOtherCpt;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _moveGo;

    #endregion


    #region 生命




    public CameraMoveOtherComponent Init(Transform moveTrans, float cameraSpeed)
    {
        _moveGo = moveTrans.gameObject;
        _speed = cameraSpeed;
        //导弹自身还有另外的MoveComponent，所以不能GetOrAdd
        _moveOtherCpt = MoveOtherComponent.InitMoveComponentKeepDesption(_moveGo, gameObject, _moveOtherCpt, _speed, SpeedDes.CAMERASPEED);
        return this;
    }

    private void OnEnable()
    {
        _moveOtherCpt.DoIfNotNull(() =>
        {
            _moveOtherCpt.enabled = true;
        });//有的需要控制 
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
    }
    private void OnDisable()
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
        _moveOtherCpt.enabled = false;                 
    }


    private void OnDestroy()
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
        Destroy(_moveOtherCpt);
    }
    #endregion


    #region IUpadate
    public int Framing { get; set; }
    public int Frame { get; }
    public void FrameUpdate()
    {
        if (_moveOtherCpt.IsNullObject())
        {
            return;
        }
        _moveOtherCpt.Move(Vector2.up);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}



