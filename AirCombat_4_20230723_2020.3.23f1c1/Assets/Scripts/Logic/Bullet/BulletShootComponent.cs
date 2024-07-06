/****************************************************
    文件：BulletShootComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/7 14:15:42
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletShootComponent : MonoBehaviour, QFramework.IController
{
    #region 字属
    /// <summary>射击间隔。也就是装弹间隔</summary>
    [SerializeField] private float _shooting;
    public float Shooting { get => _shooting; set => _shooting = value; }
    [SerializeField] private float _shoot;
    public float Shoot { get => _shoot; set => _shoot = value; }
    //
    [SerializeField] private bool _canShoot = false;
    public bool CanShoot { get => _canShoot; set => _canShoot = value; }

    /// <summary>子弹射出时的初始位置，随飞机变化。不是子弹的父节点</summary>
    [SerializeField] Transform _muzzleTrans;
    [SerializeField] private float _bulletSpeed;
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }

    [SerializeField] BulletType _bulletType;
    [SerializeField] IBulletModel _bulletModel;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] float _srBoundsSizeX;

    [SerializeField] BulletPointsCalcEllipseComponent _bulletPointSpawnComponent;
    [SerializeField] string _despawnKey;
    #endregion


    public BulletShootComponent InitComponent(Transform bulletRoot, BulletType bulletType, IBulletModel bulletModel, BulletPointsCalcEllipseComponent bulletPointSpawnComponent,SpriteRenderer sr, string despawnKey)
    {
        _despawnKey = despawnKey;
        _muzzleTrans = bulletRoot;
        _bulletType = bulletType;
        _bulletModel = bulletModel;
        _bulletPointSpawnComponent = bulletPointSpawnComponent;
        _sr = sr;
        _srBoundsSizeX = _sr.BoundsSizeX();
        return this;
    }


    public bool Check()
    {
        if (ExtendJudge.IsOnceNull(_muzzleTrans) || _bulletSpeed == 0)
            return false;
        return true;
    }
    public bool ShootTimer()
    {
        _shooting = this.Timer(_shooting, _shoot, ()=> 
        {
            _canShoot = false;
        },()=>
        { 
            _canShoot = true;
            _shooting = 0f;        
        });

        return _canShoot;
    }




    public void Fire()
    {
        this.SendCommand(new FireCommand(_muzzleTrans,_bulletPointSpawnComponent, _bulletModel, _srBoundsSizeX,_despawnKey));

    }
    #region GetArchitecture
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion  
}



