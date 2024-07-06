/****************************************************
    文件：ExtendReflection01.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/27 2:3:44
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Xxx
{
public class ExtendReflection02
{
    public static void Example(int a)
    {
        Assembly assembly = GetAssemblyCSharp();
        Type selfType = assembly.GetType("Xxx.ExtendReflection02");
        selfType.LogInfo();
    }

    public static Assembly GetAssemblyCSharp()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var a in assemblies)
        {
            if (a.FullName.StartsWith("Assembly-CSharp,"))
            {
                return a;
            }
        }

        Log.E(">>>>>>>Error: Can\'t find Assembly-CSharp.dll");
        return null;
    }
}
}




