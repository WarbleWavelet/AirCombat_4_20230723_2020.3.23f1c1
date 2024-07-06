using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;




#region GameProcessSystem
public interface IGameProcessSystem:QFramework.ISystem,ICanSendCommand
{ 

}
public class GameProcessSystem : AbstractSystem, IGameProcessSystem, IUpdate, IDestroy , IUnRegisterList
{



    #region 字属


    //
    /// <summary>还是持有</summary>
    private DomEnemyCreatorMgr _domEnemyCreatorMgr;
    private GameObject _creatorMgrGo;
    //
    /// <summary>从配置文件获得怎么敌人几时几列几个生成</summary>	
    private List<GameProcessNormalEvent> _normalEventLst;
    private List<GameProcessTriggerEvent> _triggerEventLst;
    private List<GameProcessTriggerEvent> _curTriggerEventLst;
    private GameProcessTriggerEvent _curTrigger;
    //
    private float _spawnNum;
    /// <summary>同时在场的飞机限制数量，LevelData提供</summary>
    private int _enemyActiveNumMax;
    IMessageSystem Msg { get { return this.GetSystem<IMessageSystem>(); } }
    IAirCombatAppModel Model { get { return this.GetModel<IAirCombatAppModel>(); } }
    IAirCombatAppStateModel StateModel { get { return this.GetModel<IAirCombatAppStateModel>(); } }
    private bool LevelStart { get { return StateModel.E_LevelState.Value == LevelState.START; } }
    public List<IUnRegister> UnregisterList{ get { return _unregisterList; } }
    List<IUnRegister> _unregisterList = new List<IUnRegister>();
    #endregion


    #region 生命

    protected override void OnInit()
    {
        OnInited();

    }
    #endregion

    #region GameEvent管的
    /// <summary>想要相对于OnDestroy</summary>
    private void OnInited()
    {
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.DESTROY, this);
        //以下都是状态改变,以及之后的回调,所以在回调中不要写条件大的状态
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Register(state =>
        {
            if (state != GameState.START) return;
            OnGameStart();
        }).AddToUnregisterList(this);
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Register(state =>
        {
            if (state != GameState.END) return;
            OnGameEnd();
        }).AddToUnregisterList(this);
        this.GetModel<IAirCombatAppStateModel>().E_LevelState.Register(state =>
        {
            if (state != LevelState.START) return;
            OnLevelStart();
        }).AddToUnregisterList(this);
        this.GetModel<IAirCombatAppStateModel>().E_LevelState.Register(state =>
        {
            if (state != LevelState.END) return;
            OnLevelEnd();
        }).AddToUnregisterList(this);
    }




    #endregion

    #region IUpdate  IDestroy

    public int Framing { get; set; }
    public int Frame
    {
        get { return 30; }
    }



    public void FrameUpdate()
    {
        if (!LevelStart)
        {
            return;
        }

        if (!JudgeTriggerPatchEnd())
        {
            return;
        }

        UpdateNormalLst(); //生成敌人
        UpdateTriggerLst();
    }


    #endregion


    public void DestroyFunc()
    {
        this.UnRegisterAll();
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
       
    }

    #region pri

    #region 游戏中的


    private void InitEventLst(DomEnemyCreatorMgr creatorMgr)
    {
        //按进度排序
        _triggerEventLst = creatorMgr.GetTriggerEventLst();
        _triggerEventLst.Sort((x, y) =>
        {
            return x.Prg.CompareTo(y.Prg);//-1,0,1
        });
        //
        _normalEventLst = creatorMgr.GetNormalEventLst();
        _spawnNum = GetAllNormalCreatorsSpawnNum();
        _enemyActiveNumMax = creatorMgr.GetActiveNumMax();
    }

    private int GetAllNormalCreatorsSpawnNum()
    {
        int toatal = 0;
        _normalEventLst.ForEach(normalEvent =>
        {
            toatal += normalEvent.SpawnNum;
        });
        return toatal;
    }

    private int GetAllNormalCreatorsSpawningNum()
    {
        int toatal = 0;
        _normalEventLst.ForEach(normalEvent =>
        {
            if (normalEvent.SpawningNum.IsNotNull())
            {
                toatal += normalEvent.SpawningNum();
            }
        });

        return toatal;
    }

    private float GetAllNormalCreatorsSpawningPrg()
    {
        return (float)GetAllNormalCreatorsSpawningNum() / (float)_spawnNum;
        // return (float)GetAllNormalCreatorsSpawningNum() / (float)GetAllNormalCreatorsSpawnNum();
    }

    /// <summary>生成敌人</summary>
    private void UpdateNormalLst()
    {

        // string newKey = this.GetSystem<IJsonSystem>().GetEnemyPlaneKey(i.Int2Enum<EnemyType>(), j);
        int cur = this.GetSystem<IGameObjectPoolSystem>().GetActiveCount(ResourcesPath.PREFAB_PLANE);
        bool changeCreator = cur < _enemyActiveNumMax; //levelData的配置，直接设为5
        if (!changeCreator)
        {
            return;
        }
        if (cur != 0) //折叠不方便看，所以省掉0
        {
            Debug.LogFormat($"场中机数{cur}");
        }

        for (int i = 0; i < _normalEventLst.Count; i++)
        {
            GameProcessNormalEvent e = _normalEventLst[i];
            Action action = e.ChangeCreator;  //ISubEnemuCreatorMgr.IfChangeCreatorAndSpawnNextQueue
            action.DoIfNotNull();
        }
    }


    private void UpdateTriggerLst()
    {
        int rangeIdx = 0;//第几队
        for (int i = 0; i < _triggerEventLst.Count; i++)
        {
            _curTrigger = _triggerEventLst[rangeIdx];
            float gamePrg = GetAllNormalCreatorsSpawningPrg();
            if (gamePrg >= _curTrigger.Prg)// 一队跑完了//就是这个别
            {
                _curTriggerEventLst.Clear();

                if (_curTrigger.PatchEnd != null && _curTrigger.NeedPauseProcess)
                {
                    _curTriggerEventLst.Add(_curTrigger);
                }

                _curTrigger.SpawnAction.DoIfNotNull();
                rangeIdx++;
            }
            else
            {
                break;
            }
        }

        _triggerEventLst.RemoveRange(0, rangeIdx);
    }


    private void ClearData()
    {
        if (_creatorMgrGo.IsNotNull())
            GameObject.Destroy(_creatorMgrGo);

        _curTriggerEventLst.Clear();
    }


    private void InitDomCreatorMgr()
    {
        Transform t = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.Creator_EnemyCreator);
        t.DestroyChildren();
        _domEnemyCreatorMgr = new DomEnemyCreatorMgr().Init(InitEventLst);
    }
    /// <summary>是否达到路径末端</summary>
    private bool JudgeTriggerPatchEnd()
    {
        bool isEnd = true;
        foreach (var e in _curTriggerEventLst)
        {
            if (!e.PatchEnd())
            {
                e.SpawnAction.DoIfNotNull();
                isEnd = false;
            }
        }

        return isEnd;
    }
    #endregion



    #region OnStateXXX
    
    private void OnGameStart()
    {
        this.SendCommand(new SetPlaneSpriteLevelCommand(this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel));
        this.GetModel<IAirCombatAppStateModel>().E_LevelState.Value = LevelState.START;

    }

    
    /// <summary>OnLevelEnd(注意是后,方法是因变量)后会执行的</summary
    private void OnGameEnd()
    {
        this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_GAME_RESULT_VIEW);
    }


    /// <summary>OnLevelEnd(注意是后,方法是因变量)后会执行的</summary>
    private void OnLevelStart()
    {
        StateModel.IsFinishOneLevel.Value = false;
        this.GetSystem<IUISystem>().HidePopPanel( );
        _curTriggerEventLst = new List<GameProcessTriggerEvent>();
        InitDomCreatorMgr();
    }


    private void OnLevelEnd()
    {
        ClearData();
        StateModel.CurLevel.Value++;
        StateModel.IsFinishOneLevel.Value = true;
        this.GetSystem<IUISystem>().PopPanel(ResourcesPath.PREFAB_GAME_RESULT_VIEW);
        this.GetSystem<ICoroutineSystem>().StartDelay(Const.WAIT_LEVEL_START_TIME, () =>
        {
            StateModel.E_LevelState.Value = LevelState.START;
        });
    }
    #endregion
    #endregion
}
#endregion


