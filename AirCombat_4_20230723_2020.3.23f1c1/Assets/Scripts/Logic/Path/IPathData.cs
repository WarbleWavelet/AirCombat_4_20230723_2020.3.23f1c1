/****************************************************
    文件：IPathData.cs
	作者：lenovo
    邮箱: 
    日期：2024/6/11 18:54:48
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#region IPathData 
/// <summary>轨迹</summary>
public interface IPathData : IJsonPath
{


}

/// <summary>为了PathName</summary>
public abstract class PathDataBase : IPathData
{
    public string ConfigPath()
    {
        return ResourcesPath.CONFIG_ENEMY_TRAJECTORY;

    }
}
#endregion



#region PathDataMgr


/// <summary>敌人轨迹Mgr</summary>
public class PathDataMgr
{
    private int _pathID;
    public Dictionary<TrajectoryType, IPathData[]> PathDataDic;
    private Func<TrajectoryType, IPathData> _getAction;

    public void Init(int pathID)
    {
        this._pathID = pathID;
        {
            //_getAction = (_coroutineID < 0)//这会报错
            //    ? GetRandomData
            //    : GetOneData;

            if (this._pathID < 0)
            {
                _getAction = GetRandomData;
            }
            else
            {
                _getAction = GetOneData;
            }
        }

    }



    #region pub



    public IPathData GetData(TrajectoryType type)
    {
        if (_getAction == null)
        {
            Debug.LogError("当前数据为初始化，请先调用Init方法");
            return null;
        }
        else
        {
            return _getAction(type);
        }
    }

    private IPathData GetRandomData(TrajectoryType type)
    {
        if (!PathDataDic.ContainsKey(type))
            return null;

        int count = PathDataDic[type].Length;
        if (count > 0)
        {

            int index = Random.Range(0, count);
            return PathDataDic[type][index];
        }
        else
        {
            Debug.LogError("当前的轨迹数组长度为0");
            return null;
        }
    }

    private IPathData GetOneData(TrajectoryType type)
    {
        if (!PathDataDic.ContainsKey(type))
            return null;

        if (_pathID < PathDataDic[type].Length)
        {
            return PathDataDic[type][_pathID];
        }
        else
        {
            Debug.LogError("当前ID不存在，id：" + _pathID);
            return null;
        }
    }
    #endregion
}
#endregion


#region IPathData,实例类



/// <summary>直线,</summary>
public class StraightPathData : PathDataBase
{
    /// <summary>Degree角度</summary>
    public double Angle;

    public override string ToString()
    {
        string str = "";
        str += "\t" + Angle;

        return str;
    }
}


/// <summary>椭圆</summary>
public class EllipsePathData : PathDataBase
{
    /// <summary>x轴半径</summary>
    public double XRadius;
    /// <summary>y轴半径</summary>
    public double YRadius;
    /// <summary>椭圆圆心</summary>
    public Vector2 Center;      
    /// <summary>一般是子弹生出处,即在四各个轴点中</summary>
    public Vector2 StartPos;
    //
    public float XPos;

    public double YRatioInScreen;
    /// <summary>精确度, 指定椭圆形取用多少点作为路径点  </summary>
    public int Precision;


    public override string ToString()
    {
        string str = "";
        str += "\t" + XPos;
        str += "\t" + StartPos;
        str += "\t" + Center;
        str += "\t" + YRatioInScreen;
        str += "\t" + XRadius;
        str += "\t" + YRadius;
        str += "\t" + Precision;

        return str;
    }
}

/// <summary>W看到Config中这个类型</summary>
public class WPathData : PathDataBase
{
    public double Angle;

    public override string ToString()
    {
        string str = "";
        str += "\t" + Angle;

        return str;
    }
}

/// <summary>V型</summary>
public class VPathData : PathDataBase
{
    public double Angle;
    public float[] XPos;


    public override string ToString()
    {
        string str = "";
        str += "\t" + Angle;
        str += "\t" + XPos;

        return str;
    }
}


/// <summary>旋转</summary>
public class RotatePathData : PathDataBase
{
    public double StartAngle;
    public double EndAngle;
    public double RotateOffset;

    public override string ToString()
    {
        string str = "";
        str += "\t" + StartAngle;
        str += "\t" + EndAngle;
        str += "\t" + RotateOffset;

        return str;
    }
}
#endregion  



