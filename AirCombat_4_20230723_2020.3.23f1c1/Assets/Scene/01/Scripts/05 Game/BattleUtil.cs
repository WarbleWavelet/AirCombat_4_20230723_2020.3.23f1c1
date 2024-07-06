/****************************************************
    文件：BattleUtil.cs
	作者：lenovo
    邮箱: 
    日期：2023/12/17 15:32:18
	功能：
*****************************************************/

using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


/// <summary>在战斗中用到的Util</summary>
public interface IBattleUtil : QFramework.IUtility
{
    public void ChangeHandPos(RectTransform rect, HandMode handMode)  ;
}
public class BattleUtil : IBattleUtil, ICanGetModel
{


    #region pub
    public void ChangeHandPos(RectTransform rect,HandMode handMode)
    {
        var value = Mathf.Abs(rect.anchoredPosition.x);
        if (handMode == HandMode.LEFT)
            SetPosData(rect, Vector2.zero, value);
        else
            SetPosData(rect, Vector2.right, -value);
    }
    #endregion




    #region pri
    private static void SetPosData(RectTransform rect, Vector2 anchorValue, float x)
    {
        rect.anchorMin = anchorValue;
        rect.anchorMax = anchorValue;
        rect.SetAnchoredPosX(x);
    }
    #endregion




    #region 实现
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion  

}



