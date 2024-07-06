/****************************************************
    文件：BulletLoadComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/7 14:14:44
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletLoadComponent : MonoBehaviour, QFramework.IController
{


    #region 字属




    #region Load
    /// <summary>射击间隔。也就是装弹间隔</summary>

    [SerializeField] private float _loadTime;
    public float LoadTime { get => _loadTime; set => _loadTime = value; }
    //
    [SerializeField] private bool _loadFinish;
    public bool LoadFinish { get => _loadFinish; set => _loadFinish = value; }
    #endregion




    #region BulletCnt
    /// <summary>子弹容量</summary>
    [SerializeField] private int _bulletCnt;
    public int BulletCnt { get => _bulletCnt; set => _bulletCnt = value; }
    /// <summary>子弹存量</summary>
    [SerializeField] private int _bulletCnting;
    public int BulletCnting { get => _bulletCnting; set => _bulletCnting = value; }
    #endregion

    //

    #endregion

    #region pub      

    public void LoadBullet()
    {
        _loadFinish = false;

        ActionKit.Delay(_loadTime, () =>
        {
            _loadFinish = true;
            _bulletCnting = _bulletCnt;
        }).Start(this);
    }



    public bool SubBullet()
    {
        if (!_loadFinish)
        { 
            return false;
        
        }
        if (_bulletCnting > 0)
        {
            _bulletCnting--;
            return true;
        }

        return false;
    }



    #endregion



    #region QFramework
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }




    #endregion



}




