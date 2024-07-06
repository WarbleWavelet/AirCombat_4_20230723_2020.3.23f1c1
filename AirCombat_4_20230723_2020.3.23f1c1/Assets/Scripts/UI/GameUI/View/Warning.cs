using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;
using UnityEngine.UI;

public class Warning : MonoBehaviour ,ICanGetSystem
{

	private Image _icon;
	private Text _content;
	private Color _defaultIconColor;
	private Color _defaultContentColor;
	private Color _targetColor;
	private float _fadeTime;



	#region 生命
	private void Awake()
	{
		_fadeTime = 0.5f;
		_icon = GetComponentInChildren<Image>();
		_content = GetComponentInChildren<Text>();
		_defaultIconColor = _icon.color;
		_defaultContentColor = _content.color;
		_defaultIconColor.a = 0;
		_defaultContentColor.a = 0;
		_targetColor = new Color(1,1,0,1);
	}

	   
	public void OnEnable()
	{
		Init();

		ShowAni();
		
		this.GetSystem<ICoroutineSystem>().StartDelay(Const.WAIT_BOSS_TIME - _fadeTime,HideAni);
		this.GetSystem<IAudioSystem>().PlaySound(AudioGame.Effect_Boss_Warning);
	}

    private void OnDisable()
    {
        this.GetSystem<IAudioSystem>().Stop(AudioGame.Effect_Boss_Warning.ToString());
    }
	#endregion



	#region pri
	private void Init()
	{
		_icon.color = _defaultIconColor;
		_content.color = _defaultContentColor;
		_icon.DOKill();
		_content.DOKill();
	}

	private void ShowAni()
	{
		_icon.DOFade(1, _fadeTime);
		_content
			.DOFade(1, _fadeTime)
			.OnComplete(ContentAni);
	}

	private void ContentAni()
	{
		_content
			.DOColor(_targetColor, 1)
			.SetLoops(-1,LoopType.Yoyo);
	}

	private void HideAni()
	{
		_icon.DOFade(0, _fadeTime);
		_content
			.DOFade(0, _fadeTime)
			.OnComplete(()=>Destroy(gameObject));
	}


	#endregion



    public IArchitecture GetArchitecture()
    {
		return AirCombatApp.Interface;
    }
}
