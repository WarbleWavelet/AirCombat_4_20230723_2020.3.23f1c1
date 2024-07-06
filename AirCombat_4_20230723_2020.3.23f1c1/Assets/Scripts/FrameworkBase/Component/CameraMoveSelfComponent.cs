using QFramework;
using QFramework.AirCombat;
using System;
using UnityEngine;
using SpeedDes = ISpeed.SpeedDes;


/// <summary>01必须调用Init初始化好标签
/// <para/> 02本质是一个MoveComponent.InitMoveComponentKeepDesption(就是常用的那个)来同步相机的基础向上移动
/// <br/> 给了相机和相关移动的物体
/// </summary>
public class CameraMoveSelfComponent : MonoBehaviour
    , IUpdate,IHide   ,
    ICanGetSystem
{


    #region 字属

    [SerializeField]   private MoveSelfComponent _moveCpt;
     [SerializeField] private float _speed;
    [SerializeField] private GameObject _entity;

    #endregion


    #region 生命

    private void OnEnable()
    {
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
     
    }


    /// <summary>此时已经保证有相机速度</summary>
    public CameraMoveSelfComponent InitComponent(float cameraSpeed)
    {

        _speed = cameraSpeed;
        //导弹自身还有另外的MoveComponent，所以不能GetOrAdd
        _moveCpt = MoveSelfComponent.InitMoveComponentKeepDesption(gameObject,_moveCpt,_speed, SpeedDes.CAMERASPEED);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);

        return this;        
    }

    public CameraMoveSelfComponent Init(Transform t, float cameraSpeed)
    {
        _entity = t.gameObject;
        _speed = cameraSpeed;
        //导弹自身还有另外的MoveComponent，所以不能GetOrAdd
        _moveCpt = MoveSelfComponent.InitMoveComponentKeepDesption(_entity, _moveCpt, _speed, SpeedDes.CAMERASPEED);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);

        return this;
    }

    private void OnDestroy()
    {
        Destroy(_moveCpt);
    }
    #endregion


    #region IUpadate
    public int Framing { get; set; }
    public int Frame { get; }
    public void FrameUpdate()
    {
        if (_moveCpt.IsNullObject())
        {
            return;
        }
        _moveCpt.Move(Vector2.up);
    }
    #endregion   
    

    #region 重写

    public void Hide()
    {
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}



