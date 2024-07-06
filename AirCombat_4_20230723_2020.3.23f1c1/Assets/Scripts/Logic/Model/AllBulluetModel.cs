using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx.InternalUtil;
using UnityEngine;


#region 接口

public interface IBulletCtrl 
{
    BulletModelComponent GetBulletModelComponent();
}


/// <summary>01 挂子弹上
/// <br/>02 挂发射子弹用来校准位置的子节点</summary>

public interface IBullet
{
    /// <summary>子弹是谁的</summary>
    BulletOwner Owner { get; }
    /// <summary>目标的枚举,可以设计的目标</summary>
    BulletOwner[] ShootOwners { get; }
    /// <summary>获取子弹的目标</summary>
    HashSet<string> GetShootTags();
    /// <summary>可以打记得目标</summary>
    bool ContainsDamageTo(BulletOwner to);
    /// <summary>
    ///  A.包含(天敌B)
    /// 可以直接撞毁,血条都给你扬了
    /// </summary>
    bool ContainsDeadFrom(string toTag);
    /// <summary>子弹给的伤害</summary>
    int GetAttack();
}


/// <summary>统一扔到子节点</summary>
public interface IBulletModel : IBullet
{
    BulletType E_BulletType { get; }
    string AudioName { get; }
    float FireTime { get; }
    Sprite Sprite();
    void OnGotBulletSpeed(Action<float> callBack);
    void OnGotPathCalcArr(Action<IPathCalc[]> callBack);
}


/// <summary>多了EnemyData的Init</summary>
public interface IEnemyBulletModel : IBulletModel
{
    void Init(EnemyData data);
}

public interface IEnemyBossBulletModel : IEnemyBulletModel 
{
    /// <summary>生命进度</summary>
    void UpdateEvent(float lifePrg);
}

#endregion



#region BulluetMode


#region EnemyBossBulluetModelBase
public abstract class EnemyBossBulluetModelBase : IEnemyBossBulletModel  ,ICanGetUtility
{
    private EnemyData _enemyData;
    protected BossBulletData _bossBulletData;
    /// <summary>Boss血量百分比与事件</summary>
    private Dictionary<float, KeyValuePair<BulletEventType, BulletEventData>> _bulletEventDic;
    private Action<Action<float>> _getBulletSpeedAction;
    private Action<Action<IPathCalc[]>> _getTrajectoryAction;
    public void Init(EnemyData enemyData)
    {
        _enemyData = enemyData;
        _getBulletSpeedAction = GetDefaultBulletSpeed;
        _getTrajectoryAction = GetDefaultTrajectory;
        _bossBulletData = GetBulletData();
        _bulletEventDic = InitBulletEventDic(_bossBulletData);
    }
                             
    protected abstract BossBulletData GetBulletData();
    protected abstract BulletName GetBulletName();
    protected abstract IPathCalc[] GetTrajectory();

    #region pub

