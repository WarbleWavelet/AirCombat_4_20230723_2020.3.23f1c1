using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>子弹类型</summary>

[AttributeUsage(AttributeTargets.Class)]
public class BulletAttribute : Attribute
{
    public BulletType E_BulletType;


    #region 构造
    public BulletAttribute(BulletType eBulletType)
    {
        E_BulletType = eBulletType;
    }
    #endregion
}
