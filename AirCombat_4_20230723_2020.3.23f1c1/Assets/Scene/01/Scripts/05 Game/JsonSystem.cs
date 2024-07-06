/****************************************************
    文件：SpawnPlaneSystem.cs
	作者：lenovo
    邮箱: 
    日期：2023/12/29 20:57:10
	功能：
*****************************************************/

using LitJson;
using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;



public interface IJsonSystem : QFramework.ISystem
{
     EnemyData GetEnemyData(EnemyType enemyType, int id);
     EnemyData[] GetEnemyDatas(EnemyType enemyType);
    string GetEnemyPlaneKey(EnemyType enemyType, int id);
}
//public class GameObjectSystem  功能重复


public class JsonSystem : QFramework.AbstractSystem,IJsonSystem   ,IDestroy
{
    #region 属性
    //
    /// <summary>enemy数据</summary>
    Dictionary<EnemyType, Dictionary<int, EnemyData>> _enemyDataDic = new Dictionary<EnemyType, Dictionary<int, EnemyData>>();

    Dictionary<TrajectoryType, IPathData[]> _pathDataDic = new Dictionary<TrajectoryType, IPathData[]>();
    ResLoader _resLoader;
    #endregion




    #region 生命
    protected override void OnInit()
    {
        //  return;
        ResKit.Init();
        _resLoader = ResLoader.Allocate();
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY, this);
        //
        InitEnemyDataDic(out _enemyDataDic);
        InitPathDataDic(out _pathDataDic);

    }



    #endregion

    #region pub ISpawnPlaneSystem      

    public void Log()
    {
        CommonClass.Log_ClassFunction();
    }

    public EnemyData GetEnemyData(EnemyType enemyType, int id)
    {
        Dictionary<int, EnemyData> dic;
        EnemyData enemyData;
        if (_enemyDataDic.TryGetValue(enemyType, out dic))
        {
            if (dic.TryGetValue(id, out enemyData))
            {
                return enemyData;

            }
        }

        Debug.LogError($"获取不到敌人飞机数据，类型：{enemyType}，id：{id}");
        return null;
    }

    public EnemyData[] GetEnemyDatas(EnemyType enemyType)
    {
        Dictionary<int, EnemyData> dic = new Dictionary<int, EnemyData>();
        if (_enemyDataDic.TryGetValue(enemyType, out dic))
        {
            return dic.Values.ToArray();
        }

        Debug.LogError($"获取不到敌人飞机数据，类型：{enemyType}");
        return null;
    }
    #endregion
                                                                                

    #region pub IDestroy 
    public void DestroyFunc()
    {
        _resLoader.Dispose();
    }
    #endregion

    #region pri
    /// <summary>防止存取时key混乱了</summary>
    public string GetEnemyPlaneKey(EnemyType enemyType, int id)
    {
        return $"{ResourcesPath.PREFAB_PLANE}/{enemyType}{id}";              
    }                                                 
    private static IPathData[] GetTrajectoryDataArr(TrajectoryType type, JsonData data)
    {
        string typeStr = type.Enum2String();
        string jsonData = data[typeStr].ToJson();
        switch (type) //这里的枚举大小写 关联json中的大小写
        {
            case TrajectoryType.STRAIGHT:
                return jsonData.JsonStr2Object<StraightPathData[]>(JsonParseType.JsonMapper);
            case TrajectoryType.W:
                return jsonData.JsonStr2Object<VPathData[]>(JsonParseType.JsonMapper);
            case TrajectoryType.ELLIPSE:
                return jsonData.JsonStr2Object<EllipsePathData[]>(JsonParseType.JsonMapper);
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }



    /// <summary>获取数据AllEnemyData</summary>
    void InitEnemyDataDic(out Dictionary<EnemyType, Dictionary<int, EnemyData>> enemyDataDic)
    {
        enemyDataDic = new Dictionary<EnemyType, Dictionary<int, EnemyData>>();
        //AllEnemyData allEnemyData= LoadByJson<AllEnemyData>(Paths.CONFIG_ENEMY);
        AllEnemyData allEnemyData = (ResourcesPath.CONFIG_ENEMY).JsonPath2Object<AllEnemyData>(JsonParseType.JsonConvert);

        //Debug.Log(CommonClass.Log_ClassFunction()+ allEnemyData.ToString());
        Dictionary<int, EnemyData> normalDic = new Dictionary<int, EnemyData>();
        Dictionary<int, EnemyData> eliteDic = new Dictionary<int, EnemyData>();
        Dictionary<int, EnemyData> bossDic = new Dictionary<int, EnemyData>();
        foreach (EnemyData item in allEnemyData.NORMAL)
        {
            normalDic.Add(item.id, item);
        }
        foreach (EnemyData item in allEnemyData.ELITES)
        {
            eliteDic.Add(item.id, item);
        }
        foreach (EnemyData item in allEnemyData.BOSS)
        {
            bossDic.Add(item.id, item);
        }
        enemyDataDic.Add(EnemyType.NORMAL, normalDic);
        enemyDataDic.Add(EnemyType.ELITES, eliteDic);
        enemyDataDic.Add(EnemyType.BOSS, bossDic);


        foreach (Dictionary<int, EnemyData> i in _enemyDataDic.Values)
        {
            foreach (EnemyData j in i.Values)
            {

                // Debug.Log("****"+j.ToString());
            }
        }
    }

    /// <summary>轨迹数据</summary>
    void InitPathDataDic(out Dictionary<TrajectoryType, IPathData[]> pathDataDic)
    {
        pathDataDic = new Dictionary<TrajectoryType, IPathData[]>();
        JsonData jsonData = (ResourcesPath.CONFIG_ENEMY_TRAJECTORY).JsonPath2JsonData_JsonMapper();
        for (TrajectoryType type = TrajectoryType.STRAIGHT; type < TrajectoryType.COUNT; type++)
        {
            if (jsonData.Keys.Contains(type.ToString()))
            {
                pathDataDic[type] = GetTrajectoryDataArr(type, jsonData);
            }
        }
        foreach (IPathData[] i in pathDataDic.Values)
        {
            foreach (IPathData j in i)
            {

                // Debug.Log("****" + j.ToString());
            }
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




