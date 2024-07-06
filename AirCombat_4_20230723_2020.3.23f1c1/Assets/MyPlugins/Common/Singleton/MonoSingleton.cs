using UnityEngine;


#region MonoSingleton
/// <summary>
/// 01 与普通单例相比，new时是先new GameObject，然后AddComponent
/// <br />  02 与MonoSingletonSimple相比，用GameObject.Find检查保证唯一性
/// </summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
        protected static T mInstance = null;

        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogWarning("More than 1");
                        return mInstance;
                    }

                    if (mInstance == null)
                    {
                        var instanceName = typeof(T).Name;
                        Debug.LogFormat("Instance Name: {0}", instanceName);
                        var instanceObj = GameObject.Find(instanceName);

                        if (!instanceObj)
                            instanceObj = new GameObject(instanceName);

                        mInstance = instanceObj.AddComponent<T>();
                        DontDestroyOnLoad(instanceObj); //保证实例不会被释放

                        Debug.LogFormat("Add New Singleton {0} in Game!", instanceName);
                    }
                    else
                    {
                        Debug.LogFormat("Already exist: {0}", mInstance.name);
                    }
                }

                return mInstance;
            }
        }

        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }
#endregion







#region MonoSingletonSimple



/// <summary>
/// 可能是想用MonoBehaviour的方法
/// <br/>与MonoSingleton相比，没有用GameObject.Find检查保证唯一性
/// </summary>
public class MonoSingletonSimple<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _single;

    public static T Single
    {
        get
        {
            if (_single == null)
            {
                var go = new GameObject(typeof(T).Name);
                DontDestroyOnLoad(go);
                _single = go.AddComponent<T>();
            }

            return _single;
        }
    }

    protected virtual void OnDestroy()
    {
        _single = null;
    }
}

#endregion  
