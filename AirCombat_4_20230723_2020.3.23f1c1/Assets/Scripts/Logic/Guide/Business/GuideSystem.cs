using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using TestDemo;
using UnityEngine;
using UnityEngine.UI;



#region #region GuideMgrBase<T>



#region GuideMgrBase<T>


public abstract class GuideMgrBase<T> :SimpleSingleton<T> where T : new()
{


    private Dictionary<string, IGuideRoot> _guideviewDic;
    private IGuideRoot _curGuide;



    #region pub
    public virtual void InitGuide()
    {
        _guideviewDic = GetViweGuides();
    }

    public void Update()
    {
        if (_curGuide != null)
            _curGuide.Update();
    }
    #endregion


    #region pro
    protected abstract Dictionary<string, IGuideRoot> GetViweGuides();

    protected void ShowUI(string name)
    {
        if (_guideviewDic.ContainsKey(name))
        {
            _curGuide = _guideviewDic[name];
            _curGuide.OnEnter();
        }
    }

    protected void HideUI(string name)
    {
        if (_curGuide == null)
        {
            return;
        }
        if (_curGuide.GetViewName() == name)
        {
            _curGuide = null;
        }

    }
    #endregion
}

#endregion


#region TestDemo  GuideMgr
namespace TestDemo
{


    public class GuideMgr : GuideMgrBase<GuideMgr>, IUpdate, ICanGetSystem
    {
        public override void InitGuide()
        {
            base.InitGuide();
            this.GetSystem<IUISystem>().AddShowListener(ShowUI);
            this.GetSystem<IUISystem>().AddHideListener(HideUI);
            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
        }

        protected override Dictionary<string, IGuideRoot> GetViweGuides()
        {
            Dictionary<string, IGuideRoot> dic = new Dictionary<string, IGuideRoot>();
            IGuideRoot guide = new StartViewGuide();
            dic.Add(guide.GetViewName(), guide);

            return dic;
        }



        #region IUpdate
        public int Framing { get; set; }
        public int Frame { get; }

        public void FrameUpdate()
        {
            base.Update();
        }
        #endregion  


        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
    }
}
#endregion


#region GuideMgr

public class GuideMgr : GuideMgrBase<GuideMgr>, IUpdate	,ICanGetSystem 
{
    public override void InitGuide()
    {
        base.InitGuide();
        this.GetSystem<IUISystem>().AddShowListener(ShowUI);
        this.GetSystem<IUISystem>().AddHideListener(HideUI);
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
    }
    protected override Dictionary<string, IGuideRoot> GetViweGuides()
	{
		Dictionary<string, IGuideRoot> dic = new Dictionary<string, IGuideRoot>();
		foreach (IGuideRoot root in GuideDataConfig.GUIDE_ROOTS)
		{
			dic.Add(root.GetViewName(),root);
		}
		return dic;
	}



    #region IUpdate
    public int Framing { get; set; }
	public int Frame { get; }

	public void FrameUpdate()
	{
		base.Update();
	}


    #endregion

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

}
#endregion
#endregion





#region GuideStorageUtil

public interface IGuideStorageUtil : QFramework.IUtility,IGetBool,ISetIntKEV 
{

}

public class GuideStorageUtil : IGuideStorageUtil
{
    public void SaveData(int key, bool value = true)
    {
        PlayerPrefs.SetInt(key.ToString(), Convert.ToInt32(value));
    }



    public bool GetBool<T>(T key)
    {
        int result = PlayerPrefs.GetInt(key.ToString(), Convert.ToInt32(false));
        return Convert.ToBoolean(result) ;
    }

    public void SetInt( int kv)
    {
        PlayerPrefs.SetInt(kv.ToString(), Convert.ToInt32(kv));
    }


}
#endregion


#region GuideBase


public interface IGuide
{
    void OnEnter(Action callBack = null);
    void Update();
}

public interface IGuideRoot : IGuide
{
    string GetViewName();
}

public abstract class GuideBase : IGuideRoot
{
    private Queue<IGuideGroup> _groups;
    protected abstract int GuideId { get; }

    public virtual void OnEnter(Action callBack = null)
    {
        _groups = GetGroups();
        ExecuteChildEnter();
    }

    public abstract string GetViewName();

    public void Update()
    {
        if (_groups == null)
            return;

        foreach (IGuideGroup guideGroup in _groups)
        {
            guideGroup.Update();
        }
    }

    protected abstract Queue<IGuideGroup> GetGroups();

