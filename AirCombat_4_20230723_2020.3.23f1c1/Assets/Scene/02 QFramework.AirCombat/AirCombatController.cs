/****************************************************
    文件：AirCombatController.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/8 15:49:58
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework.Example;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace QFramework.AirCombat
{
    #region Controller
    // Controller
    public class AirCombatAppController : MonoBehaviour, IController /* 3.实现 IController 接口 */
    {
        private Button mBtnAdd;
        private Button mBtnSub;
        private Text mCountText;
        //
        private IAirCombatAppModel mModel;


        #region 生命

        public void Init()
        {
            // 5. 获取模型
            mModel = this.GetModel<IAirCombatAppModel>();



            // 监听输入
            mBtnAdd.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand<IncreaseCountCommand>();
            });

            mBtnSub.onClick.AddListener(() =>
            {
                // 交互逻辑
                this.SendCommand(new DecreaseCountCommand(/* 这里可以传参（如果有） */));
            });

            // 表现逻辑
            mModel.Star.RegisterWithInitValue(newCount => // -+
            {
                UpdateView();

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }
        private void OnDestroy()
        {
            // 8. 将 Model 设置为空
            mModel = null;
        }
        #endregion



        #region 辅助

        void UpdateView()
        {
            mCountText.text = mModel.Star.ToString();
        }
        #endregion


        #region 实现
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion



    }
    #endregion
}