    public void UpdateEvent(float lifeRatio)
    {
        foreach (var pair in _bulletEventDic)
        {
            if (lifeRatio < pair.Key) //Key BulletInit的血量比率 ,小于百分
            {
                switch (pair.Value.Key)
                {
                    case BulletEventType.CHANGE_SPEED:
                        _getBulletSpeedAction = (callBack) =>
                        {
                            ChangeSpeedData speedData = pair.Value.Value as ChangeSpeedData;
                            callBack.DoIfNotNull((float)speedData.bulletSpeed);
                        };
                        break;
                    case BulletEventType.CHANGE_TRAJECTORY:
                        _getTrajectoryAction = (callBack) =>
                        {
                            ChangeTrajectoryData data = pair.Value.Value as ChangeTrajectoryData;
                            IPathCalc[] temp = SCBulletTrajectoryData.GetStraightPathCalcArr(data.trajectory[0]);
                            callBack.DoIfNotNull(temp);
                        };
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    public void OnGotBulletSpeed(Action<float> callBack)
    {
        _getBulletSpeedAction(callBack);
    }

    public void OnGotPathCalcArr(Action<IPathCalc[]> callBack)
    {
        _getTrajectoryAction(callBack);
    }
    #endregion


    #region IBullet IbulletModel
    public BulletOwner Owner
    {
        get { return BulletOwner.ENEMY; }
    }

    private BulletOwner[] _toTags = new[]
    {
        BulletOwner.PLAYER
    };

    public BulletOwner[] ShootOwners
    {
        get { return _toTags; }
    }

    private HashSet<string> _tags = new HashSet<string>()
    {
        Tags.PLAYER,
        Tags.SHIELD
    };

    public HashSet<string> GetShootTags()
    {
        return _tags;
    }

    public int GetAttack()
    {
        return _enemyData.attack;
    }

    public abstract BulletType E_BulletType { get; }

    public string AudioName
    {
        get { return AudioGame.Audio_Power; }
    }

    public float FireTime
    {
        get { 
            return (float) (_enemyData.fireRate*_bossBulletData.fireRate);
        }
    }

    private Sprite _sprite;

    public Sprite Sprite()
    {
        if (_sprite == null)
            _sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(ResourcesPath.PICTURE_ENEMY_BULLET_FOLDER + GetBulletName());

        return _sprite;
    }

    public bool ContainsDeadFrom(string toTag)
    {
        return GetShootTags().Contains(toTag);
    }

    public bool ContainsDamageTo(BulletOwner owner)
    {
        return ShootOwners.Contains(owner);
    }
    #endregion


    #region pri
    private Dictionary<float, KeyValuePair<BulletEventType, BulletEventData>> InitBulletEventDic(BossBulletData bossData)
    {
        Dictionary<float, KeyValuePair<BulletEventType, BulletEventData>> bulletEventDic = new Dictionary<float, KeyValuePair<BulletEventType, BulletEventData>>();
        foreach (BulletEvent bulletEvent in bossData.Events)
        {
            bulletEventDic[(float)bulletEvent.LifeRatio] = new KeyValuePair<BulletEventType, BulletEventData>(bulletEvent.Type, bulletEvent.Data);
        }
        return bulletEventDic;
    }

    private void GetDefaultBulletSpeed(Action<float> callBack)
    {
        float value = (float) _bossBulletData.bulletSpeed;
        callBack.DoIfNotNull(value);
    }

    private void GetDefaultTrajectory(Action<IPathCalc[]> callBack)
    {
        IPathCalc[] temp = GetTrajectory();

        callBack.DoIfNotNull(temp);
    }
    #endregion


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}

[Bullet(BulletType.ENEMY_BOSS_0)]
public class EnemyBoss0BulluetModel : EnemyBossBulluetModelBase
{
    protected override BossBulletData GetBulletData()
    {
        return GameDataMgr.Single.Get<AllBulletData>().ENEMY_BOSS_0;
    }

    protected override BulletName GetBulletName()
    {
        return BulletName.ENEMY_BOSS_0;
    }

    public override BulletType E_BulletType
    {
        get { return BulletType.ENEMY_BOSS_0; }
    }

    protected override IPathCalc[] GetTrajectory()
    {
        var data = GameDataMgr.Single.Get<AllBulletData>().ENEMY_BOSS_0;
        return SCBulletTrajectoryData.GetStraightPathCalcArr(data.trajectory[0]);
    }
}

[Bullet(BulletType.ENEMY_BOSS_1)]
public class EnemyBoss1BulluetModel : EnemyBossBulluetModelBase
{
    private bool _forward;
    private float _angle;
    private IPathCalc[] _trajectories = new IPathCalc[1];
    
    public override BulletType E_BulletType
    {
        get { return BulletType.ENEMY_BOSS_1; }
    }
    
    protected override BossBulletData GetBulletData()
    {
        return GameDataMgr.Single.Get<AllBulletData>().ENEMY_BOSS_1;
    }

    protected override BulletName GetBulletName()
    {
        return BulletName.ENEMY_BOSS_1;
    }
    protected override IPathCalc[] GetTrajectory()
    {
        var data = GameDataMgr.Single.Get<AllBulletData>().ENEMY_BOSS_1;
        var rotateData = data.trajectory[0];

        if (_angle == 0)
        {
            _angle = (float) rotateData.StartAngle;
            _forward = true;
        }


        if (_angle < rotateData.EndAngle)
        {
            _forward = false;
        }
        else if(_angle > rotateData.StartAngle)
        {
            _forward = true;
        }

        if (_forward)
        {
            _angle += (float) rotateData.RotateOffset;
        }
        else
        {
            _angle -= (float) rotateData.RotateOffset;
        } 
        
        StraightPathCalc trajectory = new StraightPathCalc();
        trajectory.Init(_angle);
        _trajectories[0] = trajectory;
        return _trajectories;
    }
}
#endregion


[Bullet(BulletType.ENEMY_NORMAL_0)]
public class EnemyNormalBulluetModel : IEnemyBulletModel  ,ICanGetUtility
{

    #region 字属
    private EnemyData _enemyData;
    private Sprite _sprite;


    #region IEnemyBulletModel
    public BulletOwner Owner
    {
        get
        {
            return BulletOwner.ENEMY;
        }
    }
    private BulletOwner[] _targets = new[]
    {
        BulletOwner.PLAYER
    };

    public BulletOwner[] ShootOwners
    {
        get
        {
            return _targets;
        }
    }
    private HashSet<string> _tags = new HashSet<string>()
    {
        Tags.PLAYER,
        Tags.SHIELD
    };
    public BulletType E_BulletType
    {
        get
        {
            return BulletType.ENEMY_NORMAL_0;
        }
    }

    public string AudioName
    {
        get
        {
            return AudioGame.Audio_Fire;
        }
    }

    public float FireTime
    {
        get
        {
            return (float)_enemyData.fireRate;
        }
    }
    public HashSet<string> GetShootTags()
    {
        return _tags;
    }

    public int GetAttack()
    {
        _enemyData.CkeckNull();
        return _enemyData.attack;
    }

    public Sprite Sprite()
    {
        if (_sprite == null)
            _sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(ResourcesPath.PICTURE_ENEMY_BULLET_FOLDER + BulletName.ENEMY_NORMAL_0);

        return _sprite;
    }
    public bool ContainsDeadFrom(string toTag)
    {
        return GetShootTags().Contains(toTag);
    }
    public bool ContainsDamageTo(BulletOwner tar)
    {
        return ShootOwners.Contains(tar);
    }
    #endregion  

    #endregion



    public void Init(EnemyData data)
    {
        _enemyData = data;
    }


    #region pub

    public void OnGotBulletSpeed(Action<float> callBack)
    {
        float value = (float)GameDataMgr.Single.Get<AllBulletData>().ENEMY_NORMAL_0.bulletSpeed;
        callBack.DoIfNotNull(value);
    }

    public void OnGotPathCalcArr(Action<IPathCalc[]> callBack)
    {
        var data = GameDataMgr.Single.Get<AllBulletData>().ENEMY_NORMAL_0;

        IPathCalc[] temp = SCBulletTrajectoryData.GetStraightPathCalcArr(data.trajectory[0]);
        callBack.DoIfNotNull(temp);
    }
    #endregion


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}

[Bullet(BulletType.PLAYER)]
public class PlayerBulletModel : IBulletModel  ,QFramework.ICanGetUtility   ,QFramework.ICanGetModel   ,ICanGetSystem
{


    #region 字属


    private Sprite _sprite;
    /// <summary>轨迹</summary>
    private IPathCalc[] _pathCalcArr;






    public float FireTime
    {
        get
        {
            var key = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.value, PlaneProperty.Property.fireRate.Enum2String());
            var rate = this.GetUtility<IStorageUtil>().Get<int>(key);
            return Const.FIRE_BASE_TIME / rate;
        }
    }
    #endregion

    #region IBulletModel
    public BulletOwner Owner => BulletOwner.PLAYER;
    public string AudioName => AudioGame.Audio_Fire;


    private readonly HashSet<string> _toTags = new HashSet<string>
    {
        Tags.ENEMY
    };
    public BulletOwner[] ShootOwners { get; } =
    {
        BulletOwner.ENEMY
    };

    public BulletType E_BulletType
    {
        get
        {
            return BulletType.PLAYER;
        }
    }     
    
    public int GetAttack()
    {
        var key = this.GetUtility<IKeysUtil>().GetNewKey(PropertyItem.ItemKey.value, PlaneProperty.Property.attack.Enum2String());
        var attack = this.GetUtility<IStorageUtil>().Get<int>(key);
        return attack;
    }


    public Sprite Sprite()
    {
        if (_sprite == null)
        {
            _sprite =this.GetSystem<ISpriteSystem>().GetCurIDPlaneSprite();
        }

        return _sprite;
    }

    public bool ContainsDamageTo(BulletOwner owner)
    {
        return ShootOwners.Contains(owner);
    }
    public bool ContainsDeadFrom(string toTag)
    {
        return _toTags.Contains(toTag);
    }

    public HashSet<string> GetShootTags()
    {
        return _toTags;
    }
    #endregion


    #region pub
    public void OnGotBulletSpeed(Action<float> callBack)
    {
        float value = (float)GameDataMgr.Single.Get<AllBulletData>().PLAYER.bulletSpeed;
        callBack.DoIfNotNull(value);
    }

    public void OnGotPathCalcArr(Action<IPathCalc[]> callBack)
    {
        var playerData = GameDataMgr.Single.Get<AllBulletData>().PLAYER;
        int level =   this.GetModel<IAirCombatAppStateModel>().PlaneBulletLevel;
        // TODO:打完Boss结束后报错,超出长度
        // 其他报错,|| 拆开方便看  .发现是playerData, 也就是GameDataMgr未初始化
         //空或者变了
        if (_pathCalcArr == null  ) 
        {
            _pathCalcArr = SCBulletTrajectoryData.GetStraightPathCalcArr(playerData.trajectory[level]);
        }
        try
        {
            if (_pathCalcArr.Length != playerData.trajectory[level].Length)
            {
                _pathCalcArr = SCBulletTrajectoryData.GetStraightPathCalcArr(playerData.trajectory[level]);

            }
        }
        catch (Exception e)
        {
            throw new System.Exception($"异常:...{level}..");
        }


        callBack.DoIfNotNull(_pathCalcArr);
    }
  




    #endregion
    #region QF


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}

[Bullet(BulletType.POWER)]
public class PowerBulletModel : NormalSingleton<PowerBulletModel>, IBulletModel ,ICanGetUtility
{
    private Sprite _sprite;

    private IPathCalc[] _trajectories;

    public HashSet<string> GetShootTags()
    {
        return this.GetUtility<IBulletModelUtil>().GetBulletModel(BulletType.PLAYER).GetShootTags();
    }



    public BulletOwner Owner => this.GetUtility<IBulletModelUtil>().GetBulletModel(BulletType.PLAYER).Owner;

    public int GetAttack()
    {
        return this.GetUtility<IBulletModelUtil>().GetBulletModel(BulletType.PLAYER).GetAttack();
    }

    public BulletOwner[] ShootOwners => this.GetUtility<IBulletModelUtil>().GetBulletModel(BulletType.PLAYER).ShootOwners;

    public BulletType E_BulletType
    {
        get { return BulletType.PLAYER; }
    }
    public string AudioName => AudioGame.Audio_Power;

    public float FireTime => 0.3f;

    public Sprite Sprite()
    {
        if (_sprite == null)
        {
            var path = ResourcesPath.PICTURE_BULLET_POWER;
            _sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(path);
        }

        return _sprite;
    }

    public void OnGotBulletSpeed(Action<float> callBack)
    {
        IBulletModel bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(BulletType.PLAYER);
        bulletModel.OnGotBulletSpeed(callBack);
    }

    public void OnGotPathCalcArr(Action<IPathCalc[]> callBack)
    {
        if (_trajectories == null)
        {
            var tempList = new List<IPathCalc>();
            var angleOffset = 5;
            StraightPathCalc temp;
            for (var curAngle = 60; curAngle < 120; curAngle += angleOffset)
            {
                temp = new StraightPathCalc();
                temp.Init(curAngle);
                tempList.Add(temp);
            }

            _trajectories = tempList.ToArray();
        }

        callBack(_trajectories);
    }

    public bool ContainsDeadFrom(string toTag)
    {
        return GetShootTags().Contains(toTag);
    }
    public bool ContainsDamageTo(BulletOwner owner)
    {
        return ShootOwners.Contains(owner);
    }
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }




}
#endregion




