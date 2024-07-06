using HutongGames.PlayMaker;
using QFramework.AirCombat;
using System;
using System.Linq;
using UnityEngine;


/// <summary>预制体的路径和优先级</summary>
[AttributeUsage(AttributeTargets.Class)]
public class BindPrefabAttribute : Attribute
{
    public BindPrefabAttribute(string path, int priority = 100)
    {
        Path = path;
        Priority = priority;
    }

    public string Path { get; }
    public int Priority { get; }
    //public static string GetPath() 
    //{
    //    var type = typeof(Attribute);
    //    var attribute = type.GetCustomAttributes(typeof(BindPrefabAttribute), false).FirstOrDefault();
    //    if (attribute == null)
    //    {
    //        return null;
    //    }
    //    return ((BindPrefabAttribute)attribute).Path;
    //}
}