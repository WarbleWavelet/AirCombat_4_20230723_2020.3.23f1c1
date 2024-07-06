/****************************************************
    文件：Quad.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/27 21:4:36
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Quad : Graphic
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        UIVertex[] verts = new UIVertex[4];
        verts[0].position = new Vector3(0, 0);
        verts[0].color = Color.red;
        verts[0].uv0 = Vector2.zero;

        verts[1].position = new Vector3(0, 100);
        verts[1].color = Color.green;
        verts[1].uv0 = Vector2.zero;

        verts[2].position = new Vector3(200, 100);
        verts[2].color = Color.black;
        verts[2].uv0 = Vector2.zero;

        verts[3].position = new Vector3(200, 0);
        verts[3].color = Color.blue;
        verts[3].uv0 = Vector2.zero;

        vh.AddUIVertexQuad(verts);
    }
}




