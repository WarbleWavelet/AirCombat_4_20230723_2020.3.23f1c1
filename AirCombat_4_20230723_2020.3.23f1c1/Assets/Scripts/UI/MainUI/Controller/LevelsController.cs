using QFramework;
using QFramework.AirCombat;

[BindPrefabAttribute(ResourcesPath.PREFAB_LEVELS_VIEW, Const.BIND_PREFAB_PRIORITY_CONTROLLER)]
public class LevelsController : ControllerBase ,ICanGetUtility ,ICanGetSystem
{
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void InitChild()
    {
        this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(transform,GameObjectName.Back, this.GetSystem<IUISystem>().Back);
        transform.Find(GameObjectName.Levels).gameObject.AddComponent<LevelRootController>();
    }
}