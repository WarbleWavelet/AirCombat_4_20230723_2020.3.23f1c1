/****************************************************
    文件：CollideMsgFromPlaneComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/6 19:39:57
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>飞机本身自带的碰撞信息</summary>
public class CollideMsgFromPlaneComponent : MonoBehaviour, ICollideMsg, ICanSendCommand,IViewTransfrom 
{

    #region 字属


    private IDespawnCase _destroyCase;
   private IBullet[] _selfBullets;
    //
    InvincibleComponent _invincibleComponent;
    EInvincibleType _eInvincibleType;
    #endregion


    // Use this for initialization
    /// <summary>ShootCtrlsComponent=>ShootCtrl=>BulletModelComponent</summary>
    public CollideMsgFromPlaneComponent InitComponent(IBullet[] bullets, IDespawnCase destroyCase)
    {
        _selfBullets = bullets;
        _destroyCase = destroyCase;
        _eInvincibleType = EInvincibleType.CantInvincible;
        return this;
    }


    /// <summary></summary>
    public CollideMsgFromPlaneComponent InitComponent(IBullet bullet, IDespawnCase destroyCase,InvincibleComponent invincibleComponent=null)
    {
        _selfBullets = new IBullet[1];
        _selfBullets[0] = bullet;
        _destroyCase = destroyCase;
         _invincibleComponent=invincibleComponent;
        _eInvincibleType = EInvincibleType.CanInvincible;
        return this;
    }

    public void CollideMsg(Transform other)
    {
        if (ExtendJudge.IsOnceNull(_selfBullets, _destroyCase))
        {
            return;
        }
        if (_invincibleComponent != null && _invincibleComponent.Invincible)    // 闪避组件正在闪避
        {
            return;
        }
        // _invincibleComponent 是否存在
        for (int i = 0; i < _selfBullets.Length; i++)
        {
            this.SendCommand(new CollideMsgFromPlaneCommand(_destroyCase, _selfBullets[i], ViewTransfrom(), other, _invincibleComponent));

            //迷惑InvinciblepandaUN一个独享是否放在哪里最好
            //if (_eInvincibleType == EInvincibleType.CantInvincible) //
            //{
            //    this.SendCommand(new CollideMsgFromPlaneCommand(_destroyCase, _selfBullets[i], other));
            //}
            //else
            //{
            //    this.SendCommand(new CollideMsgFromPlaneCommand(_destroyCase, _selfBullets[i], other, _invincibleComponent.Invincible));
            //}
        }
    }

    #region 实现
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    public Transform ViewTransfrom()
    {
        return transform.parent;
    }


    private void OnDisable()
    {
        _invincibleComponent = null;
    }
    #endregion


}


