using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>用来读取所有敌人信息json的一个类</summary>

public class AllEnemyData: IJsonPath  ,ICanGetSystem  ,IInit
{
    //三个属性关联json,不要单改名字
    /// <summary>Boss怪</summary>
    public EnemyData[] BOSS { get; set; }
    /// <summary>精英怪</summary>
    public EnemyData[] ELITES { get; set; }
    /// <summary>小怪</summary>
    public EnemyData[] NORMAL { get; set; }


    public EnemyData[] GetData(EnemyType type)
    {
        if ( ExtendJudge.IsOnceNull(BOSS,ELITES,NORMAL) )
        {
            Init();
        }
        switch (type)
        {
            case EnemyType.NORMAL: return NORMAL;
            case EnemyType.ELITES: return ELITES;
            case EnemyType.BOSS:   return BOSS;
            default: Debug.LogError($"AllEnemyData未有此EnemyType：{type}"); return null;
        }
    }



    #region pri


  public  void Init()
    {
        IJsonSystem sys = this.GetSystem<IJsonSystem>();
        BOSS = sys.GetEnemyDatas(EnemyType.BOSS);
        ELITES = sys.GetEnemyDatas(EnemyType.ELITES);
        NORMAL = sys.GetEnemyDatas(EnemyType.NORMAL);
    }
    #endregion


    #region 重写
    public override string ToString()
    {
        string str = "";
        if (NORMAL == null) Debug.LogError("EnemyData Normal为null");
        if (ELITES == null) Debug.LogError("EnemyData Elites为null");
        if (BOSS == null)   Debug.LogError("EnemyData Boss为null");
        foreach (EnemyData item in NORMAL)
        {
            str+= item.ToString();
        }
        foreach (EnemyData item in ELITES)
        {
            str += item.ToString();
        }
        foreach (EnemyData item in BOSS)
        {
            str += item.ToString();
        }

        return str;
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion
    public string ConfigPath()
    {
        return ResourcesPath.CONFIG_LEVEL_ENEMY_DATA;
    }
}

public class EnemyData   :IJsonPath
{
    public int id;
    public double attackTime;
    public int attack;
    public double fireRate;
    public int life;
    public double speed;
    //
    public TrajectoryType trajectoryType;
    /// <summary>-1代表当前是随机轨迹，大于0的值，代表轨迹id</summary>
    public int trajectoryID;
    public BulletType[] bulletType;   //看json文件没有加s
    public int starNum;
    public int score;
     //
    /// <summary> 掉落道具的可能性，例如值为10，就代表百分之十的概率 </summary>
    public int itemProbability;
    /// <summary> 掉落道具的范围，应该是长度为2的数组 </summary>
    public RewardType[] itemRange;
    /// <summary>
    /// 掉落道具的数量，每个道具都在范围内随机
    /// <para />例如：数量是2，范围是[0,1]，那么可能会出一个0，一1.或者是两个1，或者是两个0
    /// </summary>
    public int itemCount;

    public string ConfigPath()
    {
      return ResourcesPath.CONFIG_ENEMY;
    }
    //public override string ToString()
    //{
    //    string str = "";
    //    str += "\t" + id;
    //    str += "\t" + attack;
    //    str += "\t" + life;
    //    str += "\t" + trajectoryID;
    //    str += "\t" + starNum;
    //    str += "\t" + score;
    //    str += "\t" + itemCount;
    //    str += "\t" + attackTime;
    //    str += "\t" + fireRate;
    //    str += "\t" + speed;
    //    str += "\t" + trajectoryType.ToString();
    //    str += "\t";
    //    foreach (var item in bulletType)
    //    {
    //        str += item.ToString() + ",";
    //    }
    //    str += "\t";
    //    foreach (var item in itemRange)
    //    {
    //        str += item.ToString() + ",";
    //    }

    //    return str;
    //}

}

