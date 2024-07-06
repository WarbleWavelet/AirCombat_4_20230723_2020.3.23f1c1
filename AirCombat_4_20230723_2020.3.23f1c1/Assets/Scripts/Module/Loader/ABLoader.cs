using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using UnityEngine;
using Object = UnityEngine.Object;
using ResLoader = QFramework.ResLoader;


/// <summary>
///  原本没内容的 ，欧式throw new NotImplementedException();
///  <para/>尝试用QF的来填充
/// </summary>
public class ABLoader : ILoader ,ICanGetSystem
{
    ResLoader _resLoader;
    public ABLoader()
    {
        ResKit.Init();
        _resLoader = ResLoader.Allocate();
    }
    ~ABLoader()
    {
        _resLoader.Dispose();
    }

    public GameObject LoadPrefab(string path)
    {
        string abName = path.TrimName(TrimNameType.SlashAfter);
        GameObject prefab = _resLoader.LoadSync<GameObject>(abName);
        return prefab;
    }

    public GameObject LoadAndInstantiateGameObject(string path, Transform parent = null)
    {
        string abName = path.TrimName(TrimNameType.SlashAfter);
        GameObject prefab = _resLoader.LoadSync<GameObject>(abName);
        GameObject go = GameObject.Instantiate(prefab, parent);

        return go;
    }


    #region pub pri 照抄ResourceLoader的

    #endregion
    public void LoadConfig(string path, Action<object> complete)
    {
        this.GetSystem<ICoroutineSystem>().StartOutter(GetConfigByWWW(path, complete));
    }

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
        var sprites = Resources.LoadAll<T>(path);
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogError($"ResourceLoader当前路径下未找到对应资源，路径：{path}");
            return null;
        }

        return sprites;
    }

    private IEnumerator GetConfigByWWW(string path, Action<object> onComplete)
    {
        if (Application.platform != RuntimePlatform.Android)
        {
            path = $"file://{path}";
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

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}