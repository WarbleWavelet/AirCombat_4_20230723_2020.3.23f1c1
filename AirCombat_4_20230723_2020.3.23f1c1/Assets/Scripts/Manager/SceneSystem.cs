using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public interface ISceneSystem : QFramework.ISystem
{
    void AsyncLoadScene(ESceneName name);
    void AddSceneLoaded(ESceneName name, Action<Action> action);

    void AddSceneUnloaded(ESceneName name, Action action);

    float Process();

    void SceneActivation();
    bool IsScene(ESceneName name);
}
public class SceneSystem : AbstractSystem, ISceneSystem
{
    private AsyncOperation _async;
    private readonly Dictionary<ESceneName, int> _initItemTotalNum = new Dictionary<ESceneName, int>();
    private readonly Dictionary<ESceneName, Action<Action>> _loadedDic = new Dictionary<ESceneName, Action<Action>>();
    private readonly Dictionary<ESceneName, Action> _unloadedDic = new Dictionary<ESceneName, Action>();
    public int CurInitNum { get; private set; }

    public int InitTotalNum => _initItemTotalNum[this.GetModel<IAirCombatAppStateModel>().TarScene];


    protected override void OnInit()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnload;
        InitItemTotalNum();
    }




    #region pub
    public void AsyncLoadScene(ESceneName name)
    {
        ResetData();
        this.GetSystem<ICoroutineSystem>().StartOutter(AsyncLoad(name.ToString()));
    }

    public void AddSceneLoaded(ESceneName name, Action<Action> action)
    {
        _initItemTotalNum[name] += 1;

        if (_loadedDic.ContainsKey(name))
            _loadedDic[name] += action;
        else
            _loadedDic[name] = action;
    }

    public void AddSceneUnloaded(ESceneName name, Action action)
    {
        if (_unloadedDic.ContainsKey(name))
            _unloadedDic[name] += action;
        else
            _unloadedDic[name] = action;
    }

    public float Process()
    {
        var ratio = CurInitNum / (float)InitTotalNum;
        if (_async != null && _async.progress >= 0.9f)
            SceneActivation();

        return ratio;
    }

    public void SceneActivation()
    {
        CurInitNum++;
        _async.allowSceneActivation = true;
        this.GetModel<IAirCombatAppStateModel>().CurScene.Value = this.GetModel<IAirCombatAppStateModel>().TarScene;
        _async = null;
    }
    public bool IsScene(ESceneName name)
    {
        return this.GetModel<IAirCombatAppStateModel>().CurScene.Value == name;

    }
    #endregion


    #region pri


    private void ResetData()
    {
        CurInitNum = 0;
        InitItemTotalNum();
    }
    private void InitItemTotalNum()
    {
        for (var i = ESceneName.Main; i < ESceneName.COUNT; i++)
        {
            _initItemTotalNum[i] = 1;
        }
    }    
    
    private IEnumerator AsyncLoad(string name)
    {
        _async = SceneManager.LoadSceneAsync(name);
        _async.allowSceneActivation = false;
        yield return _async;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var name = scene.name.String2Enum<ESceneName>();
        if (_loadedDic.ContainsKey(name) && _loadedDic[name] != null)
        {
            _loadedDic[name](LoadCallBack);
        }
    }

    private void LoadCallBack()
    {
        CurInitNum++;
    }

    private void OnSceneUnload(Scene scene)
    {
        var name = scene.name.String2Enum<ESceneName>();
        if (_unloadedDic.ContainsKey(name) && _unloadedDic[name] != null)
        {
            _unloadedDic[name]();
        }
    }


    #endregion



    #region 重写


    #endregion
}