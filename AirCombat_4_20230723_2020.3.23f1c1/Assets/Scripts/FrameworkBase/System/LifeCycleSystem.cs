using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



#region LifeCycleSystem


public interface ILifeCycleSystem : QFramework.ISystem  
{
   void Add(LifeName name, object o);
   void Remove(LifeName name, object o);
   void RemoveAll(object o);
}

/// <summary>有Json数据和PlayerPrefers数据的初始化</summary>
public class LifeCycleSystem : QFramework.AbstractSystem, ILifeCycleSystem, ICanGetModel
{
                                                                        
    private LifeCycleSystemMono _mono;
    protected override void OnInit()
    {
        var cfg = new LifeCycleAddConfig();
        cfg.Init();
        Add2LifeCycleConfig(cfg);

        LifeCycleConfig.Do(LifeName.INIT);
        //
        Transform t = this.GetSystem<IDontDestroyOnLoadSystem>().GetOrAdd(GameObjectPath.System_LifeCycleSystem);
        _mono = t.GetOrAddComponent<LifeCycleSystemMono>();
        //自定义的UniRx拓展
        _mono.DoWhenUpdate(UpdateFunction);
        _mono.DoWhenDestroy(DestroyFunction);
    }

    #region pri


     void UpdateFunction()
    {
       
        if (this.GetModel<IAirCombatAppStateModel>().E_GameState == GameState.PAUSE   )
        {
            return;
        }
        LifeCycleConfig.Do(LifeName.UPDATE);
    }

    void DestroyFunction()
    { 
    
    }
    #endregion  

    #region pub   ILifeCycleSystem

    public void Add(LifeName name, object o)
    {
        LifeCycleConfig.Add(name, o);
    }

    public void Remove(LifeName name, object o)
    {
        LifeCycleConfig.Remove(name, o);
    }

    public void RemoveAll(object o)
    {
        LifeCycleConfig.RemoveAll( o);
    }


    #endregion


    #region pri
    private void Add2LifeCycleConfig(LifeCycleAddConfig cfg)
    {
     //尝试私有化 LifeCycleConfig.LifeCycleDic的写法
        foreach (object o in cfg.LifeCycleArrayLst)
        {
            //TODD ：LifeCycleMgr，不确定这样改会不会报错
            LifeCycleConfig.Add(o);
        }          
    }
    #endregion

    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion
}
#endregion


#region LifeCycleAddConfig

/// <summary>生命周期配置表
/// <br/>LifeCycleAddConfig=LifeCycleConfig</summary> 
public class LifeCycleAddConfig : IInit
{
    public ArrayList LifeCycleArrayLst { get; private set; }

    public void Init()
    {
        LifeCycleArrayLst = new ArrayList
        {
            //new StCustomAttributes(),
            //NSPlaneSpritesModel.Single,
            //GameDataMgr.Single,
        };
    }

}

#endregion  


#region LifeCycleConfig
/// <summary>
/// 维护了两个字典
/// 我这里是被LifeCycleSystem管理着
///  <para />  &lt; LifeName,ILifeCycle &gt;
///  <para />  &lt; LifeName,IfChangeCreatorAndSpawnNextQueue &gt;
/// </summary> 


public class LifeCycleConfig
{
    static Dictionary<LifeName, ILifeCycle> _lifeCycleDic = new Dictionary<LifeName, ILifeCycle>
    {
        { LifeName.INIT,   new LifeCycle<IInit>()  },
        { LifeName.UPDATE, new LifeCycle<IUpdate>()} ,
        { LifeName.DESTROY, new LifeCycle<IDestroy>()}  ,
        { LifeName.SHOW, new LifeCycle<IShow>()} ,
        { LifeName.HIDE, new LifeCycle<IHide>()}
    };

    public static Dictionary<LifeName, Action> LifeCycleFuncDic = new Dictionary<LifeName, Action>
    {
        {
            LifeName.INIT,() =>
            {
                ILifeCycle life =  _lifeCycleDic[LifeName.INIT];
                life.Execute((IInit init) =>
                {
                    init.Init();
                } );
            }
        },
        {
            LifeName.UPDATE,() =>
            {
                 ILifeCycle life =_lifeCycleDic[LifeName.UPDATE];
                life.Execute((IUpdate update) =>
                {
                    if (update.Framing < update.Frame) //没满就加
                    {
                        update.Framing++;
                        return;
                    }

                    update.FrameUpdate(); //满了就执行重置   
                    update.Framing = 0;
                });
            }
        },
        {
            LifeName.DESTROY,() =>
            {
                 ILifeCycle life =_lifeCycleDic[LifeName.DESTROY];
                life.Execute((IDestroy destroy) =>
                {
                    destroy.DestroyFunc();
                });
            }
        },
        {
            LifeName.SHOW,() =>
            {
                 ILifeCycle life =_lifeCycleDic[LifeName.SHOW];
                life.Execute((IShow show) =>
                {
                    show.Show();
                });
            }
        },        
        {
            LifeName.HIDE,() =>
            {
                 ILifeCycle life =_lifeCycleDic[LifeName.HIDE];
                life.Execute((IHide hide) =>
                {
                    hide.Hide();
                });
            }
        }
    };



    #region pub LifeCycleFuncDic
    public static void Do(LifeName lifeName)
    {
        LifeCycleFuncDic[lifeName]();
    }
    #endregion



    #region pub LifeCycleDic


    public static void Add(LifeName name, object obj)
    {
        _lifeCycleDic[name].NeedAdd(obj);
    }

    public static void Add(object obj)
    {
        foreach (var cycle in _lifeCycleDic)
        {
            if (cycle.Value.NeedAdd(obj))
            {
                break;
            }
        }
    }

    public static void Remove(LifeName name, object obj)
    {
        _lifeCycleDic[name].Remove(obj);
    }
    public static void RemoveAll(object obj)
    {
        foreach (var cycle in _lifeCycleDic)
        {
            cycle.Value.Remove(obj);
        }

    }
    #endregion  
}
#endregion


#region ILifeCycle


public interface ILifeCycle
{
    bool NeedAdd(object obj);
    void Remove(object obj);
    void Execute<T>(Action<T> execute);
}


/// <summary>维护一个List&lt;object&gt;</summary> 
public class LifeCycle<T> : ILifeCycle
{
    private readonly List<object> _objLst = new List<object>();

    public bool NeedAdd(object o)
    {
        if (o is T)
        {
            if (_objLst.Contains(o))
            {
                return false;
            }
            else
            {
                _objLst.Add(o);
                return true;
            }
        }

        return false;
    }

    public void Remove(object o)
    {
        _objLst.Remove(o);
    }

    public void Execute<T1>(Action<T1> execute)
    {
        for (int i = 0; i < _objLst.Count; i++)
        {
            execute((T1)_objLst[i]);
        }
    }
}
#endregion