#region IGameProcessNormalEvent  GameProcessNormalEvent
/// <summary>生成飞机事件
/// <br/>目前只有Normal的Enemy会实现</summary>
public interface IGetGameProcessNormalEvents
{
    List<GameProcessNormalEvent> GetNormalEventLst();//接口方法
}


/// <summary>
/// 引用看接口
/// <para />生成回调，已经生成/总生成
/// <br/>是否更换Creator,是否进行下一队的生成
/// </summary>
public class GameProcessNormalEvent
{


    /// <summary>生成回调,也就是去执行生成动作
    /// <para/>在这里是去找下一队飞机的生成</summary>
    public Action ChangeCreator { get; private set; }
    /// <summary>已经生成</summary>
    public Func<int> SpawningNum { get; private set; }
    /// <summary>总生成</summary>
    public int SpawnNum { get; private set; }

    public  GameProcessNormalEvent(Action spawnAction, Func<int> spawningNum, int spawnNum)
    {
        ChangeCreator = spawnAction;
        SpawningNum = spawningNum;
        SpawnNum = spawnNum;
    }
}

#endregion


#region IGameProcessTriggerEvent GameProcessTriggerEvent
/// <summary>按Normal敌人生成数量总进度，来生成Normal类型以外的敌人</summary>
public interface IGetGameProcessTriggerEvents
{
    List<GameProcessTriggerEvent> GetTriggerEventLst();
}


/// <summary>
/// 除了Normal敌人外的其它类型敌人会实现它
/// <br/>精英怪的出现
/// <br/>火箭的触发
/// <br/>Boss前的警告
/// </summary>
public class GameProcessTriggerEvent
{

    public float Prg { get; private set; }
    /// <summary>生成敌人（靠到达路径末端判断）</summary>
    public Action SpawnAction { get; private set; }

    public bool NeedPauseProcess { get; private set; }

    /// <summary>路径末端</summary>
    public Func<bool> PatchEnd { get; private set; }

    public GameProcessTriggerEvent(float prg, Action spawnAction, bool needPauseProcess, Func<bool> patchEnd)
    {
        Prg = prg;
        SpawnAction = spawnAction;
        NeedPauseProcess = needPauseProcess;
        PatchEnd = patchEnd;
    }
}
#endregion 








