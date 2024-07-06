/****************************************************
    文件：Test_Physics2D.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/22 15:35:23
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
    public class Test_Physics2D : MonoBehaviour
    {
    #region 属性
    Physics2D rgb;
        #endregion

    #region 生命

    /// <summary>首次载入</summary>
        void Awake()
        {
            
        }
        

        /// <summary>Go激活</summary>
        void OnEnable ()
        {
            
        }

        /// <summary>首次载入且Go激活</summary>
        void Start()
        {
             rgb= new Physics2D();
        }

         /// <summary>固定更新</summary>
        void FixedUpdate()
        {
            
        }

        void Update()
        {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            Debug.Log("Q");
            rgb.IgnoreLayerCollision(4, 5,true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W");
            rgb.IgnoreLayerCollision(4, 5,false);  
        }    
        Debug.Log(Physics2D.GetIgnoreLayerCollision(4,5));
        Rigidbody2D rigidbody2D=new Rigidbody2D();
        rigidbody2D.Velocity(Vector3.zero);
        
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

        #endregion 

        #region 辅助

        #endregion

    }



