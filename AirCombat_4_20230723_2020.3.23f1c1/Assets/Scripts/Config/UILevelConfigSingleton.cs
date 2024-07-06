using QFramework;
using System.Collections.Generic;


public class UILevelConfigSingleton : NormalSingleton<UILevelConfigSingleton>
{
    public static readonly Dictionary<string, UILevel> LevelDic = new Dictionary<string, UILevel>
    {
        {ResourcesPath.PREFAB_START_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_SELECTED_HERO_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_STRENGTHEN_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_LOADING_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_LOADING_PANEL, UILevel.Common},
        {ResourcesPath.PREFAB_GAME_UI_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_LEVELS_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_DIALOG_PANEL, UILevel.PopUI},
        //
        {ResourcesPath.PREFAB_SETTING_VIEW, UILevel.Common},
        {ResourcesPath.PREFAB_GAME_RESULT_VIEW,UILevel.PopUI} ,   
        {ResourcesPath.PREFAB_SELECTED_HERO_PANEL,UILevel.Common}    
    };
}