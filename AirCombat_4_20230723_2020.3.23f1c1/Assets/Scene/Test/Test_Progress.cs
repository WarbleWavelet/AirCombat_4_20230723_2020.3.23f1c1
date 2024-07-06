/****************************************************
    文件：Test_Progress.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/14 22:31:59
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
 

public class Test_Progress : MonoBehaviour
{
    #region 属性
    public Image Image;
    public Slider Slider;
    #endregion


    #region 系统
    private void Start()
    {
        ExtendProgressBar.Example(this,Image, Slider);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ExtendProgressBar.Example(this, Image, Slider);
        }
    }
    #endregion

}




