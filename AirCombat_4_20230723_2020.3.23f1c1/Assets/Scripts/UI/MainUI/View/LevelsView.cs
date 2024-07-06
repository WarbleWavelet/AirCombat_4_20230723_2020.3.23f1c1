[BindPrefabAttribute(ResourcesPath.PREFAB_LEVELS_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class LevelsView : ViewBase
{
    protected override void InitChild()
    {
        UiUtil.Get(GameObjectName.Levels).GameObject.AddComponent<LevelRoot>();
    }
}