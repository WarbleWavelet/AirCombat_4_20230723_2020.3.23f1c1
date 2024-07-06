using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>Destroy时的爆炸图片</summary>
public class FrameAniComponent : MonoBehaviour,IUpdate ,ICanGetSystem
{
	private SpriteRenderer _sr;
	private Sprite[] _sprites;
	private int _index;
	private Action _cb;


	private void OnEnable()
	{
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
	}
    public void Init(Sprite[] sprites, Action callBack)
    {
        _index = 0;
        _sprites = sprites;
        _sr = gameObject.GetOrAddComponent<SpriteRenderer>();
        _cb = callBack;
    }

    private void OnDisable()
	{
		this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
	}




	#region pub IUpdate
	public int Framing { get; set; }
    public int Frame { get; }



    public void FrameUpdate()
	{
		if (_index < _sprites.Length)
		{
			_sr.sprite = _sprites[_index];
			_index++;
		}
		else
		{
			_cb.DoIfNotNull();
		}
	}
	#endregion


    public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }
}
