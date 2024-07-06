using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UniRx.Async.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class EnemyLifeItem : SetPosZByLayerLevelView	 ,ICanGetSystem
{

	[SerializeField]	MessageMgrComponent _messageMgrComponent;
    /// <summary>不确定InitComponent是否跑了</summary>
    [SerializeField] bool _initComponent=false;
	[SerializeField] int _minHp=0;
	public override Entity2DLayer E_Entity2DLayer
	{
		get
		{
			return Entity2DLayer.EFFECT;//层级Posz在Effect上,腹肌诶单不在Effect上
		}
	}




    public void Init(int eachLife)
    {
        _messageMgrComponent = transform.GetComponentInParentRecent<MessageMgrComponent>();
        _messageMgrComponent.AddListener(MsgEvent.EVENT_HP, UpdateLife);
		//
        //比如2000血分成10块,那就是200血/块
        _minHp = transform.GetSiblingIndex() * eachLife;//第3节点第3块,就是600血
		_initComponent = true;
    }

    private void OnDisable()
    {

        _messageMgrComponent.RemoveListener(MsgEvent.EVENT_HP, UpdateLife);
		_initComponent = false;
		
    }

    #region pri


    private void UpdateLife(object[] paras)
	{
		int life = 0;
		int lifeMax = 0;
		if (paras.Length == 2)
		{
			life = paras[0].Get<int>();
			lifeMax = paras[1].Get<int>();
		}
		else
		{
			Debug.LogError("更新生命消息部分，参数不能为空");
			return;
		}

		if (life <= _minHp)//第3节点第3块,就是600血,小于600血,就隐藏
        {
			gameObject.Hide();
		}
	}


	#endregion

}
