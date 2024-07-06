/****************************************************
    文件：Triangle.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/27 20:23:26
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
 

public class Triangle : Graphic
{


    #region 属性

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
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        vh.AddVert(new Vector3(0, 0), Color.red, Vector2.zero);
        vh.AddVert(new Vector3(0, 100), Color.green, Vector2.zero);
        vh.AddVert(new Vector3(100, 100), Color.black, Vector2.zero);

        vh.AddTriangle(0, 1, 2);

    }
    #endregion 

    #region 辅助

    #endregion

}




