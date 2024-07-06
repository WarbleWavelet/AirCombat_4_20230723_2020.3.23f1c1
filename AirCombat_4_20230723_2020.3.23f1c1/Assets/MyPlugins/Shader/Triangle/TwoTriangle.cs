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
 

public class TwoTriangle : Graphic
{


    #region 属性
    public Color Color;
    public int VertexIndex;
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
            //添加四个顶点
            vh.AddVert(new Vector3(0, 0), Color.red, Vector2.zero);
            vh.AddVert(new Vector3(0, 100), Color.green, Vector2.zero);
            vh.AddVert(new Vector3(100, 100), Color.black, Vector2.zero);
            vh.AddVert(new Vector3(100, 0), Color.blue, Vector2.zero);
            //添加两个三角形
            vh.AddTriangle(0, 1, 2);
            vh.AddTriangle(2, 3, 0);
           //
            Debug.Log("currentIndexCount " + vh.CurVertCount());//currentVertCount表示VertexHelper结构中有几个顶点
            Debug.Log("currentVertCount " + vh.CurIndexCount()); //currentIndexCount表示VertexHelper结构中有几个顶点索引
        //
            vh.SetColorByIdx(  Color, VertexIndex);
    }     




    #endregion 

    #region 辅助

    #endregion

}




