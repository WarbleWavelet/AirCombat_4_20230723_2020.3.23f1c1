using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>Boss血量比率触发事件</summary>
public class BulletBossEventComponent  : MonoBehaviour,ICanGetSystem,ICanGetUtility 
{

    [SerializeField]   private IEnemyBossBulletModel _enemyBossBulletModel;
    [SerializeField] private MessageMgrComponent _messageMgrComponent;



    public BulletBossEventComponent Init(IBulletModel model)
    {
        if (model is IEnemyBossBulletModel)
        {
            _enemyBossBulletModel = model as IEnemyBossBulletModel;
            _messageMgrComponent = transform.GetComponentInParentRecent<MessageMgrComponent>();
            _messageMgrComponent.AddListener(MsgEvent.EVENT_HP,ReceiveLifeMsg);
        }

        return this;
    }

    private void ReceiveLifeMsg(object[] os)
    {
        int life = os[0].Get<int>();
        int lifeMax = os[1].Get<int>();
        float ratio = 1;
        if (lifeMax != 0)
        {
            ratio = life / (float) lifeMax;
        }
        
        _enemyBossBulletModel.UpdateEvent(ratio);
        if (life == 0)
        {
            _messageMgrComponent.RemoveListener(MsgEvent.EVENT_HP, ReceiveLifeMsg);
        }
    }


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
