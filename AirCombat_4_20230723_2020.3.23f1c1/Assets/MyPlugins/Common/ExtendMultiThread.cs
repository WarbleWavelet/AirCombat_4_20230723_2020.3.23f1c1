/****************************************************
    文件：ExtendMultiThread.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/18 12:1:37
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;
 
public static class ExtendMultiThread
{

    /// <summary>
    /// 在Unity中看不出打印先后<para/>
    /// https://www.bilibili.com/video/BV16G4y1c72R/?spm_id_from=autoNext&vd_source=54db9dcba32c4988ccd3eddc7070f140 
    /// </summary>
    public static void Example()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Cook();
        if (Input.GetKeyDown(KeyCode.W)) CookByThread();
        if (Input.GetKeyDown(KeyCode.E)) CookByTask();
        if (Input.GetKeyDown(KeyCode.R)) CookBy2Task();
        if (Input.GetKeyDown(KeyCode.T)) CookBy2TaskAndAfter();
        if (Input.GetKeyDown(KeyCode.Y)) CookByTasks();
    }


    #region pri
    /// <summary>
    /// <para/>在unitu中同时显示
    /// </summary> 
    static void Cook()
    {
        Thread.Sleep(2);
        Debug.Log("素菜做好了");
        Thread.Sleep(2);
        Debug.Log("荤菜做好了");

    }


    /// <summary>在unitu中都不显示</summary>
    static void CookByThread()
    {
        Thread thread = new Thread(()=>
        {
            Thread.Sleep(2);
            Debug.Log("素菜做好了");
            Thread.Sleep(2);
            Debug.Log("荤菜做好了");
        });


    }


    /// <summary>在unitu中同时显示</summary>
    static void CookByTask()
    {
        Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("素菜做好了");
            Thread.Sleep(2);
            Debug.Log("荤菜做好了");
        });
    }


    /// <summary>在unitu中同时显示</summary>
    static void CookBy2Task()
    {
        Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("素菜做好了");
        });
        Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("荤菜做好了");
        });
    }

    /// <summary>在unitu中同时显示</summary>
    static async void CookBy2TaskAndAfter()
    {
        await Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("素菜做好了");
        });
        await Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("荤菜做好了");
        });

        Debug.Log("都做好了");
    }
    /// <summary>在unitu中同时显示</summary>
    static async void CookByTasks()
    {
        List<Task> taskLst = new List<Task>();
        taskLst.Add( Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("素菜做好了");
        }));
        taskLst.Add(Task.Run(() =>
        {
            Thread.Sleep(2);
            Debug.Log("荤菜做好了");
        }));

        Task.WhenAll(taskLst).ContinueWith(para => 
        {
            Debug.Log("都做好了");
        });
    }
    #endregion  




}





