/****************************************************
    文件：MoveOtherComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/6 19:31:15
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ISpeed;
using Random = UnityEngine.Random;


/// <summary>
/// 相对于组件所在节点的Other
/// <br/>组件所在节点和移动节点不是同一个
/// </summary>
public class MoveOtherComponent : MonoBehaviour, IMoveComponent
{
    [SerializeField] SpeedDes _desp;
    [SerializeField] float _speed;
    [SerializeField] GameObject _moveGo;


    #region Init


    public MoveOtherComponent Init(float speed, SpeedDes speedDes, GameObject moveGo)
    {
        _speed = speed;
        _desp = speedDes;
        _moveGo = moveGo;
        return this;
    }

    private void Start()
    {
            
    }

    #endregion

    #region InitMoveComponentKeepDesption


    /// <summary>
    /// 保持自身挂在的desp组件唯一
    /// </summary>
    /// <param name="moveGo"></param>
    /// <param name="cptGo"></param>
    /// <param name="moveCpt"></param>
    /// <param name="speed"></param>
    /// <param name="desp"></param>
    /// <returns></returns>
    public static MoveOtherComponent InitMoveComponentKeepDesption(GameObject moveGo, GameObject cptGo, MoveOtherComponent moveCpt, float speed, SpeedDes desp)
    {
        if (ExtendJudge.IsOnceNullObject(moveGo, cptGo)) // 子弹回收时又触发一次
        {


            Debug.LogError("空,MoveOtherComponent InitMoveComponentKeepDesption");
            return null;
        
        }

        MoveOtherComponent[] moves = cptGo.GetComponents<MoveOtherComponent>();//GetOut
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
            moveCpt = cptGo.AddComponent<MoveOtherComponent>();
        }


        moveCpt.Init(speed, desp, moveGo);//InitComponentEnemy

        return moveCpt;

    }



    public static MoveOtherComponent InitMoveComponentKeepDesption(GameObject go, MoveOtherComponent moveCpt, double speed, SpeedDes desp)
    {
        return InitMoveComponentKeepDesption(go, moveCpt, (float)speed, desp);
    }


    public static MoveOtherComponent InitMoveComponentKeepDesption(Transform moveTrans, Transform cptTrans, MoveOtherComponent moveCpt, float speed, SpeedDes desp)
    {
        return InitMoveComponentKeepDesption(moveTrans.gameObject, cptTrans.gameObject, moveCpt, speed, desp);
    }


    #endregion

    #region pub
    public void Move(Vector2 dir)
    {
        if (_moveGo.IsNullObject())
        {
            return;
        }

        if (_speed != 0)
        {
            _moveGo.transform.Translate(dir * _speed * Time.deltaTime, Space.World);
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
    public SpeedDes GetMoveDesp()
    {
        return _desp;
    }
    public void InitSpeed(float speed)
    {
        _speed = speed;
    }

    #endregion




}



