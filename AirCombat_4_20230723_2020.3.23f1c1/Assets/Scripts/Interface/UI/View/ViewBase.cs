using System.Collections.Generic;
using UnityEngine;

public abstract class ViewBase : MonoBehaviour, IView
{

    #region 字属


    private UiUtil _uiUtil;
    private List<IViewInit> _viewInitLst;
    private List<IViewShow> _viewShowLst;
    private List<IViewHide> _viewHideLst;
    private List<IViewUpdate> _viewUpdateLst;
    /// <summary>见名知意好点</summary>
    protected UiUtil UiUtil
    {
        get
        {
            _uiUtil = gameObject.GetOrAddComponent<UiUtil>();
            _uiUtil.Init();

            return _uiUtil;
        }
    }
    protected abstract void InitChild();
    #endregion


    #region IView


    public void Reacquire()
    {
        InitInterface();
        InitAllSubView();
    }
    public virtual void Init()
    {
        InitChild();
        InitSubView();
        InitAllSubView();
    }



    public virtual void Show()
    {
        gameObject.Show();
        _viewShowLst.ForEach(view=>view.Show());

        FrameUpdate();
    }

    public virtual void Hide()
    {
        _viewHideLst.ForEach(view=>view.Hide());
        gameObject.Hide();
    }

     
    public int Framing { get; set; }

    public int Frame { get; }
    public virtual void FrameUpdate()
    {
        _viewUpdateLst.ForEach( view=>view.FrameUpdate() );
    }
    #endregion


    #region pri
    private void InitSubView()
    {
        _viewInitLst = new List<IViewInit>();
        _viewShowLst = new List<IViewShow>();
        _viewHideLst = new List<IViewHide>();
        _viewUpdateLst = new List<IViewUpdate>();

        InitInterface();
    }     
    
    private void InitAllSubView()
    {
        _viewInitLst.ForEach(view=> view.Init()) ;
    }
    private void InitInterface()
    {
        InitViewInterface(_viewInitLst);
        InitViewInterface(_viewShowLst);
        InitViewInterface(_viewHideLst);
        InitViewInterface(_viewUpdateLst);
    }
    private void InitViewInterface<T>(List<T> list)
    {
        T view;
        foreach (Transform trans in transform)
        {
            view = trans.GetComponent<T>();
            if (view != null)
                list.Add(view);
        }

    }
    #endregion
}