using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;
using SpeedDes=ISpeed.SpeedDes;


public interface ISpeed
{
    public enum SpeedDes
    {
        CAMERASPEED,
        PLANESPEED,
        BULLETSPEED,
        EFFECTSPEED,
        STARSPEED ,
        REWARDSSPEED,

    }
    void SetSpeed();
    float GetSpeed();   
    void SetSpeedDes();
    float GetSpeedDes();
}


#region IMoveComponent
public interface IMoveEntityComponent
{
    void SetEntity(Transform t);
    void Move(Vector2 dir);

    void SetMoveDesp(SpeedDes desp);
}
public interface IMoveComponent
{
    /// <summary>速度不变</summary>

    void Move(Vector2 dir);
    /// <summary>速度,方向都可能会变</summary>
    void Move(Vector2 dir, float speed);
    void SetMoveDesp(SpeedDes desp);
}

public class MoveSelfComponent : MonoBehaviour  ,IMoveComponent ,IInitParas
{
    [SerializeField] SpeedDes _desp;
    [SerializeField] float _speed;
    GameObject _entity;


    #region Init


    public void InitParas(params object[] os)
    {
        
        float speed = (float)os[0];
        _speed = speed;
        if (os.Length == 2)
        { 
            SpeedDes description = (SpeedDes)os[1];
            _desp = description;        
        }

    }



    #endregion

    #region InitMoveComponentKeepDesption


    /// <summary>保持自身挂在的desp组件唯一</summary>
    public static MoveSelfComponent InitMoveComponentKeepDesption(GameObject go, MoveSelfComponent moveCpt, float speed, SpeedDes desp)
    {
        if (go.IsNullObject()) // 子弹回收时又触发一次
            return null;
        
        MoveSelfComponent[] moves = go.GetComponents<MoveSelfComponent>();//GetOut
        if (moves.Length > 0)
        {
            foreach (var item in moves)
            {
                if (item.GetMoveDesp() == desp)
                {
                    moveCpt = item;
                }

            }
        }

        
        if (moveCpt.IsNullObject())//OrAdd
        {
            moveCpt = go.AddComponent<MoveSelfComponent>();
        }

        
        moveCpt.InitParas(speed, desp);//InitComponentEnemy

        return moveCpt;

    }



    public static MoveSelfComponent InitMoveComponentKeepDesption(GameObject go, MoveSelfComponent moveCpt, double speed, SpeedDes desp)
    {
       return  InitMoveComponentKeepDesption(go, moveCpt, (float)speed, desp);
    }

    #endregion

    #region pub
    public void Move(Vector2 dir)
    {
        if (_speed != 0 )
        {
            transform.Translate(dir * _speed * Time.deltaTime, Space.World);
        }
    }

    public void Move(Vector2 dir,float speed)
    {
        _speed = speed;
        Move(dir);
    }

    public void SetMoveDesp(SpeedDes desp)
    {
        _desp = desp;
    }
    public SpeedDes GetMoveDesp( )
    {
        return _desp;
    }
    #endregion




}


#endregion



