using JetBrains.Annotations;
using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using UnityEngine;



#region MessageSystem


/// <summary>有时挂在敌人身上,有时作为一个全局惟一的系统</summary>
public interface IMessageMgr
{ 
    void SendMsg(int key, params object[] args);
    void SendMsg(string key, params object[] args);
    void AddListener(int key, Action<object[]> callback);
    void AddListener(string key, Action<object[]> callback);
    void RemoveListener(int key, Action<object[]> callback);
    void RemoveListener(string key, Action<object[]> callback);
}
public interface IMessageSystem : QFramework.ISystem   ,IMessageMgr
{


}

public class MessageMgr : IMessageMgr
{
    private readonly Dictionary<int, ActionMgr<object[]>> _intReceivers = new Dictionary<int, ActionMgr<object[]>>();
    private readonly Dictionary<string, ActionMgr<object[]>> _stringReceivers = new Dictionary<string, ActionMgr<object[]>>();


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    public void AddListener(int key, Action<object[]> callback)
    {
        if (!_intReceivers.ContainsKey(key))
            _intReceivers[key] = new ActionMgr<object[]>();

        _intReceivers[key].Add(callback);
    }

    public void RemoveListener(int key, Action<object[]> callback)
    {
        if (_intReceivers.ContainsKey(key))
            _intReceivers[key].Remove(callback);
    }

    public void SendMsg(int key, params object[] args)
    {
        if (_intReceivers.ContainsKey(key))
            _intReceivers[key].Execute(args);
    }

    public void AddListener(string key, Action<object[]> callback)
    {
        if (!_stringReceivers.ContainsKey(key))
            _stringReceivers[key] = new ActionMgr<object[]>();

        _stringReceivers[key].Add(callback);
    }

    public void RemoveListener(string key, Action<object[]> callback)
    {
        if (_stringReceivers.ContainsKey(key))
            _stringReceivers[key].Remove(callback);
    }

    public void SendMsg(string key, params object[] args)
    {
        if (_stringReceivers.ContainsKey(key))
            _stringReceivers[key].Execute(args);
    }

}

public class MessageSystem :QFramework.AbstractSystem, IMessageSystem 
{
    private readonly Dictionary<int, ActionMgr<object[]>> _intReceivers = new Dictionary<int, ActionMgr<object[]>>();
    private readonly Dictionary<string, ActionMgr<object[]>> _stringReceivers = new Dictionary<string, ActionMgr<object[]>>();

    protected override void OnInit()
    {
       
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    public void AddListener(int key, Action<object[]> callback)
    {
        if (!_intReceivers.ContainsKey(key))
            _intReceivers[key] = new ActionMgr<object[]> ();

        _intReceivers[key].Add(callback);
    }

    public void RemoveListener(int key, Action<object[]> callback)
    {
        if (_intReceivers.ContainsKey(key))
            _intReceivers[key].Remove(callback);
    }

    public void SendMsg(int key, params object[] args)
    {
        if (_intReceivers.ContainsKey(key))
            _intReceivers[key].Execute(args);
    }

    public void AddListener(string key, Action<object[]> callback)
    {
        if (!_stringReceivers.ContainsKey(key))
            _stringReceivers[key] = new ActionMgr<object[]>();

        _stringReceivers[key].Add(callback);
    }

    public void RemoveListener(string key, Action<object[]> callback)
    {
        if (_stringReceivers.ContainsKey(key))
            _stringReceivers[key].Remove(callback);
    }

    public void SendMsg(string key, params object[] args)
    {
        if (_stringReceivers.ContainsKey(key))
            _stringReceivers[key].Execute(args);
    }


}
#endregion  




#region ActionMgr<T>


/// <summary>维护了一个HashSet</summary>
public class ActionMgr<T>
{

    #region 字属构造
    /// <summary>委托链</summary> 
    private HashSet<Action<T>> _actionHsh;
    private Action<T> _action;

    public ActionMgr()
    {
        _actionHsh = new HashSet<Action<T>>();
        _action = null;
    }
    #endregion  


    public void Add(Action<T> action)
    {
        if (_actionHsh.Add(action))
        {
            _action += action;
        }
    }

    public void Remove(Action<T> action)
    {
        if (_actionHsh.Remove(action))
        {
            _action -= action;
        }
    }

    public void Execute(T t)
    {
        _action.DoIfNotNull(t);

    }

    public bool Contains(Action<T> action)
    {
        return _actionHsh.Contains(action);
    }
}
#endregion  



