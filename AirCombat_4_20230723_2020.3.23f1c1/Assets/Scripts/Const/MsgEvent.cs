using System;

public class MsgEvent
{
    public const int EVENT_HP = 0;
    public const int EVENT_SCORE = 1;
    public const int EVENT_SHIELD = 2;
    public const int EVENT_POWER = 3;
    //
    public const int EVENT_SHIELD_USE = 4   ;
    public const int EVENT_USE_BOMB = 5;
    public const int EVENT_CHANGE_HAND = 6;
    //
    ////如果Register需要根据值的不同而分发时
    public const int EVENT_GAME_START = 7;
    public const int EVENT_GAME_PAUSE = 8;
    public const int EVENT_GAME_CONTINUE = 9;
    /// <summary>玩家死亡会触发</summary>
    public const int EVENT_GAME_END = 10;
    /// <summary> 开始新的一关 </summary>
    public const int EVENT_LEVEL_START = 11;
    /// <summary> Boss死了,结束通过了一关 </summary>
    public const int EVENT_LEVEL_END = 12;

    //
    /// <summary> 在游戏过程中更新升级 </summary>
    public const int EVENT_GAME_UPDATE_LEVEL = 13;
    /// <summary> 通过加经验的方式提升了临时等级  </summary>
    public const int EVENT_GAME_EXP_LEVEL_UP = 14;
    public const int EVENT_BUFF = 15;
    public const int EVENT_DEBUFF = 16;
    //



    //
    /// <summary>开始射击 控制敌人射击,防止在边界外就射击,没有视觉感</summary>
    public const int EVENT_CANSHOOT = 17;
}