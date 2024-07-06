/****************************************************
    文件：ExtendOnGUI.cs
	作者：lenovo
    邮箱: 
    日期：2023/9/11 17:1:33
	功能：OnGUI,在Scene中的
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static partial class ExtendOnGUI
{ 
/*
 *  坐标系是另外开的
 * 左上角为起点
 * W,H分贝时Canvas的W,H
 * 
 */

}
public static partial class ExtendOnGUI  //用css属于来简略位置设置
{



    #region GetGUIPos_Screen 失败,两者不是概念的坐标系
    /**
     * 都是左上原点,所以位置不用加减大小一半
     */     
     static Vector2Int GetGUIPos_Screen(int width, int height, int padding, EDir dir,Camera camera=null)
    {
        return  GetGUIPos_Screen( width,  height,  padding,  padding,  dir);
    }

    /// <summary>不行</summary>
     static Vector2Int GetGUIPos_Screen(int width,int height,int paddingW,int paddingH, EDir dir, Camera camera = null)
    {

        switch (dir)
        {
            case EDir.LEFTTOP :
                {
                    int x =  paddingW;
                    int y =   paddingH;
                    return new Vector2Int(x, y);
                }
            case EDir.RIGHTBOTTOM :
                {
                    Vector2 screen = new Vector2Int( CurResolutionW(),CurResolutionH());
                    camera = Camera.main.MainCameraIfNull();
                    Vector2Int viewPort =camera .ScreenToViewportPoint(screen).V3ToV2Int();
                    int x = viewPort.x - width / 2 - paddingW;
                    int y = viewPort.y - height / 2 - paddingH;

                    return new Vector2Int(x, y);
                }
            default:  throw new System.Exception("异常"); 
        }

    }

    #endregion


    #region GetGUIPos_Canvas
    public static Vector2Int GetGUIPos(int width, int height, int padding, EDir dir, Vector2Int canvasSize)
    {
        // return GetGUIPos(width, height, , padding, dir, canvasSize);//之前少写了个padding溢栈了
        return GetGUIPos(width, height, padding, padding, dir, canvasSize);

    }
    public static Vector2Int GetGUIPos(int width, int height, int paddingW, int paddingH, EDir dir,Vector2Int canvasSize)
    {

        switch (dir)
        {
            case EDir.LEFTTOP:
                {
                    int x = paddingW;
                    int y = paddingH;
                    return new Vector2Int(x, y);
                }
            case EDir.RIGHTBOTTOM:
                {
                    int x = canvasSize.x - width - paddingW;
                    int y = canvasSize.y - height - paddingH;

                    return new Vector2Int(x, y);
                }
            default: throw new System.Exception("异常");
        }

    }

    #endregion
}
public static partial class ExtendOnGUI  //分辨率
{
    /*
     *
     *可以设置远点相对的那一角的坐标
     * 
     */
    public static int CurResolutionW()
    {
        return Screen.currentResolution.width;
    }

    public static int CurResolutionH()
    {
        return Screen.currentResolution.height;
    }
    /// <summary>
    ///  默认是1920*1080
    ///  <br/>因为  Game视图的Resolutin==Canvas的Width和Height != Screen.currentResolution
    ///  <br/>所以要设置统一SetResolution
    /// </summary>
    public static Vector2Int CurResolutionSize()
    {
        return new Vector2Int( Screen.currentResolution.width, Screen.currentResolution.height);
    }
}

public static partial class ExtendOnGUI
{
  static  Rect _rect = new Rect(100, 100, 300, 400);



    public static void Example()
    {
        GUIStyleFunc();
        Window();
        WindowID();
       // ModalWindow();
        DragWindow();
    }


    public static void GUIStyleFunc()
    {

        GUIStyle GUIStyle = new GUIStyle();
        GUISkin GUISkin;

        GUI.color = Color.red;
        GUI.contentColor = Color.green; //文本
        GUI.backgroundColor = Color.green;//Bg
        GUI.skin = null;  //皮肤
        GUI.Button(new Rect(0, 0, 100, 30), "测试按钮");
        GUI.Label(new Rect(0, 50, 100, 30), "测试按钮");


        GUI.color = Color.gray;
        GUI.Button(new Rect(0, 100, 100, 30), "测试按钮", GUIStyle);
    }


    public static void Window()
    {
        GUI.Window(id: 1, new Rect(100, 1000, 150, 200), DrawWindow, "测试窗口");
    }


    public static void WindowID()
    {
        GUI.Window(id: 1, new Rect(100, 1000, 150, 200), DrawWindowID, "测试窗口");
        GUI.Window(id: 2, new Rect(100, 1000, 150, 200), DrawWindowID, "测试窗口");
    }

    /// <summary>ModalWindow存在其他窗口不可操作</summary>
    public static void ModalWindow()
    {
        GUI.Window(id: 1, new Rect(100, 1000, 150, 200), DrawWindowID, "测试窗口");
        GUI.Window(id: 2, new Rect(100, 1000, 150, 200), DrawWindowID, "测试窗口");
        GUI.ModalWindow(id: 3, new Rect(100, 1000, 150, 200), DrawWindowID, "模态窗口");
    }


    public static void DragWindow()
    {
        _rect = GUI.Window(id:4,_rect,DrawWindowID,"拖动窗口");
    }


        #region pri
        static void DrawWindow(int id)
    {
        GUI.Button(new Rect(0, 0, 100, 30), "测试按钮");
    }

    /// <summary>windowID的作用</summary>
    static void DrawWindowID(int id)
    {

        switch (id)
        {
            case 1: GUI.Button(new Rect(0, 20, 100, 30), "测试按钮1"); break;
            case 2: GUI.Button(new Rect(0, 20, 100, 30), "测试按钮2"); break;
            case 3: GUI.Button(new Rect(0, 20, 100, 30), "测试按钮3"); break;
            case 4:
                {   
                    GUI.Button(new Rect(0, 20, 100, 30), "测试按钮4");
                    //GUI.DragWindow(); //拖动窗口的哪部分都可以
                    GUI.DragWindow(new Rect(0, 0, 300, 10)); //可以设置拖动的部分
                } 
                break;
            default:   break;
        }

    }
    #endregion  
}





