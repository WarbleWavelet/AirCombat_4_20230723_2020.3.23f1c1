/****************************************************
    文件：ExtendShader.cs
	作者：lenovo
    邮箱: 
    日期：2024/5/24 19:58:37
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static partial class ExtendShader 
{


    #region 枚举
    /// <summary>着色器单位</summary>
    public enum EShaderUnit
    { 
        顶点,
        片元,
        色块,

    }
    /// <summary>着色器语言</summary>
    public enum EShaderLanguage
    {
        /// <summary>实时着色器语言</summary>
        RealTimeSL,
        /// <summary>离线着色器语言</summary>
        OutLineSL,
        //
        /// <summary>老大哥</summary>
        OpenGL, GLSL,
        /// <summary>平台多</summary>
        CG, CForGraphic,
        /// <summary>平台多余OpenGL,效果好</summary>
        DirectX_HighLevel, HLSL,
        //
        /// <summary>汇编</summary>
        DirectX_Assembler,

        //

    }

    public enum EUnityShaderLanguage
    {
        /// <summary>
        /// Unity本地化(可以方便地引用unity的东西) ,容器(还可以杂糅支持的语言).
        /// 支持的语言(比如CG)都可以,加标志区别
        /// </summary>
        ShaderLab,
        /// <summary>老版本</summary>
        CG,
        HLSL,

    }

    /// <summary>管线</summary>
    public enum ERenderPipeline
    {
        /// <summary>内置</summary>
        BRP,
        /// <summary>可编程</summary>
        SRPs,
        /// <summary>自定义</summary>
        CRP,
        /// <summary>通用</summary>
        URP,
        /// <summary>HighDefinition</summary>
        HDRP,
    }
    #endregion


    public interface IBRP2URP {}
    public interface IBRP2HDRP{ }
    public interface IURP2HDRP{ }
    public interface IHDRP2URP{ }
}
public static partial class ExtendShader { }
public static partial class ExtendShader { }
public static partial class ExtendShader_Noise 
{
    public enum NoiseFunction
    {
        柏林噪声,PerlinNoise
    }

}




