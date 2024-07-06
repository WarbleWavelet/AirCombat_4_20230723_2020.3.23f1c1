using QFramework;
using System;
using UnityEngine;
using Object = UnityEngine.Object;


public interface ILoadUtil : QFramework.IUtility
{
    void LoadConfig(string path, Action<object> complete);
    T Load<T>(string path) where T : Object;
    T[] LoadAll<T>(string path) where T : Object;
}


public class LoadUtil : AbstractSystem, ILoadUtil
{

    #region ◊÷ Ù
    //  [SerializeField] private readonly ILoader _loader;
    private ILoader _loader;

    public ILoader Loader
    {
        get
        {
            if (_loader.IsNull())
            {
                _loader = new ResourceLoader();
            }
            return _loader;
        }
        set 
        { 
            _loader = value; 
        }
    }

    //private ResLoader _loader;
    #endregion


    protected override void OnInit()
    {
       _loader = new ResourceLoader();
       // _loader = ResLoader.Allocate();
    }
    #region pub



    public void LoadConfig(string path, Action<object> complete)
    {
        CheckLoader();
        Loader.LoadConfig(path, complete); 
    }



    public T Load<T>(string path) where T : Object
    {
        return Loader.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Loader.LoadAll<T>(path);
    }
    #endregion  


    #region pri
    void CheckLoader()
    {
        if (Loader.IsNull()) throw new System.Exception("ILoader“Ï≥£");
    }
    #endregion
}

