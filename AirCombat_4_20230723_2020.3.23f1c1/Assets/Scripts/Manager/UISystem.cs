using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Object = UnityEngine.Object;


public interface IUISystem : QFramework.ISystem
{
    RectTransform Common { get; set; }
    void AddShowListener(Action<string> showAction);
    void AddHideListener(Action<string> hideAction);
    RectTransform Open(string tarPath);
    RectTransform PopPanel(string tarPath);
    RectTransform HidePopPanel();
    void ClosePopPanel();
    void Close(string tarPath);
    void Close(MonoBehaviour mono);
    void CloseAll();
    void Back(string panelName);
    void Back<T>();
    void Back();

    void Hide(string path);
    RectTransform GetViwePrefab(string path);
    RectTransform GetCurrentViewPrefab();

}

public class UISystem : AbstractSystem, IUISystem
{

    #region 字属


    /// <summary>只有LoadingView路径的哈希集</summary>

    private readonly HashSet<string> _loadingHash = new HashSet<string>
    {
        ResourcesPath.PREFAB_LOADING_VIEW
    };
    //
     RectTransform IUISystem.Common { get { return _common; }  set { _common = value; } }
     RectTransform _common {  get;  set; }
    //以下的两个是Common的，弹出框这种不算进入
    /// <summary>面板的路径。用来保存堆叠面板(优先)，实现返回之类的功能</summary>
    private static  Stack<string> _uiStack = new Stack<string>();
    /// <summary>（面板路径，面板类）。一个总的集合</summary>
    private static  Dictionary<string, RectTransform> _rectDic = new Dictionary<string, RectTransform>();
    //
    /// <summary>一般只有一个</summary>
    RectTransform _popPanel {  get;  set; }
    //

    private Action<string> _showAction;
    private Action<string> _hideAction;
    #endregion  



    protected override void OnInit()
    {
        Transform root =GameObject.Find(GameObjectName.UIRoot).transform;
        _common = root.Find(GameObjectName.Common).Rect();
    }


    #region pub IUISystem
    public void AddShowListener(Action<string> showAction)
    {
        _showAction += showAction;
    }

    public void AddHideListener(Action<string> hideAction)
    {
        _hideAction += hideAction;
    }


    /// <summary>
    /// 打开面板 
    /// <para />类似于Prefab/UI/LoadingView （Resources下）
    /// </summary>        
    public RectTransform Open(string tarPath)
    {
        //判断已经是了
        if (_uiStack.Count > 0)
        {
            RectTransform topRect = IsTopPanel(tarPath);
            if (topRect != null)
            {
                return topRect;
            }
        } 
        //判断是否可以Back

        if (_uiStack.Count > 1)
        {
            RectTransform backRect = Back(tarPath);
            if (backRect != null) 
                return backRect;
        }
        // 处理旧的
        if (_uiStack.Count > 0)
        {
            string curPath = _uiStack.Peek();
            RectTransform curRect= _rectDic[curPath];
            HideIfCan(curRect);
        }
        //处理新的
        RectTransform tarRect = InitPanel(tarPath);
        _rectDic.Add(tarPath,tarRect);
        _uiStack.Push(tarPath);
        //
        InitIfCan(tarRect);
        ShowIfCan(tarRect);
        //_showAction.DoIfNotNull(tarPath);


        return tarRect;
    }


    #region PopPanel
    public RectTransform PopPanel(string tarPath)
    {
        if (_popPanel == null)
        {
            _popPanel = InitPanel(tarPath);
        }
        //
        InitIfCan(_popPanel);
        ShowIfCan(_popPanel);
        return _popPanel;
    }

    public RectTransform HidePopPanel()
    {
        if (_popPanel != null)
        {     
            HideIfCan(_popPanel);
            _popPanel.Hide();
        }
      
        return _popPanel;
    }
    #endregion

    #region IfCan


    void InitIfCan(RectTransform tarRect)
    {
        IInit init = tarRect.GetComponent<IInit>();
        if (init != null)
        {
            init.Init();
        }
        //

    }
    void ShowIfCan(RectTransform tarRect)
    {
        tarRect.Show();
        IShow show = tarRect.GetComponent<IShow>();
        if (show != null)
        {
            show.Show();
        }
    }

    void HideIfCan(RectTransform tarRect)
    {
        
        IHide hide = tarRect.GetComponent<IHide>();
        if (hide != null)
        {
            hide.Hide();
        }
        tarRect.Hide();
    }

    void HideIfCan(string tarPath)
    {
        RectTransform tarRect = _rectDic[tarPath];
        HideIfCan(tarRect);
    }
    #endregion


    public RectTransform GetViwePrefab(string path)
    {
        if (_rectDic.ContainsKey(path))
        {
            return _rectDic[path];
        }
        else
        {
            try
            {
                return Open(path);
            }
            catch (Exception)
            {
                Debug.LogError("当前预制体未在在uimanager管理当中,路径:" + path);
                return null;
            }
           

        }
    }

