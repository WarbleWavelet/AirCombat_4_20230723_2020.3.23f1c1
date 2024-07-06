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
using SpeedDes = ISpeed.SpeedDes;

namespace QFramework.AirCombat
{
    public class MainCameraCtrl : MonoBehaviour  ,IController  ,IInit
    {
        #region 属性
        CameraMoveSelfComponent _moveCpt;

        #endregion

        #region 生命
       public void Init()
        {
          float cameraSpeed=  this.GetModel<IAirCombatAppStateModel>().CameraSpeed;
                _moveCpt = gameObject.GetOrAddComponent<CameraMoveSelfComponent>().InitComponent(cameraSpeed);
                this.SendCommand(new InitCameraSpeedCommand());
        }
        #endregion
                                                       

        #region 实现
        public  IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }


        #endregion

    }
}



