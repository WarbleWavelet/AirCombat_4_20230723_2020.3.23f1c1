/****************************************************
    文件：Enums.BulletEnemyCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/7/23 23:11:22
	功能：这里的都是资源名称
*****************************************************/


/// <summary>关联到人物语音的枚举。维持大小写</summary>
public enum Hero //:IJson
{
    Player_0,
    Player_1,
    Player_2
}


public class AudioBG
{
    const string _pre = "Audio/BGM/";
    public const string Game_BGM = _pre + "Game_BGM";
    public const string Battle_BGM = _pre + "Battle_BGM";
    public const string Boss_BGM = _pre + "Boss_BGM";
}

public class AudioUI
{
    const string _pre = "Audio/UI/";
    public const string UI_ClickButton = _pre + "UI_ClickButton";
    public const string UI_Loading = _pre + "UI_Loading";
    public const string UI_StartGame = _pre + "UI_StartGame";
}

public class AudioGame
{

    const string _pre = "Audio/";
    //
    /// <summary>打印容易认</summary>
    public  const string GameAudioNull = "GameAudioNull";
    public  const string Audio_Fire = _pre + "Buttle/Fire";
    public  const string Audio_Power= _pre + "Buttle/Power";
    //
    private const string _explode = _pre + "Explode/";
    private const string _effect = _pre + "Effect/";
    private const string _item = _pre + "Item/";
    public const string Effect_Great =           _effect + "Effect_Great";
    public const string Effect_GameOver =        _effect + "Effect_GameOver";
    public const string Effect_Boss_Warning =    _effect + "Effect_Boss_Warning";
    public const string Effect_Missile_Warning = _effect + "Effect_Missile_Warning";
    public const string Explode_Bullet =         _explode +"Explode_Bullet";
    public const string Explode_Plane =          _explode +"Explode_Plane";
    public const string Get_Gold      =          _item + "Get_Gold";
    public const string Get_Item      =          _item + "Get_Item";
    public const string Get_Shield    =          _item + "Get_Shield";
    public const string Lost_Item     =          _item + "Lost_Item";
}