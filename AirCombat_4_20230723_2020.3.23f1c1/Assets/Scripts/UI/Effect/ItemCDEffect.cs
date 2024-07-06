using QFramework;
using QFramework.AirCombat;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemCDEffect : MonoBehaviour, IUpdate ,ICanGetSystem
{

    #region 字属

    /// <summary>回调callBack</summary>
    private Action _cb;
    private Image _image;
    public int Framing { get; set; }

    public int Frame { get; }
    private Image Image
    {
        get
        {
            if (_image == null)
            { 
                _image = GetComponent<Image>();
            
            }

            return _image;
        }
    }
    #endregion






    #region 辅助
    public void FrameUpdate()
    {
        Image.fillAmount -= UnityEngine.Time.deltaTime / Const.CD_EFFECT_TIME;
        if (Image.fillAmount <= 0)
        {
            this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
            _cb.DoIfNotNull();
        }
    }

    /// <summary>可释放</summary>
    public void SetShow()
    {
        Image.fillAmount = 0;
    }

    /// <summary>不可释放</summary>
    public void SetMask()
    {
        Image.fillAmount = 1;
    }


    /// <summary>开始计时</summary>
    public void StartCD(Action cb)
    {
        _cb = cb;
        Image.fillAmount = 1;
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}