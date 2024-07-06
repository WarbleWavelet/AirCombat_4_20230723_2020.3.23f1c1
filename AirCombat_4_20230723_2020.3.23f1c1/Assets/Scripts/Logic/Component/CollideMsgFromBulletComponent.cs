/****************************************************
    文件：CollideMsgFromBulletComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/6 19:40:25
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>Go会受子弹的</summary>
public class CollideMsgFromBulletComponent : MonoBehaviour, ICollideMsg, ICanSendCommand
{
    private IDespawnCase _despawnBulletCase;
    private IBullet _bullet;
    //private SubMsgMgr _msgMgr;

    // Use this for initialization
    public CollideMsgFromBulletComponent InitComponent(IBullet bullet, IDespawnCase despawnBulletCase)
    {
        _bullet = bullet;
        _despawnBulletCase = despawnBulletCase;
        _bullet.CkeckNull();
        _despawnBulletCase.CkeckNull();
        return this;
    }

    public void CollideMsg(Transform other)
    {
        this.SendCommand(new CollideMsgFromBulletCommand(_despawnBulletCase, _bullet, other));
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}


