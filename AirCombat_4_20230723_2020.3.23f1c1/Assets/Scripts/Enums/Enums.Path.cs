/****************************************************
    文件：Enums.BulletEnemyCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/23 23:11:22
	功能：
*****************************************************/


/// <summary>路径类型，有关Json，不要乱改大小写</summary>
public enum TrajectoryType 
{
    /// <summary> 直线路径 </summary>
    STRAIGHT,

    /// <summary> W型路径 </summary>
    W,

    /// <summary>  只有入场动画，入场后呆在上方不动 </summary>
    STAY_ON_TOP,

    /// <summary> 椭圆路径 </summary>
    ELLIPSE,
    /// <summary>原地旋转</summary>
    ROTATE,
    //V,轨迹Data中有个V
    COUNT
}


public enum PathState
{
    NULL,
    ENTER,
    FORWARD_MOVING,
    BACK_MOVING
}






