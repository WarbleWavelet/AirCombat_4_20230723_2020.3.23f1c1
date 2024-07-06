/****************************************************
    文件：DontDestroyOnLoadSystem.cs
	作者：lenovo
    邮箱: 
    日期：2024/2/15 12:53:51
	功能：
*****************************************************/

using QFramework;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;


public interface IDontDestroyOnLoadSystem : QFramework.ISystem
{
    Transform GetOrAdd(string path);
}
public class DontDestroyOnLoadSystem :QFramework.AbstractSystem, IDontDestroyOnLoadSystem 
{


    #region 字属生命


    private Dictionary<string,Transform> _dic = new Dictionary<string,Transform>();
    protected override void OnInit()
    {
        _dic = new Dictionary<string, Transform>();
        Add(GameObjectName.System);
        Add(GameObjectName.Pool);
        Add(GameObjectPath.Pool_BulletPool);
        //Add(GameObjectName.Mgr);
        Add(GameObjectPath.Creator_EnemyCreator);
    }
    #endregion




    #region pub
    public Transform GetOrAdd(string path)
    {
        if (!path.Contains(StringMark.ForwardSlash))
        {
           return GetOrAddTop(path);
        }
        else
        {
           return GetOrAddDeep( path);
        }
    }

    #endregion  


    #region pri
     Transform GetOrAddTop(string path)
    {
        if (Get(path) != null)
        {
            return Get(path);
        }
        else
        { 
            return Add(path);
        }
    }

     Transform GetOrAddDeep(string path)
    {
        string[] strs = path.Split(CharMark.ForwardSlash);
        string str = strs[0];
        Transform t = AddIfNull(str); //Top节点

        for (int i = 1; i < strs.Length; i++)
        {
            str += StringMark.ForwardSlash + strs[i];
            Transform t2= AddIfNull(str);
            t2.SetParent(t);
            t = t2;
        }

        return t;
    }

    Transform AddIfNull(string str)
    {
        if (Get(str) != null)
        {
            return Get(str);
        }
        else
        {
           return Add(str);
        }
    }


    Transform Get(string path)
    {
        Transform qry;
        if (_dic.TryGetValue(path, out qry))
        {
            return qry;
        }

        return null;
    }

    Transform Add(string path)
    {
        Transform t;

        Transform camera = Camera.main.transform;
        if (!path.Contains(StringMark.ForwardSlash))
        {
            t = camera.FindTopOrNew(path);
        }
        else
        { 
            t = camera.FindTopOrNewPath(path);
        
        }

        t.GetOrAddComponent<DontDestroyOnLoad>();
        _dic[path] = t;
        return t;
    }
    #endregion

}




