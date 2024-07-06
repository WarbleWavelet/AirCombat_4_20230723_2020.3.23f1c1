/****************************************************
    文件：MainCameraCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using LitJson;
using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UniRx.Triggers;
using UnityEngine;
using static ResourcesName;
using Random = UnityEngine.Random;


[Obsolete("已过时=>GameObejctPoolSystem")]
public interface ISpawnBulletSystem : QFramework.ISystem
{
     GameObject SpawnBullet();

     void RecycleBullet(GameObject go);

}

[Obsolete]
public class SpawnBulletSystem :QFramework.AbstractSystem, ISpawnBulletSystem
{
    #region 属性
    Dictionary<BulletType, SimpleObjectPool<GameObject>> _bulletPrefabDic = new Dictionary<BulletType, SimpleObjectPool<GameObject>>();
    Dictionary<BulletType, IBulletModel> _bulletModelDic = new Dictionary<BulletType, IBulletModel>();
    SimpleObjectPool<GameObject> _bulletPool;
    const int _preloadCnt = 50;
    #endregion


    #region 生命

    protected override void OnInit()
    {
        ResKit.Init();
        ResLoader resLoader = ResLoader.Allocate();
        //
        {//Player的子弹 
            GameObject prefab = resLoader.LoadSync<GameObject>(ABName.Bullet);
            _bulletPool = new SimpleObjectPool<GameObject>(() =>
            {
                GameObject go = GameObject.Instantiate(prefab, Vector2.zero, Quaternion.identity);
                go.SetParent(Camera.main.transform.FindTopOrNewPath(GameObjectPath.Pool_BulletPool));
                go.Identity();
                go.Hide();

                return go;
            }, go =>
            {
                go.RemoveIfExistComponent<BulletPlayerCtrl>();
                go.RemoveIfExistComponent<BulletEnemyCtrl>();
                go.Hide();
            }, _preloadCnt);
        }
        {//子弹Model 
            JsonData jsonData = ResourcesPath.CONFIG_BULLET_CONFIG.JsonPath2JsonData_JsonMapper();
            for (BulletType type = BulletType.PLAYER; type < BulletType.COUNT; type++)
            {
                if (jsonData.Keys.Contains(type.ToString()))
                {
                    _bulletModelDic[type] = GetBulletModelArr(type, jsonData);
                }
            }
            foreach (IBulletModel i in _bulletModelDic.Values)
            {
                Debug.Log("****_bulletModelDic" + i.ToString());
            }
        }
    }

    #endregion



    #region pub ISpawnBulletSystem
    public GameObject SpawnBullet()
    {

        GameObject go = _bulletPool.Allocate();
        if (go.IsNullObject())
        {
            Debug.LogError($"取不到子弹，{_bulletPool}未初始化");
        }
        return go;
    }

    public void RecycleBullet(GameObject go)
    {
        _bulletPool.Recycle(go);
    }
    #endregion

    #region pri

    private static IPathData[] GetTrajectoryDataArray(TrajectoryType type, JsonData data)
    {
        switch (type)
        {
            case TrajectoryType.STRAIGHT:
                return JsonMapper.ToObject<StraightPathData[]>(data[type.ToString()].ToJson());
            case TrajectoryType.W:
                return JsonMapper.ToObject<VPathData[]>(data[type.ToString()].ToJson());
            case TrajectoryType.ELLIPSE:
                return JsonMapper.ToObject<EllipsePathData[]>(data[type.ToString()].ToJson());
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
    private static IBulletModel GetBulletModelArr(BulletType type, JsonData data)
    {
        string typeStr = type.Enum2String();
        string jsonData = data[typeStr].ToJson();
        switch (type)
        {
            case BulletType.PLAYER:
                return jsonData.JsonStr2Object<PlayerBulletModel>(JsonParseType.JsonMapper);
            case BulletType.ENEMY_NORMAL_0:
                return jsonData.JsonStr2Object<EnemyNormalBulluetModel>(JsonParseType.JsonMapper);
            case BulletType.ENEMY_BOSS_0:
                return jsonData.JsonStr2Object<EnemyBoss0BulluetModel>(JsonParseType.JsonMapper);
            case BulletType.ENEMY_BOSS_1:
                return jsonData.JsonStr2Object<EnemyBoss1BulluetModel>(JsonParseType.JsonMapper);
            case BulletType.POWER:
                return jsonData.JsonStr2Object<PowerBulletModel>(JsonParseType.JsonMapper);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    #endregion





    #region 实现
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion

}



