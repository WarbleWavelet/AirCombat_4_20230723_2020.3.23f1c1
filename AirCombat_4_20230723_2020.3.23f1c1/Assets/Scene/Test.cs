/****************************************************
    文件：Test.cs
	作者：lenovo
    邮箱: 
    日期：2024/2/15 1:58:58
	功能：
*****************************************************/

using UnityEngine;
using UnityEngine.Rendering;


namespace Demo00_00
{
    public class Test : MonoBehaviour
    {
        #region 属性
           public SpriteRenderer sr;
        public float Degree;
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

            //  ExtendLinearAlgebra.Example_Det();
            //ExtendGreekAlphabet.ExampleGreekAlphabet();
        }

         /// <summary>固定更新</summary>
        void FixedUpdate()
        {
            
        }

        void Update()
        {
            ExtendTransform.Example_EulerAngle(sr.transform);
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
}



