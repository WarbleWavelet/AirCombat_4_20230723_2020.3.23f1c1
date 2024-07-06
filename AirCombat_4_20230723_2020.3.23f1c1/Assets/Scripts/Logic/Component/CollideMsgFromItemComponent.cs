/****************************************************
    文件：CollideMsgFromItemComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/6 19:39:20
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CollideMsgFromItemComponent : MonoBehaviour, ICollideMsg
{
    private Action _onCollide;

    public void Init(Action collideEvent)
    {
        _onCollide = collideEvent;
    }

    public void CollideMsg(Transform other)
    {
        if (other.tag == Tags.PLAYER)
        {
            _onCollide.DoIfNotNull();
        }
    }
}