    private void ExecuteChildEnter()
    {
        foreach (IGuideGroup guideGroup in _groups)
        {
            guideGroup.OnEnter();
        }
    }
}
#endregion




#region GuideUiSystem



public interface IGuideUiSystem : QFramework.ISystem
{
    Transform Show(string path);
}
public class GuideUiSystem:QFramework.AbstractSystem,  IGuideUiSystem
{

    private Transform _root;
    private Dictionary<string, Transform> _prefabCache;


    protected override void OnInit()
    {
        _prefabCache = new Dictionary<string, Transform>();

        var go = new GameObject(GameObjectName.GuideUiRoot);
        var rect = go.AddComponent<RectTransform>();
        var canvas = go.AddComponent<Canvas>();
        go.AddComponent<GraphicRaycaster>();
        _root = go.transform;

        rect.localPosition = Vector3.zero;
        rect.sizeDelta = _root.parent.GetComponent<RectTransform>().rect.size;
        canvas.overrideSorting = true;
        canvas.sortingOrder = 1;
    }



    public Transform Show(string path)
    {
        Transform trans = null;
        if (_prefabCache.ContainsKey(path))
        {
            if (_prefabCache[path].gameObject.activeSelf)
            {
                trans = CreateNew(path);
                trans.gameObject.AddComponent<DestroyWhenDisable>();
            }
            else
            {
                trans = _prefabCache[path];
            }
        }
        else
        {
            trans = CreateNew(path);
            _prefabCache.Add(path, trans);
        }

        trans.Show();
        return trans;
    }



    #region pri
    private Transform CreateNew(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        return UnityEngine.Object.Instantiate(prefab, _root).transform;
    }
    #endregion



}


#endregion



#region GuideGroupBase


public interface IGuideGroup : IGuide
{
}

public abstract class GuideGroupBase<T> : ICanGetSystem,ICanGetUtility, IGuideGroup where T : IGuide
{
    private Queue<T> _guideItems;
    private Action _complete;
    protected abstract bool IsTrigger { get; }
    protected abstract int GroupId { get; }
    /// <summary>
    /// 数据持久化的键值
    /// </summary>
    protected int _dataId;

    /// <summary>
    /// 是否是第一次执行引导逻辑
    /// </summary>
    private bool _firstExecute;

    public GuideGroupBase(int parentId)
    {
        _dataId = GetDataId(parentId);
        _dataId = parentId < 0 ? parentId : _dataId;
    }

    private int GetDataId(int parentId)
    {
        return parentId << 8 + GroupId;
    }

    public void OnEnter(Action callBack)
    {
        if (this.GetUtility<IGuideStorageUtil>().GetBool(_dataId))
            return;

        _firstExecute = true;
        _complete = callBack;
        _guideItems = GetGuideItems();
    }

    private void SaveData()
    {
        if (_dataId < 0)
            return;

        this.GetUtility<IGuideStorageUtil>().SetInt(_dataId);
    }

    public virtual void Update()
    {
        if (IsTrigger && _firstExecute)
        {
            _firstExecute = false;
            ExecuteGuideItem();
        }
    }

    protected abstract Queue<T> GetGuideItems();

    private bool ExecuteGuideItem()
    {
        if (_guideItems.Count > 0)
        {
            T behaviour = _guideItems.Dequeue();
            behaviour.OnEnter(End);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void End()
    {
        if (!ExecuteGuideItem())
        {
            SaveData();
            //todo:引导结束后要执行的逻辑
            _complete.DoIfNotNull();
        }
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}

/// <summary>
/// 动作组基类
/// </summary>
public abstract class GuideBehaviourGroupBase : GuideGroupBase<IGuideBehaviour>
{
    protected GuideBehaviourGroupBase(int parentId) : base(parentId)
    {
    }
}

public abstract class GuideGroupGroupBase : GuideGroupBase<IGuideGroup>
{
    protected GuideGroupGroupBase(int parentId) : base(parentId)
    {
    }
}
#endregion




#region GuideBehaviourBase


/// <summary>
/// 引导行为接口
/// </summary>
public interface IGuideBehaviour : IGuide
{
}

public abstract class GuideBehaviourBase : IGuideBehaviour
{
    private Action _callBack;
    public virtual void OnEnter(Action callBack)
    {
        _callBack = callBack;
        OnEnterLogic();
    }

    public virtual void Update()
    {

    }

    protected abstract void OnEnterLogic();

    protected virtual void OnExit()
    {
        OnExitLogic();

        _callBack.DoIfNotNull();
    }

    protected abstract void OnExitLogic();
}

#endregion	




