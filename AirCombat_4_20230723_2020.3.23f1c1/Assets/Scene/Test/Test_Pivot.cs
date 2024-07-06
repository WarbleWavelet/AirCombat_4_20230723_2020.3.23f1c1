/****************************************************
    文件：Test_Enum.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/24 17:58:32
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 

public class Test_Pivot : MonoBehaviour
{
    RectTransform _rect;
    public EDir Dir;
    private void Start()
    {
        _rect= GetComponent<RectTransform>();
      
    }
    private void Update()
    {
        _rect.SetPivot(Dir);   
    }

}



