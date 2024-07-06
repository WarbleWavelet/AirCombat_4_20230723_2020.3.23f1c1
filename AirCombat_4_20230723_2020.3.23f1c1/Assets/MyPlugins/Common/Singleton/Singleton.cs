using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


/// <summary>找GetConstructors</summary>
public abstract class Singleton<T> where T : Singleton<T>
{
    private static T mInstance;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);

                if (ctor == null)
                    throw new Exception("Non-public ctor() not found!");

                mInstance = ctor.Invoke(null) as T;
            }

            return mInstance;
        }
    }

    protected Singleton()
    {

    }
}

/// <summary>只排除不是MonoBehaviour</summary> 
public class NormalSingleton<T> where T : class, new()
{
    protected static T _single;

    public static T Single
    {
        get
        {
            if (_single == null)//不存在就是实例
            {
                var t = new T();
                if (t is MonoBehaviour)
                {
                    Debug.LogError("Mono类请使用MonoSingleton");
                    return null;
                }

                _single = t;
            }

            return _single;
        }
    }
}



/// <summary>为空就会New()</summary> 
public class SimpleSingleton<T> where T :  new()
{
    protected static T _single;

    public static T Single
    {
        get
        {
            if (_single == null)
            {
                var t = new T();

                _single = t;
            }

            return _single;
        }
    }
}

