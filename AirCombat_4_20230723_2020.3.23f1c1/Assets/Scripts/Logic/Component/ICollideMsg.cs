using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


#region interface


public interface ICollideMsg
{
    void CollideMsg(Transform other);
}



public interface ICollideItem
{
    void CollideItem(Transform other);
}
public interface ICollidePlayer
{
    /// <summary>一般在自身上，不用传参</summary>
    void CollidePlayer();//
}
public interface ICollidePlane
{
    void CollidePlane(Transform other);
}

public interface ICollideBullet
{
    void CollideBullet(Transform other);
}
#endregion

                                   