using UnityEngine.Purchasing.MiniJSON;


/// <summary>所有关卡的敌人数据</summary>
public class EnemyLevelData :IJsonPath
{
    //
    /// <summary>
    /// 索引Idx==curLevel-1
    /// Assets/StreamingAssets/Config/onfig.json
    /// </summary>
    public LevelData[] LevelDatas;

    public string ConfigPath()
    {
        return ResourcesPath.CONFIG_LEVEL_ENEMY_DATA;
    }
}

public class LevelData:IJsonPath  //有关json，不要随便改字段名
{

    #region 说明
    /**
        "LevelDatas": [
    {
      "PlaneCreaterDatas": [
        {
          "IdMax": 1,
          "IdMin": 0,
          "QueuePlaneNum": 5,
          "QueueNum": 4,
          "E_BulletType": 0,
          "X": -1.0
        },
    ...
      ],
      "MissileCreaterDatas": [
        {
          "Batch": 0,
          "X": 1,
          "NumOfWarning": 2,
          "EachWarningTime": 0.8,
          "SpawnCount": 2,
          "Speed": 4
        },
    ...
      ],
      "EnemyNumMax": 5,
      "EnemyNumMin": 5,
      "NormalDeadNumForSpawnElites": 20
    },
     */

    #endregion
    public PlaneCreatorData[] PlaneCreaterDatas; //敌我飞机
    public MissileCreatorData[] MissileCreaterDatas; //导弹
    public int EnemyNumMax;
    public int EnemyNumMin;
    /// <summary>
    /// 每死亡多少个普通怪生成一波精英怪
    /// </summary>
    public int NormalDeadNumForSpawnElites;

    public string ConfigPath()
    {
        return ResourcesPath.CONFIG_LEVEL_CONFIG;
    }
}



#region ICreaterData


/// <summary>LevelEnemyDataConfig</summary>
public interface ICreatorData
{
    
}

public abstract class CreatorDataBase : ICreatorData
{
    public string ConfigPath()
    {
        return ResourcesPath.CONFIG_LEVEL_ENEMY_DATA;
    }
}

public class PlaneCreatorData : CreatorDataBase    //json数据没乱改字段名
{                                            
    /**
    {
        "IdMax": 1,
        "IdMin": 0,
        "QueuePlaneNum": 5,
        "QueueNum": 4,
        "E_BulletType": 0,
        "X": -1.0
    },     */
    /// <summary>找图用的。同等级下的不同Id敌人飞机</summary>
    public int IdMax;
    /// <summary>找图用的。同等级下的不同Id敌人飞机</summary>
    public int IdMin;
    /// <summary> 每个飞机队列的飞机数量  </summary>
    public int QueuePlaneNum;
    /// <summary>  需要生成队列的数量 </summary>
    public int QueueNum;
    public EnemyType Type; // EnemyType  json数据没乱改字段名
    /// <summary>-1左 0中 1右</summary>
    public double X;


}


/// <summary>导弹</summary>
public class MissileCreatorData : CreatorDataBase
{
    /// <summary> 当前导弹的生成批次 </summary>
    public int Batch;
    public double X;
    public int NumOfWarning;
    public double EachWarningTime;
    public int SpawnCount; 
    public double Speed;


}
#endregion
