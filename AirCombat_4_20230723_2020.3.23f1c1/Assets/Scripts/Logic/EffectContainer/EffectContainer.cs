/****************************************************
    文件：EffectContainer.cs
	作者：lenovo
    邮箱: 
    日期：2024/6/30 14:15:5
	功能： EffectContainer集合
*****************************************************/

using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;




public static class SEffectContainerFactory
{
    public static IEffectContainer New(EItemType type)
    {
        switch (type)
        {
            case EItemType.STAR: return new StarEffectContainer();//属于必得奖励,不归ItemType管理
            case EItemType.ADD_EXP: return new DefaultEffectContainer();
            case EItemType.ADD_BULLET: return new DefaultEffectContainer();
            case EItemType.SHIELD: return new DefaultEffectContainer(); 
            case EItemType.POWER: return new DefaultEffectContainer();
            //TODO  :Boss1BulletEffectContainer
            // case Boss1BulletEffectContainer: return new StarEffectContainer();// 有交集,先放这里待处理
            default: return new DefaultEffectContainer();
        }
    }
}

#region EffectContainerBase : IEffectContainer


/// <summary>
/// 不破坏双边的命名习惯
///  <br/>01 原来叫  IEffectView(一不能挂节点,二和ItewView起冲突,一个节点两个View,觉得乱),
///   <br/>02 换成IEffectCalc ,  因为用了很多随机的缓动(大小,位置)
///   <br/>03 换成 IEffectContainer,因为里面时各种Effect的集合
/// </summary>
public interface IEffectContainer : IEffectEntity2D
{

}

public abstract class EffectContainerBase : IEffectContainer
{
    private IEffect[] _effects;
    private int _times;
    private Action _stopAction;
    protected Transform _transform;



    public virtual void Init(Transform transform)
    {
        _transform = transform;
        _effects = GetEffects(transform);
        _times = 0;
    }



    #region IEffect:IEffectLife


    public void Begin()
    {
        _effects.ForEach(effect => effect.Begin());
    }

    public void Stop(Action callBack)
    {
        _stopAction = callBack;
        _effects.ForEach(effect => effect.Stop(End));
    }

    public void Hide()
    {
        _effects.ForEach(effect => effect.Hide());
    }

    public void Clear()
    {
        _effects.ForEach(effect => effect.Clear());
    }
    #endregion

    #region pro

    protected abstract IEffect[] GetEffects(Transform transform);

    protected virtual void StopCallBack()
    {
        _stopAction.DoIfNotNull();
    }
    #endregion

    #region pri
    private void End()
    {
        _times++;
        if (_times == _effects.Length)
        {
            StopCallBack();
        }
    }


    #endregion
}

#endregion



#region BulletEffectContainerBase:EffectContainerBase二抽类

/// <summary>这种需要处理类型的Pool</summary>
public abstract class BulletEffectContainerBase : EffectContainerBase, IPoolable, IPoolType
{
    public abstract bool IsRecycled { get; set; }

    public abstract void OnRecycled();
    public abstract void Recycle2Cache();
}

#endregion



#region BulletEffectContainerBase实现类
public class Boss1BulletEffectContainer : BulletEffectContainerBase
{




    #region IPoolable  ，IPoolType

    //public static Boss1BulletEffectContainer Allocate()//System来持有
    //{
    //  return  SafeObjectPool<Boss1BulletEffectContainer>.Instance.Allocate();
    //}
    public override bool IsRecycled { get; set; }

    public override void OnRecycled()
    {

    }

    public override void Recycle2Cache()
    {
        SafeObjectPool<Boss1BulletEffectContainer>.Instance.Recycle(this);
    }
    #endregion


    protected override IEffect[] GetEffects(Transform transform)
    {
        IEffect[] effects = new IEffect[1];
        var rotate = new RotateEffect();
        rotate.Init(transform, -60);
        effects[0] = rotate;

        return effects;
    }
}

#endregion

#region EffectContainerBase实现类


public class StarEffectContainer : EffectContainerBase
{

    protected override IEffect[] GetEffects(Transform transform)
    {
        IEffect[] effects = new IEffect[3];

        ScaleEffect scaleEffect = new ScaleEffect();
        Vector3 max = transform.localScale * 1.2f;
        scaleEffect.Init(transform, Vector3.zero, max, 0.5f, -1, LoopType.Yoyo);
        effects[0] = scaleEffect;

        SlowSpeedEffect ySlow = new SlowSpeedEffect();
        ySlow.Init(transform, Vector2.up, (1f, 4f).RR(), (2f, 4f).RR(), -10);
        effects[1] = ySlow;
        //
        SlowSpeedEffect xSlow = new SlowSpeedEffect();
        float startSpeed = (-0.5f, 0.5f).RR();
        float slowSpeed = 0;
        slowSpeed = (startSpeed > 0) ? (0.3f, 1f).RR() : (-0.3f, -1f).RR();
        xSlow.Init(transform, Vector2.right, startSpeed, slowSpeed, 0);
        effects[2] = xSlow;

        return effects;
    }


    protected override void StopCallBack()
    {
        FlyIntoUIEffect effect = new FlyIntoUIEffect();
        effect.Init(_transform, 0.8f, ResourcesPath.PREFAB_GAME_UI_PANEL, GameObjectPath.RightTopPin_Star_Value, () => base.StopCallBack());
        effect.Begin();

    }
}



/// <summary>
/// 原来ADD_EXP, ADD_BULLET两个类都是一样的类体函数,参数完全没变,
/// 直接统一default</summary>
public class DefaultEffectContainer : EffectContainerBase
{
    protected override IEffect[] GetEffects(Transform transform)
    {
        IEffect[] effects = new IEffect[1];

        SlowSpeedEffect ySlow = new SlowSpeedEffect();
        ySlow.Init(transform, Vector2.up, 0, (2f, 4f).RR(), -5f);
        effects[0] = ySlow;

        return effects;
    }
}




public class MissileWarningEffectContainer : EffectContainerBase
{
    private int _numOfWarning;
    private float _eachWarningTime;
    private Action _endAction;

    public void Init(Transform transform, int numOfWarning, float eachWarningTime, Action endAction)
    {
        _numOfWarning = numOfWarning;
        _eachWarningTime = eachWarningTime;
        _endAction = endAction;
        base.Init(transform);
    }

    protected override IEffect[] GetEffects(Transform transform)
    {
        if (_eachWarningTime == 0 || _numOfWarning == 0)
        {
            Debug.LogError("当前组件未初始化");
            return null;
        }

        IEffect[] effects = new IEffect[1];
        ShowAndHideEffect effect = new ShowAndHideEffect();
        effect.Init(transform, _eachWarningTime, _numOfWarning, _endAction);

        effects[0] = effect;

        return effects;
    }
}





#endregion

