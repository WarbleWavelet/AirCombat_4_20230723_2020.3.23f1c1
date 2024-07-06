/****************************************************
    文件：ExtendDateTime.cs
	作者：lenovo
    邮箱: 
    日期：2024/1/29 19:20:14
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 

public static partial class ExtendDateTime 
{
    static void A()
    {

        Console.WriteLine(DateTime.Now);        //2024/1/29 19:21:15
        Console.WriteLine(DateTime.MinValue);   //0001/1/1 0:00:00
        Console.WriteLine(DateTime.MaxValue);   //9999/12/31 23:59:59
        Console.WriteLine(DateTime.UtcNow);     //2024/1/29 11:22:22
    }

}



