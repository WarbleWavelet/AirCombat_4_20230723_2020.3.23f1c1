using QFramework;
using QFramework.AirCombat;

[BindPrefabAttribute(ResourcesPath.PREFAB_START_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class StartController : ControllerBase ,ICanGetSystem ,ICanGetUtility
{
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Start , () =>
        {
            this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_SELECTED_HERO_VIEW);
            this.GetSystem<IAudioSystem>().PlaySound(AudioUI.UI_StartGame);
        }, false);

        this.GetSystem<IAudioSystem>().PlayMusic(AudioBG.Game_BGM);

      
    }
}