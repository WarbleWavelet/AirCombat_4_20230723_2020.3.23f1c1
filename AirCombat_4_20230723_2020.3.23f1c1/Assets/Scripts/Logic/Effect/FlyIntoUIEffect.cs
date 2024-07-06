using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class FlyIntoUIEffect : IEffect,ICanGetUtility ,ICanGetSystem
{
    
    private Transform _starUi;
    private float _cameraSpeed;
    private Transform _transform;
    private float _duration;
    private Action _callBack;


    #region IAniLife


    public void Init(Transform transform,float duration ,string uiPath,string childPath,Action callBack)
    {
        _transform = transform;
        _duration = duration;
        _callBack = callBack;
        RectTransform panel = this.GetSystem<IUISystem>().GetViwePrefab(uiPath);
        _starUi = panel.FindCurChildPath(childPath);
		
        var reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_GAME_CONFIG);
        reader[ReaderKey.cameraSpeed].Get<float>(value => { _cameraSpeed = value; });
    }
    public void Begin()
    {
        var pos = Camera.main.ScreenToWorldPoint(_starUi.position) + Vector3.up*_cameraSpeed*_duration;
        _transform.DOKill();
        _transform.DOMove(pos, _duration);
        _transform.DOScale(Vector3.zero, _duration).OnComplete(() =>
        {
            _callBack.DoIfNotNull();
        });
    }

    public void Stop(Action cb)
    {
        cb.DoIfNotNull();
    }

    public void Hide()
    {
    }

    public void Clear()
    {
        _transform = null;
        _duration = 0;
        _starUi = null;
    }


    #endregion

    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
