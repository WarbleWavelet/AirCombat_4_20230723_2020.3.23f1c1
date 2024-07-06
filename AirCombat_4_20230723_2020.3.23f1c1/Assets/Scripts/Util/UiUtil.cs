using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary> 不清楚能否深度查找</summary>
public class UiUtil : MonoBehaviour ,IInit
{
    private Dictionary<string, UiUtilData> _uiUtilDataDic;

    public void Init()
    {
        _uiUtilDataDic = new Dictionary<string, UiUtilData>();
        var rect = transform.GetComponent<RectTransform>();
        foreach (RectTransform child in rect)
        { 
           _uiUtilDataDic.Add(child.name, new UiUtilData(child));
        } 
    }

    public UiUtilData Get(string name)
    {
        if (_uiUtilDataDic.ContainsKey(name))
        { 
            return _uiUtilDataDic[name];
        }

        var t = transform.Find(name);
        if (t.IsNullObject() ||  t.IsNull())
        {
            Debug.LogError("无法按照路径查找到物体，路径为：" + name);
            return null;
        }

        _uiUtilDataDic.Add(name, new UiUtilData(t.GetComponent<RectTransform>()));
        return _uiUtilDataDic[name];
    }
}


#region UiUtilData


/// <summary>
/// GameObject
/// <br/>RectTransform
/// <br/>Image
/// <br/>Text
/// </summary>
public class UiUtilData:IRectTransform,IGameObject,IImage,IText
{


    #region 字属构造
    public GameObject GameObject { get; }
    public RectTransform RectTransform { get; }
    public Image Image { get; }
    public Text Text { get; }
    public UiUtilData(RectTransform rect)
    {
        RectTransform = rect;
        GameObject = rect.gameObject;
        Image = rect.GetComponent<Image>();
        Text = rect.GetComponent<Text>();
    }
    #endregion


    #region pub
    public void SetSprite(Sprite sprite)
    {
        if (Image != null )
            Image.sprite = sprite;
        else
            Debug.LogError("当前物体上没有image组件，物体名称为" + GameObject.name);
    }

    public void SetText(int content)
    {
        SetText(content.ToString());
    }


    public void SetText(string content)
    {
        if (Text != null)
            Text.text = content;
        else
            Debug.LogError("当前物体上没有Text组件，物体名称为" + GameObject.name);
    }

    public T Get<T>() where T : Component
    {
        if (GameObject.IsNotNull())
        { 
            return GameObject.GetComponent<T>();
        }

        Debug.LogError("当前gameobject为空"+GameObject.name);
        return null;
    }

    public T Add<T>() where T : Component
    {
        if (GameObject.IsNotNull())
        {
            return GameObject.AddComponent<T>();
        }

        Debug.LogError("当前gameobject为空" + GameObject.name);
        return null;
    }
    #endregion


}
#endregion  

