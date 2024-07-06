using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceLoader : ILoader  ,ICanGetSystem
{


    #region pub




    public T Load<T>(string path) where T : Object
    {
        var sprite = Resources.Load<T>(path);
        if (sprite == null)
        {
            Debug.LogError("未找到对应图片，路径：" + path);
            return null;
        }

        return sprite;
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        var arr = Resources.LoadAll<T>(path);
        if (arr == null || arr.Length == 0)
        {
            Debug.LogError($"ResourceLoader当前路径下未找到对应资源，路径：{path}" );
            return null;
        }

        return arr;
    }

    public void LoadConfig(string path, Action<object> complete)
    {
        this.GetSystem<ICoroutineSystem>().StartOutter(GetConfigByWWW(path, complete));
    }
    #endregion




    #region pri


    private IEnumerator GetConfigByWWW(string path, Action<object> onComplete)
    {
        if (Application.platform != RuntimePlatform.Android)
        {       
            path = $"file://{path}" ;
        }
        var www = new WWW(path);
        yield return www;


        if (www.error != null)
        {
            Debug.LogError("ResourceLoader加载配置错误，路径为：" + path);
            yield break;
        }


        onComplete(www.text);
        //Debug.Log("文件加载成功，路径为：" + path);
    }


    #endregion
        public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}