    public RectTransform GetCurrentViewPrefab()
    {
        var name = _uiStack.Peek();
        return GetViwePrefab(name);
    }



    #endregion  



    #region pri



    private RectTransform InitPanel(string path)
    {
        if (_rectDic.ContainsKey(path))
        {
            return _rectDic[path];
        }
        GameObject viewGo = this.GetSystem<IGameObjectSystem>().Instantiate(path, LoadType.RESOURCES);

        InitUILevel(path, viewGo.transform);//加UILevel
        AddTypeComponent(viewGo, path);//加组件
        //AddUpdateListener(viewGo);//可选 Update
        //AddInit(viewGo); //
        //ShowAll(viewGo.RectTransform());
        viewGo.Rect().Stretch();
        //
        return viewGo.Rect();
    }

    private void InitUILevel(string path, Transform view)
    {
        this.GetSystem<IUILevelSystem>().SetParent(path, view);
    }

    private void AddTypeComponent(GameObject viewGo, string path)
    {
        foreach (var type in this.GetUtility<IBindPrefabUtil>().GetType(path))
        {
            viewGo.GetOrAddComponent(type);
        }
    }

    /// <summary>tar的layer高，就隐藏cur(当前最上面的面板)</summary>
    private RectTransform StickIfTopLayer(string tarPath)
    {
        if (_uiStack.Count > 0)
        {
            var curPath = _uiStack.Peek(); //取出
            if (GetUILevel(curPath) >= GetUILevel(tarPath)) //对比
            {
                HideIfCan(curPath);
            }
        }
        //
        RectTransform tarRect = InitPanel(tarPath);
      

        if (!_loadingHash.Contains(tarPath))//?_skipViews
        {
            _uiStack.Push(tarPath);
        }

        return tarRect;
    }

    private UILevel GetUILevel(string path)
    {
        return this.GetSystem<IUILevelSystem>().GetUILevel(path);
    }



    private void ShowAll(RectTransform rect)
    {
        IEntity2D view = rect.GetComponent<IEntity2D>();
        if (view == null)
        {
            return;
        }
        //
        foreach (var show in view.RectTransform.GetComponents<IShow>())
        {
            if (show != null)
            {
                show.Show();
            }
        }
    }
    #endregion

    #region Hide
    public void Hide(string path)
    {
        if (_rectDic.ContainsKey(path))
        { 
        
            RectTransform tarRect = _rectDic[path];
            HideIfCan(tarRect);        
        }

    }

    private void HideAll(RectTransform rect)
    {
        IHide[] hides = rect.GetComponentsInChildren<IHide>();
        if (hides.Length >0)
        {
            foreach (var hide in hides)
            {
                hide.Hide();
            }
        }
    }
    #endregion  

    #region Back


    public RectTransform Back(string tarPath)
    {
        
       string curPath= _uiStack.Pop();
       string backPath= _uiStack.Pop();
        if (backPath == tarPath)//符合Back
        {
            //ABC+D=>ABDC
            _uiStack.Push(curPath);
            _uiStack.Push(tarPath);
            //
            RectTransform curRect = _rectDic[curPath];
            HideIfCan(curRect);
            //
            RectTransform tarRect = _rectDic[tarPath];
            ShowIfCan(tarRect);
            return tarRect;
        }
        else
        {
            _uiStack.Push(backPath);
            _uiStack.Push(curPath);
        }
        return null;

    }

    public RectTransform IsTopPanel(string tarPath)
    {
        string curPath = _uiStack.Peek();
        if (curPath == tarPath)
        {
            return _rectDic[curPath];
        }
        return null;
    }

    public void Back<T>()
    {
        throw new System.Exception("异常");   
    }

    public void Back()
    {
        if (_uiStack.Count <= 1)
            return;

        //隐藏
        var name = _uiStack.Pop();
        Hide(name);
        //显示
        name = _uiStack.Peek();
        ShowAll(_rectDic[name]);
    }

    void IUISystem.Back(string panelName)
    {
        throw new NotImplementedException();
    }
    #endregion

    #region Close


    public void Close(string tarPath)
    {
    
        if (_rectDic.ContainsKey(tarPath))
        {
            RectTransform rect= _rectDic[tarPath];
            HideIfCan(rect);
            _rectDic.Remove(tarPath);
            GameObject.Destroy(rect.gameObject); //GameObject.Despawn(rect不被允许
            if (_uiStack.Contains(tarPath)) //一般在最上面，不管了
            {
                _uiStack.Pop();
            }
        }
    }

    public void Close(MonoBehaviour mono)
    {


    }

    public void CloseAll()
    {
        if (_popPanel != null)
        {
            GameObject.Destroy(_popPanel);
            _popPanel = null;
        }
        //
        _uiStack.Clear();
        foreach (RectTransform item in _rectDic.Values)
        {
            GameObject.Destroy(item.gameObject);
        }
        _rectDic.Clear();
    }
    #endregion


    #region 重写


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    public void ClosePopPanel()
    {
        if (_popPanel != null)
        {
            GameObject.Destroy(_popPanel.gameObject);
        }
    }
    #endregion


}