using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public interface ILoadCreatorDataSystem : QFramework.ISystem
{
    void Init(Action<AllEnemyData, PathDataMgr, EnemyLevelData> callBack);
}
public class LoadCreatorDataSystem :  QFramework.AbstractSystem, ILoadCreatorDataSystem
{

    private AllEnemyData _allEnemyData;
    private PathDataMgr _enemyPathDataMgr;
    /// <summary>所有关卡的敌人数据</summary>
    private EnemyLevelData _enemyCreatorConfigData;
    private Action<AllEnemyData, PathDataMgr, EnemyLevelData> _LoadConfigCallBack;
    protected override void OnInit()
    {

    }
    public void Init(Action<AllEnemyData, PathDataMgr, EnemyLevelData> callBack)
    {
        if (ExtendJudge.IsAllFull(_allEnemyData, _enemyPathDataMgr, _enemyCreatorConfigData))
        {

            callBack.DoIfNotNull(_allEnemyData, _enemyPathDataMgr, _enemyCreatorConfigData);
            return;
        }
        _LoadConfigCallBack = callBack;
        InitPathData();
        InitEnemyData();
        InitCreatorDataForAllLevels();
    }



    #region pri


    private void InitPathData()
    {
        this.GetUtility<ILoadUtil>().LoadConfig(ResourcesPath.CONFIG_ENEMY_TRAJECTORY, (value) =>
        {
            var json = (string)value;
            var dic = JsonUtil.Json2Dic_Trajectory(json);
            _enemyPathDataMgr = new PathDataMgr();
            _enemyPathDataMgr.PathDataDic = dic;

            Callback();
        });
    }
    private void InitEnemyData()
    {
        this.GetUtility<ILoadUtil>().LoadConfig(ResourcesPath.CONFIG_ENEMY, (value) =>
        {
            string json = (string)value;
            _allEnemyData = json.JsonStr2Object<AllEnemyData>(JsonParseType.JsonMapper);
            Callback();
        });
    }

    /// <summary>加载关卡的敌人类型Creator</summary>
    private void InitCreatorDataForAllLevels()
    {
        this.GetUtility<ILoadUtil>().LoadConfig(ResourcesPath.CONFIG_LEVEL_ENEMY_DATA, (value) =>
        {
            string json = (string)value;
            _enemyCreatorConfigData = json.JsonStr2Object<EnemyLevelData>(JsonParseType.JsonMapper);//关卡的类型敌人
            Callback();
        });
    }

    private void Callback()
    {
        if (ExtendJudge.IsOnceNull(_allEnemyData, _enemyPathDataMgr, _enemyCreatorConfigData))
        {
            return;
        }
        _LoadConfigCallBack.DoIfNotNull(_allEnemyData, _enemyPathDataMgr, _enemyCreatorConfigData);
    }




    #endregion
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

}