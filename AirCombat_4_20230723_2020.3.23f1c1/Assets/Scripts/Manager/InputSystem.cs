using System.Collections.Generic;
using System;
using UnityEngine;
using QFramework;




#region 接口
public interface IInputModule
{
    void AddListener(KeyCode code);
    void AddMouseListener(int code);
    void RemoveListener(KeyCode code);
    void RemoveMouseListener(int code);
}

/// <summary>按键与按键状态的组合key</summary>
public interface IInputUtil : QFramework.IUtility
{
    public string GetKey(KeyCode code, KeyState state);
    public string GetKey(int code, KeyState state);

}
#endregion



#region InputSystem


public interface IInputSystem : QFramework.ISystem, IInputModule, IInputUtil
{
     void AddListener(KeyCode keyCode, KeyState KeyState, Action<object[]> callback);
     void RemoveListener(KeyCode keyCode, KeyState KeyState, Action<object[]> callback);

}

public class InputSystem : QFramework.AbstractSystem,IInputSystem, IUpdate,ICanGetSystem
{
    private readonly InputModule _module= new InputModule();
    private readonly bool _updating = false;
                                  
    protected override void OnInit()
    {
        _module.AddSendEvent(SendKey);
        _module.AddSendEvent(SendMouse);
    }



    #region pub IInputUtil
    public string GetKey(KeyCode code, KeyState state)
    {
        return code + state.ToString();
    }

    public string GetKey(int code, KeyState state)
    {
        return code + state.ToString();
    }
    #endregion  


    #region pub IInputModule
    public void AddListener(KeyCode code)
    {
        _module.AddListener(code);
        AddUpdate();
    }

    public void AddMouseListener(int code)
    {
        _module.AddMouseListener(code);
        AddUpdate();
    }

    public void RemoveListener(KeyCode code)
    {
        _module.RemoveListener(code);
        RemoveUpdate();
    }

    public void RemoveMouseListener(int code)
    {
        _module.RemoveMouseListener(code);
        RemoveUpdate();
    }
    #endregion


    #region pub InputSystem

    public void AddListener(KeyCode keyCode, KeyState keyState, Action<object[]> callback)
    {
        var key = GetKey(keyCode, keyState);
      this.GetSystem<IMessageSystem>().AddListener(key, callback);
    }

    public void RemoveListener(KeyCode keyCode, KeyState keyState, Action<object[]> callback)
    {
        var key = GetKey(keyCode, keyState);
        this.GetSystem<IMessageSystem>().RemoveListener(key, callback);
    }

    #endregion  

    #region pub IUpdate
    public int Framing { get; set; }

    public int Frame { get; }

    public void FrameUpdate()
    {
        _module.Execute();
    }
    #endregion  



    #region pri
    private void SendKey(KeyCode code, KeyState state)
    {
        this.GetSystem<IMessageSystem>().SendMsg(GetKey(code, state), state);
    }

    private void SendMouse(int code, KeyState state)
    {
        this.GetSystem<IMessageSystem>().SendMsg(GetKey(code, state), state);
    }

    private void AddUpdate()
    {
        if (!_updating)
            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
    }

    private void RemoveUpdate()
    {
        if (_module.ListenerCount == 0)
            this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
    }


    #endregion
}
#endregion


#region InputModule
public class InputModule : IInputModule
{


    #region 字属构造
    private readonly Dictionary<KeyCode, int> _keyCodeDic;
    private readonly Dictionary<int, int> _mouseDic;
    private Action<KeyCode, KeyState> _keyEvent;
    private Action<int, KeyState> _mouseEvent;

    public InputModule()
    {
        _keyCodeDic = new Dictionary<KeyCode, int>();
        _mouseDic = new Dictionary<int, int>();
    }

    public int ListenerCount
    {
        get
        {
            if (_keyCodeDic == null || _mouseDic == null)
                return 0;

            return _keyCodeDic.Count + _mouseDic.Count;
        }
    }
    #endregion



    #region 实现
    public void AddListener(KeyCode code)
    {
        if (_keyCodeDic.ContainsKey(code))
            _keyCodeDic[code] += 1;
        else
            _keyCodeDic.Add(code, 1);
    }

    public void AddMouseListener(int code)
    {
        if (_mouseDic.ContainsKey(code))
            _mouseDic[code] += 1;
        else
            _mouseDic.Add(code, 1);
    }

    public void RemoveListener(KeyCode code)
    {
        if (_keyCodeDic.ContainsKey(code))
        {
            _keyCodeDic[code] -= 1;
            if (_keyCodeDic[code] <= 0) _keyCodeDic.Remove(code);
        }
        else
        {
            Debug.LogError("当前移除指令并没有被监听，Keycode：" + code);
        }
    }

    public void RemoveMouseListener(int code)
    {
        if (_mouseDic.ContainsKey(code))
        {
            _mouseDic[code] -= 1;
            if (_mouseDic[code] <= 0) _mouseDic.Remove(code);
        }
        else
        {
            Debug.LogError("当前移除指令并没有被监听，Keycode：" + code);
        }
    }
    #endregion



    #region 辅助


    public void AddSendEvent(Action<KeyCode, KeyState> keyEvent)
    {
        _keyEvent = keyEvent;
    }

    public void AddSendEvent(Action<int, KeyState> keyEvent)
    {
        _mouseEvent = keyEvent;
    }


    public void Execute()
    {
        if (_keyEvent == null || _mouseEvent == null)
        {
            Debug.LogError("输入监听模块发送消息事件不能为空");
            return;
        }

        foreach (var pair in _keyCodeDic)
        {
            if (Input.GetKeyDown(pair.Key)) _keyEvent(pair.Key, KeyState.DOWN);
            if (Input.GetKey(pair.Key)) _keyEvent(pair.Key, KeyState.PREE);
            if (Input.GetKeyUp(pair.Key)) _keyEvent(pair.Key, KeyState.UP);
        }

        foreach (var pair in _mouseDic)
        {
            if (Input.GetMouseButtonDown(pair.Key)) _mouseEvent(pair.Key, KeyState.DOWN);
            if (Input.GetMouseButton(pair.Key)) _mouseEvent(pair.Key, KeyState.PREE);
            if (Input.GetMouseButtonUp(pair.Key)) _mouseEvent(pair.Key, KeyState.UP);
        }
    }
    #endregion


}



public enum KeyState
{
    DOWN,
    /// <summary>一直按着</summary>
    PREE,
    UP
}
#endregion
