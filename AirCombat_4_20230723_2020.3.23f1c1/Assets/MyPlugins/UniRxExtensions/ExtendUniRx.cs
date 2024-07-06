/****************************************************
    文件：UniRxExtensions.cs
	作者：lenovo
    邮箱: 
    日期：2023/6/6 17:55:35
	功能：
    来源：https://www.bilibili.com/video/BV1EB4y1z7nY/?spm_id_from=333.337.search-card.all.click&vd_source=54db9dcba32c4988ccd3eddc7070f140
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
//需要这两个
using UniRx;
//using UniRx.Async;
using UniRx.Triggers;
using UnityEngine.Events;
using QFramework;


/*
 *  AddTo，同生共死
 *  Subject，一个参数，多个用元组
 *  Where==if，过滤
 *  First，第一个才有效
 *  括号里面的方法可以写 _=> ()=> 形参(比如unit,list)=>
 *  buffer，事件缓存，事件存一块可以一起输出
 *  01 go.UpdateAsObservable() ；02 Observable.EveryUpdate().AddTo(this) 。01的好处是自动AddTo(go)上
 *  x协程返回IEnumerator，无法适应TryCatch
 *  ReactiveProperty，常用于UI的MVC
 *  SelectMany
 */

public static partial class ExtendUniRx
{

    #region 生命


    /// <summary>首次载入且Go激活</summary>
    public static void ExampleMonoBehaviour(this MonoBehaviour mono)
    {
        if (false)
        { 
            mono.DoAfterTime();
            mono.DoWhenUpdate();
            mono.gameObject.DoTimer(2f);        
        }

    }

    public static void ExampleGameObject(this GameObject go, MonoBehaviour mono)
    {
        if (false)
        { 
            go.DoWhenHide();
            go.DoWhenDestroy();
            go.DoWhenMouseDown();
            go.DoDelayMouseDown(2f);
            go.DoDelayFrameMouseDown(2);
            go.DoDelayMouseDownByUpdate(2f);
            go.DoWhenCollide();
            go.DoUpdate();
            go.DoTimer(1f);
            IDisposable disposable = go.DoUpdateDispose();//ifxxx，操作disposable
        }

       

    }

    public static void BindInt(this ReactiveProperty<int> num,MonoBehaviour mono) 
    {
        num   
            .Skip(1)//跳过第一次的赋值初始化阶段
            .Subscribe(_ =>Debug.Log("ValueChange"))
            .AddTo(mono);
    }


    #endregion


    #region 辅助


    #region When
    public static void DoWhenHide(this GameObject go)
    {
        go.DoWhenDisable();
    }

    public static void DoWhenDisable(this GameObject go)
    {
        go
          .OnDisableAsObservable()
          .Subscribe(_ => Debug.Log("DoWhenHide"))
          .AddTo(go);
    }

    public static void DoWhenDisable(this MonoBehaviour mono, Action action)
    {
        mono
          .OnDisableAsObservable()
          .Subscribe(_ => action())
          .AddTo(mono);
    }

    public static void DoWhenDestroy(this MonoBehaviour mono,Action action)
    {
        mono
          .OnDestroyAsObservable()
          .Subscribe(_=>action())
          .AddTo(mono);
    }

    public static void DoWhenDestroy(this GameObject go)
    {
        go
          .OnDestroyAsObservable()
          .Subscribe(_ => Debug.Log("DoWhenHide"))
          .AddTo(go);
    }

    public static void DoWhenMouseDown(this GameObject go)
    {
        go
          .OnMouseDownAsObservable()
          .Subscribe(_ => Debug.Log("DoWhenMouseDown"))
          .AddTo(go);
    }       
    public static void DoWhenCollide(this GameObject go)
    {
        go
          .OnCollisionEnterAsObservable()
          .Subscribe(_ => Debug.Log("DoWhenCollide"))
          .AddTo(go);
    }
    #endregion


