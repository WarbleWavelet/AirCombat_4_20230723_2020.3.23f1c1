/****************************************************
    文件：Test_OnGUI.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/25 17:45:14
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 

    public class Test_OnGUI : MonoBehaviour
    {
    #region 属性
    RectTransform _rect;
        #endregion

        #region 生命

        /// <summary>首次载入</summary>
        void Awake()
        {
            
        }
        

        /// <summary>Go激活</summary>
        void OnEnable ()
        {
           _rect=GetComponent<RectTransform>();     
        }

        /// <summary>首次载入且Go激活</summary>
        void Start()
        {
            
        }

         /// <summary>固定更新</summary>
        void FixedUpdate()
        {
            
        }

        void Update()
        {
            
        }

         /// <summary>延迟更新。适用于跟随逻辑</summary>
        void LateUpdae()
        {
            
        }

        /// <summary> 组件重设为默认值时（只用于编辑状态）</summary>
        void Reset()
        {
            
        }
      

        /// <summary>当对象设置为不可用时</summary>
        void OnDisable()
        {
            
        }


        /// <summary>组件销毁时调用</summary>
        void OnDestroy()
        {
            
        }
    #endregion

    #region 系统
    private void OnGUI()
    {
        GetComponent<RectTransform>().DrawText();
       // GUI.Label(new RectTransform(Screen.width/2f, Screen._height/2f, 150, 80), "RectTransform : " +_rect.rect);
    }
    #endregion

    #region 辅助

    #endregion

}




