/****************************************************
    文件：ExtndQFramework.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/14 15:36:19
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using QFramework;

namespace QFramework
{
    /**  构建架构
     * Kit>Manager.Instance
     */
    /**  用法架构
     *   Model，数据共享
     *   View
     *   Controller，
     *   Command，拆分于Controller
     *   Event ，广播
     *   System，Command的交集
     *   Query，Command
     *   Util，Model的读写
     */
    /**
     *  对象池
     *  对象工厂
     */

    public static partial class ExtendQFramework  //ActionKit
    {
        public static void Example_ActionKit( MonoBehaviour mono,float delay,Action cb)
        {
            mono.DelayStart(delay, cb);

        }

        public static void DelayStart(this MonoBehaviour mono, float delay, Action cb)
        {
            ActionKit
                .Delay(delay, cb)
                .Start(mono);

        }
    }


    public static partial class ExtendQFramework  //pool
    {
         
        public static void Example_Pool()
        {
            SimpleObjectPoolExample simpleObjectPoolExample;
          //  SafeObjectPool<PoolObjectBase> safeObjectPool;
           // NonPublicObjectPool<PoolObjectBase> nonPublicObjectPool;

        }
    }
    public static partial class ExtendQFramework  //读取资源和示例
    {
        public static void Example(Transform parent)
        {
            LoadAndInstantiateGameObject("WhiteChess", parent);
        }

        public static GameObject LoadAndInstantiateGameObject(string likeWhiteChess
            , Transform parent)
        {
            //在扫雷案例中测试的，用到WhiteChess
            ResKit.Init();
            ResLoader loader = ResLoader.Allocate();
            GameObject prefab = loader.LoadSync<GameObject>(likeWhiteChess);
            GameObject go = GameObject.Instantiate(prefab, parent);
            return go;

        }

        public static Transform LoadAndInstantiateGameObject(this Transform parent
            , string likeWhiteChess
            , int count)
        {
            //在扫雷案例中测试的，用到WhiteChess
            ResKit.Init();
            ResLoader loader = ResLoader.Allocate();
            GameObject prefab = loader.LoadSync<GameObject>(likeWhiteChess);
            for (int i = 0; i < count; i++)
            {
                GameObject go = GameObject.Instantiate(prefab, parent);
            }
            return parent;

        }

        public static GameObject LoadGameObject(this string likeWhiteChess)
        {
            //在扫雷案例中测试的，用到WhiteChess
            ResKit.Init();
            ResLoader loader = ResLoader.Allocate();
            GameObject go = loader.LoadSync<GameObject>(likeWhiteChess);
            return go;

        }
    }

    public static partial class ExtendQFramework     //Audio
    {
        static void Example_Audio()
        {
            string path ="";
            string clipName ="";
            AudioKit.PlayMusic("resources://"+path+clipName);//""路径格式相当于Resources.LoadTime();
        }
    }
}



