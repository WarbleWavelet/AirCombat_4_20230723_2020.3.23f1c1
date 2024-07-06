using System;
using DG.Tweening;
using UnityEngine;

public class ScaleEffect : IEffect 
{
    private Vector3 _defaultScale;
    private Transform _transform;
    private Vector3 _toScale, _fromScale;
    private float _duration;
    private int _loopTimes;
    private LoopType _loopType;



    #region й╣ож      
    public void Init(Transform transform,Vector3 fromScale,Vector3 toScale,float duration,int loopTimes,LoopType loopType)
    {
        _transform = transform;
        _defaultScale = transform.localScale;
        _toScale = toScale;
        _fromScale = fromScale;
        _duration = duration;
        _loopTimes = loopTimes;
        _loopType = loopType;
    }
    public void Begin()
    {
        _transform.localScale = _fromScale;
        _transform
            .DOScale(_defaultScale, _duration*0.6f)
            .OnComplete(Idle);
    }

    public void Stop(Action onStop)
    {
        onStop.DoIfNotNull();
        _transform.localScale = _defaultScale;
    }

    public void Hide()
    {
        _transform.localScale = _defaultScale;
    }

    public void Clear()
    {
        _transform = null;
    }
    #endregion  


    private void Idle()
    {
        _transform
            .DOScale(_toScale, _duration)
            .SetLoops(_loopTimes,_loopType);
    }


}