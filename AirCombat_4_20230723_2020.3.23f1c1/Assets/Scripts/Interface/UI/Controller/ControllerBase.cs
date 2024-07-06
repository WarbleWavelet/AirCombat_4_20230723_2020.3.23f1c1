using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ControllerBase : MonoBehaviour, IController
{

    private List<IControllerInit> _initLst;
    private List<IControllerShow> _showLst;
    private List<IControllerHide> _hideLst;
    private List<IControllerUpdate> _updateLst;
    private Action _onUpdate;


    #region IController
    public virtual void Init()
    {
        InitChild();
        InitAllComponents();
        InitComponents();
        AddUpdateAction();
    }



    public void Reacquire()
    {
        InitInterface();
        InitComponents();
    }

    public virtual void Show()
    {
        _showLst.ForEach(show => show.Show());
    }

    public virtual void Hide()
    {
       _hideLst.ForEach(hide => hide.Hide());
    }

    #region IUpdate


    public int Framing { get; set; }

    public int Frame { get; }

    public virtual void FrameUpdate()
    {
        _updateLst.ForEach(update=>update.FrameUpdate());
    }
    #endregion  



    public void AddUpdateListener(Action update)
    {
        _onUpdate += update;
    }
    #endregion


    protected abstract void InitChild();



    #region pri
    private void AddUpdateAction()
    {
        foreach (var button in GetComponentsInChildren<Button>())
        { 
            button.onClick.AddListener(() =>
            {
                _onUpdate.DoIfNotNull();
                FrameUpdate();
            });        
        }

    }

    private void InitInterface()
    {
        InitComponent(_initLst, this);
        InitComponent(_showLst, this);
        InitComponent(_hideLst, this);
        InitComponent(_updateLst, this);
    }

    private void InitAllComponents()
    {
        _initLst = new List<IControllerInit>();
        _showLst = new List<IControllerShow>();
        _hideLst = new List<IControllerHide>();
        _updateLst = new List<IControllerUpdate>();

        InitInterface();
    }

    private void InitComponent<T>(List<T> components, T removeObject)
    {
        var temp = transform.GetComponentsInChildren<T>();

        components.AddRange(temp);

        components.Remove(removeObject);
    }

    private void InitComponents()
    {
        _initLst.ForEach(component=>component.Init());
    }
    #endregion



}