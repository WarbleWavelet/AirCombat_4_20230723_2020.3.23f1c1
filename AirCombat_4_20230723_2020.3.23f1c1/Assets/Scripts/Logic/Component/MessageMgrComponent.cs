/****************************************************
    文件：MessageMgrComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/20 19:45:30
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 

public class MessageMgrComponent : MonoBehaviour,IMessageMgr ,IInitComponent<MessageMgrComponent> 
{
    MessageMgr _messageMgr;

    public MessageMgrComponent InitComponent()
    {
        _messageMgr = new MessageMgr();
        return this;
    }


    #region IMessageMgr
    public void AddListener(int key, Action<object[]> callback)
    {
        _messageMgr.AddListener(key, callback);
    }

    public void AddListener(string key, Action<object[]> callback)
    {
        _messageMgr.AddListener(key, callback);
    }



    public void RemoveListener(int key, Action<object[]> callback)
    {
        _messageMgr.RemoveListener(key, callback);
    }

    public void RemoveListener(string key, Action<object[]> callback)
    {
        _messageMgr.RemoveListener(key, callback);
    }

    public void SendMsg(int key, params object[] args)
    {
        _messageMgr.SendMsg(key, args);
    }

    public void SendMsg(string key, params object[] args)
    {
        _messageMgr.SendMsg(key, args);
    }
    #endregion

}



