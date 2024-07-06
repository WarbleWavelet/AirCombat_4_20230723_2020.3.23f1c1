using LitJson;
using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public interface IReaderUtil : QFramework.IUtility
{
    public IReader GetReader(string path);
}
public class ReaderUtil : IReaderUtil  ,ICanGetUtility
{
    private readonly Dictionary<string, IReader>_readerDic = new Dictionary<string, IReader>();

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    /// <summary>通过路径获取一个填充好数据Configd(json)的IReader</summary>
    public IReader GetReader(string path)
    {
        IReader reader = null;
        if (_readerDic.ContainsKey(path))
        {
            reader = _readerDic[path];
        }
        else
        {
            reader = ReaderConfig.GetReader(path);
            this.GetUtility<ILoadUtil>().LoadConfig(path, reader.SetData);
            if (reader != null)
            {
                _readerDic[path] = reader;
            }
            else
            { 
                Debug.LogError("ReaderMgr未获取到对应reader，路径：" + path);
            }
        }

        return reader;
    }

}

#region ReaderConfig
public class ReaderConfig
{
    private static readonly Dictionary<string, Func<IReader>>_readerDic = new Dictionary<string, Func<IReader>>
    {
        { ".json", () => new JsonReader()}
    };

    public static IReader GetReader(string path)
    {
        foreach (var pair in _readerDic)
        {
            if (path.Contains(pair.Key))
            {
                return pair.Value();
            }
        }

        Debug.LogError("未找到对应文件的读取器，文件路径：" + path);
        return null;
    }
}
#endregion


#region 接口

public interface IReader
{
    IReader this[string key] { get; }
    IReader this[int key] { get; }
    void Count(Action<int> callBack);
    void Get<T>(Action<T> callBack);
    void SetData(object data);
    ICollection<string> Keys();
}


//jsonReader["planes"][0]["planeId"].GetOut<int>(()=>)
//jsonReader["planes"][0]["planeId"]
public class JsonReader : IReader
{
    private JsonData _jsonData;
    private readonly Queue<KeyQueue> _keyQueues = new Queue<KeyQueue>();
    private KeyQueue _keyQueue;
    private JsonData _tmpJsonData;

    public IReader this[int key]
    {
        get
        {
            SetReader(key, () => _tmpJsonData = _tmpJsonData[key]);
            return this;
        }
    }

    public IReader this[string key]
    {
        get
        {
            SetReader(key, () => _tmpJsonData = _tmpJsonData[key]);
            return this;
        }
    }


    #region 辅助 public


    public void Count(Action<int> callBack)
    {
        var success = SetKey<Action>(() =>
        {
            callBack.DoIfNotNull(GetCount());
        });

        if (!success)
        {
            callBack(GetCount());
        }
        else
        {
            _keyQueues.Enqueue(_keyQueue);
            _keyQueue = null;
        }
    }

    public void Get<T>(Action<T> callBack)
    {
        if (_keyQueue != null)
        {
            _keyQueue.OnComplete(dataTemp =>
            {
                var value = GetValue<T>(dataTemp);
                ResetData();
                callBack(value);
            });

            _keyQueues.Enqueue(_keyQueue);
            _keyQueue = null;
            ExecuteKeysQueue();
            return;
        }

        if (callBack == null)
        {
            Debug.LogWarning("当前回调方法为空，不返回数据");
            ResetData();
            return;
        }

        var data = GetValue<T>(_tmpJsonData);
        ResetData();
        callBack(data);
    }

    public void SetData(object data)
    {
        if (data is string)
        {
            _jsonData = JsonMapper.ToObject(data as string);
            ResetData();
            ExecuteKeysQueue();
        }
        else
        {
            Debug.LogError("当前传入数据类型错误，当前类只能解析json");
        }
    }

    public ICollection<string> Keys()
    {
        if (_tmpJsonData == null)
            return new string[0];

        return _tmpJsonData.Keys;
    }
    #endregion


    #region 辅助 pri

    void SetReader<T>(T key, Action action)
    {
        if (!SetKey(key))
        {
            try
            {
                action.DoIfNotNull();
            }
            catch (Exception e)
            {
                Debug.LogError($"在数据中无法找到对应键值，数据：({key.GetType().Name}){_tmpJsonData.ToJson()}" +
                    $"\t 键值：{key}");
            }
        }
    }




    private bool SetKey<T>(T key)
    {
        if (_jsonData == null || _keyQueue != null)
        {

            IKey keyData = new Key(key);

            if (_keyQueue == null)
            {
                _keyQueue = new KeyQueue();
            }
            _keyQueue.Enqueue(keyData);


            return true;
        }

        return false;
    }

    private int GetCount()
    {
        return _tmpJsonData.IsArray ? _tmpJsonData.Count : 0;
    }

    private void ExecuteKeysQueue()
    {
        if (_jsonData == null)
            return;

        IReader reader = null;
        foreach (var keyQueue in _keyQueues)
        {
            foreach (var value in keyQueue)
            {
                if (value is string)
                    reader = this[(string)value];
                else if (value is int)
                    reader = this[(int)value];
                else if (value is Action)
                    ((Action)value)();
                else
                    Debug.LogError("当前键值类型错误");
            }


            keyQueue.Complete(_tmpJsonData);
        }

        _keyQueues.Clear();
    }

    private T GetValue<T>(JsonData data)
    {
        var converter = TypeDescriptor.GetConverter(typeof(T));

        try
        {
            if (converter.CanConvertTo(typeof(T)))
                return (T)converter.ConvertTo(data.ToString(), typeof(T));
            return (T)(object)data;
        }
        catch (Exception e)
        {
            Debug.LogError("当前类型转换出现问题，目标类型为：" + typeof(T).Name + "  data:" + data);
            return default(T);
        }
    }

    private void ResetData()
    {
        _tmpJsonData = _jsonData;
    }
    #endregion

}


#region Key


public class KeyQueue : IEnumerable
{
    private Action<JsonData> _complete;
    private readonly Queue<IKey> _keyQueue = new Queue<IKey>();



    public IEnumerator GetEnumerator()
    {
        foreach (var key in _keyQueue)
        {
            yield return key.Get();
        }
    }



    #region 辅助


    public void Enqueue(IKey key)
    {
        _keyQueue.Enqueue(key);
    }

    public void Dequeue()
    {
        _keyQueue.Dequeue();
    }

    public void Clear()
    {
        _keyQueue.Clear();
    }

    public void Complete(JsonData data)
    {
        _complete.DoIfNotNull(data);
    }

    public void OnComplete(Action<JsonData> complete)
    {
        _complete = complete;
    }
    #endregion

}

public interface IKey
{
    Type KeyType { get; }
    void Set<T>(T key);
    object Get();
}

public class Key : IKey
{
    private object _key;
    public Type KeyType { get; private set; }

    public Key(object key)
    {
        _key = key;
    }

    public Key()
    {

    }


    public void Set<T1>(T1 key)
    {
        _key = key;
    }

    public object Get()
    {
        return _key;
    }
}
#endregion

#endregion