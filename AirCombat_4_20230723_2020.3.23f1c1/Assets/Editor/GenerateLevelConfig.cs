using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using DG.Tweening.Plugins.Core.PathCore;
using LitJson;
using UnityEditor;
using UnityEngine;

public class GenerateLevelConfig:IEditor
{
    [MenuItem(MenuItemPath.Tools_GenerateLevelEnemyConfig)]
    private static void Execute()
    {
        var data = new EnemyLevelData();
        data.LevelDatas = new LevelData[2];
        data.LevelDatas[0] = LevelOne(); 
        data.LevelDatas[1] = LevelTwo();
        string path = ResourcesPath.CONFIG_LEVEL_ENEMY_DATA;
        data.Object2JsonFile_JsonMapper(path);
        (path.SubStringStartWith(StringMark.Assets)).ShootAt();
    }





    #region pri
    private static PlaneCreatorData GetCreaterData(int idMin, int idMax, int queueNum, int queuePlaneNum, EnemyType type, double x)
    {
        var data = new PlaneCreatorData()
        {
            IdMin = idMin,
            IdMax = idMax,
            QueueNum = queueNum,
            QueuePlaneNum = queuePlaneNum,
            Type = type,
            X = x
        };

        return data;
    }


    private static LevelData LevelOne()
    {
        return new LevelData()
        {
            EnemyNumMax = 50,
            EnemyNumMin = 40,
            PlaneCreaterDatas = GetCreaterListOne()
        };
    }



    private static LevelData LevelTwo()
    {
        return  new LevelData()
        {
            EnemyNumMax = 50,
            EnemyNumMin = 40,
            PlaneCreaterDatas = GetCreaterListOne()
        };
    }

    private static PlaneCreatorData[] GetCreaterListOne()
    {
        List<PlaneCreatorData> list = new List<PlaneCreatorData>();
        list.Add(GetCreaterData(0, 1, 4, 5, EnemyType.NORMAL, 0));
        return list.ToArray();
    }
    #endregion  



}