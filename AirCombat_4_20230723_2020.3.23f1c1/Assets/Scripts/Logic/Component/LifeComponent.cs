using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>给敌人的，因为玩家的在UI上
/// 属性加了事件发送</summary>
public class LifeComponent : MonoBehaviour  
{
    #region 字属



    [SerializeField]  private int _life;
    [SerializeField]  private MessageMgrComponent _messageMgrComponent;
                                           
    public int Life
    {
        get { return _life; }
        set
        {
            _life = value;
            _messageMgrComponent.SendMsg(MsgEvent.EVENT_HP,_life, _lifeMax);
        }
    }

    [SerializeField] int _lifeMax;
    public int LifeMax
    {
        get { return _lifeMax; }
    }

    #endregion  


    public LifeComponent Init(int life)
    {
        _messageMgrComponent = transform.GetComponentInParentRecent<MessageMgrComponent>();
        Life = life;
        _lifeMax = life;
        return this;
    }
}
