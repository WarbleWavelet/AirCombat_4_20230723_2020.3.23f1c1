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
    public class MapCtrl : MonoBehaviour,QFramework.IController  ,IInit
    {

        #region 生命


        public void Init()
        {
     
                Transform t = GameObject.Find(GameObjectName.MapCtrl).transform;
               //$"!!!mapMgrTran={t}".LogInfo();
                Transform map0 = t.FindOrNew(GameObjectName.map_0);
                Transform map1 = t.FindOrNew(GameObjectName.map_1);
                var offsetY = Mathf.Abs(map1.position.y - map0.position.y);
                Transform camera = Camera.main.transform;
                map0.GetOrAddComponent<MapItemCtrl>().Init(offsetY, camera);
                map1.GetOrAddComponent<MapItemCtrl>().Init(offsetY, camera);
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



