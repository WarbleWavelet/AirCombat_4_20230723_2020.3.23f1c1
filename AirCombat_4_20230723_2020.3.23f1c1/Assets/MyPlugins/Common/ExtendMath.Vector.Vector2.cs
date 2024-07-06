/****************************************************
    文件：ExtendUnityOthers.cs
	作者：lenovo
    邮箱: 
    日期：2023/6/20 17:16:44
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Object = UnityEngine.Object;
using System.Runtime.CompilerServices;
using static ExtendVector;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public static partial class ExtendVector2
{ 

}
public static partial class ExtendVector2 //dir
{



}
static partial class ExtendVector2 //长度的  长度的 平方
{

    /// <summary>V2的长度的平方 x2+y2</summary>
    public static float Pow2(this Vector2 v)
    {

        return v.sqrMagnitude;
    }
    /// <summary>V2的长度 (x2+y2).Sqrt()</summary>
    public static float Length(this Vector2 v)
    {
        return v.magnitude;
    }

}
public static partial class ExtendVector2 // 点击 叉积 模 乘  顺逆时针
{



    public static float Mod(this Vector2 v)
    {
        return (v.x.Pow2() + v.y.Pow2()).Sqrt(); 
    }


    #region DotProduct


    /// <summary>ab*cos 相当于缩放 
    /// <br/>标积/内积/数量积/点积
    /// <br/>因为cos有正负,可以判断是在左边还是右边</summary>
    public static float DotProduct(this Vector2 a, Vector2 b,EVectorCalcType vectorCalcType= EVectorCalcType.COORDINATE)
    {

        switch (vectorCalcType)
        {
            case EVectorCalcType.TRIANGLE: return a.DotProduct_Triangle(b);
            case EVectorCalcType.COORDINATE: return a.DotProduct_Coordinate(b);
            default: throw new System.Exception("异常");
        }
    }

    #endregion


    #region CrossProduct
    /// <summary>
    /// absin 
    /// <br/>向量积，数学中又称积、叉积，物理中称矢积、叉乘
    /// <br/>也就是a*a边上的高,1/2 * absin=两个向量围成的三角形的面积
    /// <br/>  以b在x坐标正方向,|b|sin=b对应的高,也就是垂直于|a|上的垂线
    /// 
    /// </summary>
    public static float CrossProduct(this Vector2 a, Vector2 b, EVectorCalcType vectorCalcType= EVectorCalcType.TRIANGLE)
    {

        switch (vectorCalcType)
        {
            case EVectorCalcType.TRIANGLE : return a.CrossProduct_Triangle(b);
            case EVectorCalcType.COORDINATE : return a.CrossProduct_Coordinate(b);
            default: throw new System.Exception("异常");
        }

    }

    #endregion


    #region pri
    static float DotProduct_Coordinate(this Vector2 a, Vector2 b)
    {
        return Vector2.Dot(a, b); //lhs.x * rhs.x + lhs.y * rhs.y;
    }


    static float DotProduct_Triangle(this Vector2 a, Vector2 b)
    {
        return a.Mod() * b.Mod() * (Vector2.Angle(a, b).Degree2Radian()).Cos(); //1-si=cos
    }

    /// <summary>ab*sin 矢积/外积/向量积/叉积</summary>
    static float CrossProduct_Triangle(this Vector2 a,Vector2 b)
    { 
        return  a.Mod() * b.Mod() * Vector2.SignedAngle(a,b).Degree2Radian().Sin();  //sin 
    }

    /// <summary>
    /// 二维叉积,垂直于平面的伪向量
    /// <br/>辅助理解,所以三维的ixj=k,ixk=j,jxk=i就是表示相互垂直
    /// </summary>
     static float CrossProduct_Coordinate(this Vector2 a, Vector2 b)
    {
        //axb=(x1,y1) x (x2,y2)=(x1y2-x2y1)
        float x = a.x * b.y - b.x * a.y;
        return x;
    }


    #endregion



    public static float DegreeOffset(this Vector2 from, Vector2 to)
    {
        float radian = from.DotProduct(to).Acos();
        float degree = radian.Radian2Degree();
        return degree;
    }

    public static float RadianOffset(this Vector2 from, Vector2 to, ExtendCoordinates.ECoordinates coordinates = ExtendCoordinates.ECoordinates.LEFT)
    {
       return RadianOffset(from,to,coordinates);
    }

    /// <summary>
    /// 时针方向
    /// <br/>Unity默认左手坐标系
    /// </summary>
    public static EDir ClockDir(this Vector2 from, Vector2 to, ExtendCoordinates.ECoordinates coordinates = ExtendCoordinates.ECoordinates.LEFT)
    {
        Vector3 radian;
        if (coordinates == ExtendCoordinates.ECoordinates.RIGHT)
        {
            radian = Vector3.Cross(from, to);

        }
        else
        {
            radian = Vector3.Cross(to, from);
        }


        if (radian.z > 0)
        {
            return EDir.CLOCKWISE;
        }
        else if (radian.z < 0)
        {
            return EDir.CONTRACLOCKWISE;
        }
        return EDir.MIDDLECLOCKWISE;
    }


}


public static partial class ExtendVector2 //Set
{
    public static Vector2 SetX(ref this Vector2 v, float value)
    {
        Vector2 pos = v;
        v = new Vector2(value, pos.y);
        return v;
    }



    public static Vector2 SetY( this Vector2 v, float value)
    {
        Vector2 pos = v;
        v = new Vector2(pos.x, value);
        return v;
    }

    public static Vector2 SetXY(ref this Vector2 v, float x, float y)
    {
        Vector2 pos = v;
        v = new Vector3(x, y);
        return v;
    }

    /// <summary>ref直接动v的内存</summary>
    public static Vector2 Mult(ref this Vector2 v, float x, float y)
    {
        v.x *= x;
        v.y *= y;

        return v;
    }
    /// <summary>ref直接动v的内存</summary>
    public static Vector2 Mult(ref this Vector2 v, Vector2 v1)
    {
        return v.Mult(v1.x,v1.y);
    }

}
public static partial class ExtendVector2  //Reversal X  Y 
{

    /// <summary>取反</summary>
    public static Vector2 Reversal(ref this Vector2 dir)
    {
        Vector2 pos= new Vector2(-dir.x, -dir.y);
        dir = pos;
        return dir;
    }

    public static Vector2 ReversalX(ref this Vector2 dir)
    {
        Vector2 pos = new Vector2(-dir.x, dir.y);
        dir = pos;
        return dir;
    }

    public static Vector2 ReversalY(ref this Vector2 dir)
    {
        Vector2 pos = new Vector2(dir.x, -dir.y);
        dir = pos;
        return dir;
    }

}
public static partial class ExtendVector2
{


}
public static partial class ExtendVector2//Vector2Int
{
    public static Vector2Int LeftUp(this Vector2Int v)
    {
        return new Vector2Int(-1,1);
    }

    public static Vector2Int LeftDown(this Vector2Int v)
    {
        return new Vector2Int(-1, -1);
    }

    public static Vector2Int RightUp(this Vector2Int v)
    {
        return new Vector2Int(1, 1);
    }

    public static Vector2Int RightDown(this Vector2Int v)
    {
        return new Vector2Int(1, -1);
    }

}






