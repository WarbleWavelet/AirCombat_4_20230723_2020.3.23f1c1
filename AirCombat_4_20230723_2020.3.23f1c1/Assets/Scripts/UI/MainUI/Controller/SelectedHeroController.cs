using QFramework.AirCombat;
using QFramework;
using UnityEngine;

[BindPrefabAttribute(ResourcesPath.PREFAB_SELECTED_HERO_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class SelectedHeroController : ControllerBase, ICanGetUtility ,ICanGetSystem
{
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    protected override void InitChild()
    {
        transform.Find(GameObjectName.Heros).AddComponent<SelectHeroController>();

        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectPath.OK_Value, () => { this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_LEVELS_VIEW); });

        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Exit, () => { Application.Quit(); });

        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Strengthen, () => { this.GetSystem<IUISystem>().Open(ResourcesPath.PREFAB_STRENGTHEN_VIEW); });
    }
}