using QFramework.AirCombat;
using QFramework;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


/// <summary>弹出对话框</summary>
[BindPrefabAttribute(ResourcesPath.PREFAB_DIALOG_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class DialogPanel : MonoBehaviour,IInitParas<DialogPanel>, ICanGetUtility ,ICanGetSystem
{


    #region 字属



    private readonly float _leftAndRight = 60;
    private readonly float _maxWidth = 550;
    private readonly float _minWidth = 330;
    private readonly float _offset = 40;
    private readonly float _upAndDown = 40;

    private UiUtil _util;

    /// <summary>一个按钮的父节点</summary>
    private readonly string _onePath = "One";
    /// <summary>两个按钮的父节点</summary>
    private readonly string _twoPath = "Two";
    private readonly string _noBtn = "No";
    private readonly string _yesBtn = "Yes";
    private readonly string _content = "Content";
    #endregion




    #region pub

    public DialogPanel InitParas(params object[] os)
    {
        string content = os[0].ToString();
        Action trueAction = os[1].ReturnIfNotNull();
        Action falseAcion = os[2].ReturnIfNotNull();
        if (_util == null)
        {
            _util = gameObject.AddComponent<UiUtil>();
            _util.Init();
        }

        UpdateContent(content);
        AddAction(trueAction, falseAcion);
        this.GetSystem<ICoroutineSystem>().StartOutter(ChangeSize());

        return this;
    }

    #endregion

    #region pri


    private void AddAction(Action trueAction, Action falseAcion)
    {
        if (trueAction == null && falseAcion == null)//单选
        {
            SetButtonState(true);
            AddOneListener(trueAction);
        }
        else if (trueAction == null && falseAcion != null)
        {
            Debug.LogError("在取消事件不为空时，确认事件不能为空");
            AddOneListener(null);
        }
        else if (trueAction != null && falseAcion == null)  //单选
        {
            SetButtonState(true);
            AddOneListener(trueAction);
        }
        else  //双选
        {
            SetButtonState(false);
            AddTwoListener(trueAction, falseAcion);
        }
    }

    private void SetButtonState(bool isOne)
    {
        gameObject.FindChildDeep(_onePath).SetActive(isOne);
        gameObject.FindChildDeep(_twoPath).SetActive(!isOne);
    }

    private void AddOneListener(Action trueAction)
    {
        if (trueAction == null)
            transform.FindChildDeep(_onePath).GetButtonDeep(_yesBtn, () => this.GetSystem<IUISystem>().HidePopPanel());
        else
            transform.FindChildDeep(_onePath).GetButtonDeep(_yesBtn, () => trueAction());
    }

    private void AddTwoListener(Action trueAction, Action falseAcion)
    {
        transform.FindChildDeep(_twoPath).GetButtonDeep(_yesBtn, () => trueAction());
        transform.FindChildDeep(_twoPath).GetButtonDeep(_noBtn,  () => falseAcion());
    }


    private IEnumerator ChangeSize()              
    {
        yield return null;
        var content =transform.GetComponentDeep<RectTransform>(GameObjectName.Content).Rect();
        var buttons = transform.GetComponentDeep<RectTransform>(GameObjectName.Buttons).Rect();
        var frame = transform.GetComponentDeep<RectTransform>(GameObjectName.Frame).Rect();


       // SetWidth(content, frame);
        yield return null;
       // SetHeight(content, buttons, frame);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="content">标题显示内容文本</param>
    /// <param name="frame">背景</param>
    private void SetWidth(RectTransform content, RectTransform frame)
    {
        var weight = _leftAndRight * 2 + content.rect.width;

        float result = 0;
        if (weight <= _minWidth)
        {
            result = _minWidth;
        }
        else if (weight > _maxWidth)
        {
            result = _maxWidth + _leftAndRight * 2;
            content.GetComponent<LayoutElement>().SetPreferredWidth(_maxWidth)  ;
        }
        else
        {
            result = weight;
        }

        frame.sizeDelta = new Vector2(result, frame.SizeDeltaY());
    }

    private void SetHeight(RectTransform content, RectTransform buttons, RectTransform frame)
    {
        SetContentY(content);
        SetButtonsY(content, buttons);
        SetFrameHeight(content, buttons, frame);
    }

    private void SetContentY(RectTransform content)
    {
        var y = content.rect.height * 0.5f + _upAndDown;
        SetPosY(content, y);
    }

    private void SetButtonsY(RectTransform content, RectTransform buttons)
    {
        var offset = _upAndDown + content.rect.height + _offset;

        var y = offset + buttons.rect.height * 0.5f;

        SetPosY(buttons, y);
    }

    private void SetFrameHeight(RectTransform content, RectTransform buttons, RectTransform frame)
    {
        var height = _upAndDown * 2 + _offset + content.rect.height + buttons.rect.height;
        var size = frame.sizeDelta;
        size.y = height;
        frame.sizeDelta = size;
    }

    private void SetPosY(RectTransform rect, float y)
    {
        var pos = rect.anchoredPosition;
        pos.y = -y;
        rect.anchoredPosition = pos;
    }

    private void UpdateContent(string content)
    {
        transform.GetComponentDeep<Text>(_content).text = content;
    }


    #endregion


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}