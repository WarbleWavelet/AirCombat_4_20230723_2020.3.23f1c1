using QFramework;
using System.Collections.Generic;
using UnityEngine;

public interface IUILevelSystem : QFramework.ISystem
{


     void SetParent(string path, Transform trans);

     UILevel GetUILevel(string path);

}
public class UILevelSystem : QFramework.AbstractSystem, IUILevelSystem
{
    private Dictionary<UILevel, RectTransform> _uiParentDic;



    #region 生命

    protected override void OnInit()
    {
        Transform parent = Camera.main.transform.FindTop(GameObjectName.UIRoot);
        if (parent != null)
        { 
            InitFunc(parent);
        }
    }

    #endregion




    #region pub


    public void SetParent(string path, Transform trans)
    {
        if (_uiParentDic == null)
        { 
              Debug.LogError("****UILayerMgr未初始化");
        }
      
        var parent = _uiParentDic[GetUILevel(path)];

        trans.SetParent(parent);
    }

    public UILevel GetUILevel(string path)
    {
        if (UILevelConfigSingleton.LevelDic.ContainsKey(path))
        {
            return UILevelConfigSingleton.LevelDic[path];
        }

        Debug.LogWarning("当前预制未初始化层级配置，预制路径：" + path);
        return UILevel.Common;
    }
    #endregion


    #region pri
    void InitFunc(Transform parent)
    {
        _uiParentDic = new Dictionary<UILevel, RectTransform>();
        RectTransform rect;
        for (UILevel i = UILevel.AlwayBottom; i < UILevel.COUNT; i++)//有的过时跳过了
        {
            if (i.IsObsolete())
                continue;
            if (parent.Find(i.ToString()) == null)
            {
                var child = new GameObject(i.ToString());
                child.transform.SetParent(parent);
                rect = child.AddComponent<RectTransform>();
                rect.Stretch();
            }
            else
            { 
                rect= parent.Find(i.ToString()).Rect();
            }

            _uiParentDic[i] = rect;
        }
    }



    #endregion
}