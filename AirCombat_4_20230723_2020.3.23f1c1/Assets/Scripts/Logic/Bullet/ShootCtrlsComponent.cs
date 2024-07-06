using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;


/// <summary>动态添加
/// 不负责初始化
/// <br/>目前只有敌人</summary>
public class ShootCtrlsComponent : MonoBehaviour 
{

    [SerializeField] private List<ShootCtrl> _shootCtrlLst = new List<ShootCtrl>();
    [SerializeField] private Transform _muzzlesTrans;

    public ShootCtrlsComponent Init(EnemyData enemyData, Transform muzzlesTrans )
    {
        _muzzlesTrans = muzzlesTrans;
        _shootCtrlLst.Clear();
        //muzzlesTrans.gameObject.RemoveAllIfExistComponents<ShootCtrl>();
        //
        for (int i = 0; i < enemyData.bulletType.Length; i++)
        {   
            
            BulletType bulletType = enemyData.bulletType[i];
            Transform muzzleTrans = _muzzlesTrans.FindOrNew($"{GameObjectName.Muzzle}_{bulletType.Enum2String()}");
            //STest.IsBossPlane(bulletType);
            //
            ShootCtrl shootCtrl = muzzleTrans.AddComponent<ShootCtrl>();
            _shootCtrlLst.Add( shootCtrl);
        }


        return this;
    }


}