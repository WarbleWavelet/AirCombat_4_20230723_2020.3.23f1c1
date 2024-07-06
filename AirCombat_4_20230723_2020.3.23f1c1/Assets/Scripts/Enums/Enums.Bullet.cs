/****************************************************
    文件：Enums.BulletEnemyCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/23 23:11:22
	功能：
*****************************************************/


public enum BulletOwner
{
    PLAYER,
    ENEMY
}


public enum BulletType
{
    PLAYER,
    ENEMY_NORMAL_0,
    ENEMY_BOSS_0,
    ENEMY_BOSS_1,
    POWER,
    COUNT,
    NULL
}


public enum BulletName
{
    ENEMY_NORMAL_0,
    ENEMY_BOSS_0,
    ENEMY_BOSS_1,
    COUNT
}


public enum BulletEventType
{
    /// <summary>变速</summary>         
    CHANGE_SPEED,
    /// <summary>弹道，轨迹</summary>
    CHANGE_TRAJECTORY
}






