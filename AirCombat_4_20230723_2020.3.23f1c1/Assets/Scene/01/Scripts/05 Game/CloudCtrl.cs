/****************************************************
    文件：MainCameraCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace QFramework.AirCombat
{
    public class CloudCtrl : MonoBehaviour,QFramework.IController  ,IInit
    {

        #region 生命


        public void Init()
        {
             this.Hide();

            
        }


        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }




        #endregion



        #region 实现
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion  

    }
}



