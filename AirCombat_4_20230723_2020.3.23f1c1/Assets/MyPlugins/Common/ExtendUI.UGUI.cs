/****************************************************
    文件：ExtendUGUI.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/25 11:55:7
	功能：UGUI,现在UI系统
*****************************************************/

//using log4net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public static partial class ExtendUGUI//CanvasSize观察坐标
{
    public static Vector2Int CanvasSizeInt(this Canvas canvas)
    {
        Vector2 v = canvas.CanvasSize();
        int x = (int)v.x;
        int y = (int)v.y;
        return new Vector2Int(x, y);
    }

    /// <summary>canvas中心为原点</summary>
    public static Vector2 CanvasSize(this Canvas canvas)
    {
        //canvas.GetComponent<Rect>()  这种报错
        float x = canvas.GetComponent<RectTransform>().rect.xMax*2f; 
        float y = canvas.GetComponent<RectTransform>().rect.yMax*2f;

        return new Vector2(x, y);
    }
}
public static partial class ExtendUGUI
{
    /*  https://blog.uwa4d.com/archives/fillrate.html
        Fill Rate(填充率)是指显卡每帧每秒能够渲染的像素数。
        在每帧绘制中，如果一个像素被反复绘制的次数越多，那么它占用的资源也必然更多。
        目前在移动设备上，FillRate 的压力主要来自半透明物体。
        因为多数情况下，半透明物体需要开启 Alpha Blend 且关闭 ZTest和 ZWrite，
        同时如果我们绘制像 alpha=0 这种实际上不会产生效果的颜色上去，也同样有 Blend 操作，这是一种极大的浪费。
        因此，今天我们为大家推荐两则UGUI 降低填充率的技巧，希望大家能受用。

        在Unity中，与能直接看到的Verts/Tris/Batches数据不同，填充率并不能被直接统计到
    */

    public static void Example()
    {
        //QFramework.Empty4Raycast();

    }


    /// <summary>
    /// 从上到下排序
    /// <para/>需要预制体已经设置好PosY的值
    /// <para/>（预制体的Posy=xxx，实力出来的第一个就是Pos=xxx）
    /// </summary> 
    public static RectTransform VerticalGroup(this RectTransform rect,int idx)
    {
        float offset = rect.rect.height * idx;
        rect.anchoredPosition -= offset * Vector2.up;

        return rect;
    }


}
public static partial class ExtendUGUI //Slider
{
    public static Slider AddValueChangeListener(this Slider slider, Action<float> action)
    {
        slider.onValueChanged.AddListener( change=>action(change));
        return slider;
    }

    public static Slider RemoveValueChangeListener(this Slider slider, Action<float> action)
    {
        slider.onValueChanged.RemoveListener(change => action(change));
        return slider;
    }
}
//public static partial class ExtendUGUI