    #region Delay
    public static void DoDelayMouseDown(this GameObject go, float delay)
    {
        go
          .OnMouseDownAsObservable()
          .Delay(TimeSpan.FromSeconds(delay))
          .Subscribe(_ => Debug.Log("DoDelayMouseDown"))
          .AddTo(go);
    }
    public static void DoDelayFrameMouseDown(this GameObject go, int cnt)
    {
        go
          .OnMouseDownAsObservable()
          .DelayFrame( cnt )
          .Subscribe(_ => Debug.Log("DoDelayMouseDown"))
          .AddTo(go);
    }



    public static void DoDelayMouseDownByUpdate(this GameObject go
        , float delay)
    {
        go
          .UpdateAsObservable()
          .Where( _=>
          {
              if (Input.GetMouseButtonDown(0))
              {
                  Debug.Log("DoDelayMouseDownByUpdate点击鼠标左键");
                  return true;
              }
              return false;
          })
          .Delay(TimeSpan.FromSeconds(delay))
          .Subscribe(_ => Debug.Log("DoDelayMouseDownByUpdate"))
          .AddTo(go);
    }
    #endregion


    #region Update
    public static MonoBehaviour DoWhenUpdate(this MonoBehaviour mono)
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => Debug.Log("DoUpdate"))
            .AddTo(mono);
        return mono;
    }
    public static MonoBehaviour DoWhenUpdate(this MonoBehaviour mono,Action action)
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => action())
            .AddTo(mono)
            .DisposeWhenGameObjectDestroyed(mono); //QF
        return mono;
    }

    public static void DoUpdate(this GameObject go)
    {
        go
            .UpdateAsObservable()
            .Subscribe(_ => Debug.Log("DoUpdate"))
            .AddTo(go);
    }

    /// <summary>一般用于if(xxx){disposable.Dispose();}</summary>
    public static IDisposable DoUpdateDispose(this GameObject go)
    {
        IDisposable disposable = go
           .UpdateAsObservable()
           .Subscribe(_ => Debug.Log("DoUpdateDispose"))
           .AddTo(go);

        return disposable;
    }

    /// <summary>一般用于if(xxx){disposable.Dispose();}</summary>
    public static IDisposable DoUpdateDispose(this GameObject go, CompositeDisposable compositeDisposable)
    {
        IDisposable disposable = go
           .UpdateAsObservable()
           .Subscribe(_ => Debug.Log("DoUpdateDispose"))
           .AddTo(compositeDisposable);

        return disposable;
    }
    #endregion


    #region 计时 Timer 一次性


    public static MonoBehaviour DoAfterTime(this MonoBehaviour mono)
    {
        Observable
            .Timer(TimeSpan.FromSeconds(2f))
            .Subscribe(_ => Debug.Log("DoAfterTime"))
            .AddTo(mono);
        return mono;
    }

    public static void DoAfterTime(this MonoBehaviour mono,float delay, UnityAction action)
    {
        Observable
            .Timer(TimeSpan.FromSeconds(2f))
            .Subscribe(_ => action())
            .AddTo(mono);
    }


    /// <summary>每隔一秒输出 ticked，当把该脚本所在的 GameObject 隐藏，则停止输出</summary>
    public static void DoTimerWhenDisable(this GameObject go, float spanTime, UnityAction action)
    {
        Observable.Timer(TimeSpan.FromSeconds(spanTime))
        .RepeatUntilDisable(go)
        .Subscribe(_=>action())
        .AddTo(go);
    }
    #endregion  
    #endregion


}


#region Sample 采样 一直跑
public static partial class ExtendUniRx  
{

    public static void DoTimer(this GameObject go, float time,UnityAction unityAction = null)
    {
        if (unityAction == null)
        {
            unityAction += ()=> Debug.Log("DoTimer");
        }
        go
            .UpdateAsObservable()
            .Sample(TimeSpan.FromSeconds(time))
            .Subscribe(_=>unityAction())
            .AddTo(go);
    }


    /// <summary>每隔2秒</summary>
    public static void DoTimer(this MonoBehaviour mono,float time, UnityAction unityAction=null)
    {
        GameObject go = mono.gameObject;
        go.DoTimer(time, unityAction);
    }


    public static void DoTimer(this Transform t, float time, UnityAction unityAction = null)
    {
        GameObject go = t.gameObject;
        go.DoTimer(time,unityAction);
    }
}

#endregion




