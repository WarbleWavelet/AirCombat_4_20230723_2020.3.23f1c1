/****************************************************
    文件：ExtendSingleton.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/31 20:36:0
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;



    public class ExtendSingleton
    {

    void Example()
    {
        Singleton singleton;
        SingletonByLazy singletonByLazy;
    }


    #region 辅助
    class Singleton
        {
            //01 私有构造，防止被new
            private Singleton() { }
            //02 通过Instance来访问
            private static Singleton _instance;

            public static Singleton Instance
            {
                get
                {
                    if (_instance == null)
                    {
                        _instance = new Singleton();
                    }

                    return _instance;
                }
            }
        }


        /// <summary>
        /// 通过Lazy
        /// 访问实例才会创建（）
        ///  创建的线程安全（防止两个发起者同时创建）
        /// </summary>
        class SingletonByLazy
        {
            //01 私有构造，防止被new
            private SingletonByLazy() { }
            public static readonly Lazy<SingletonByLazy> _instance= new Lazy<SingletonByLazy>(
                ()=> new SingletonByLazy());

            public static  SingletonByLazy Instance
            {
                get { return _instance.Value;}
            }

        }
    #endregion  

    }




