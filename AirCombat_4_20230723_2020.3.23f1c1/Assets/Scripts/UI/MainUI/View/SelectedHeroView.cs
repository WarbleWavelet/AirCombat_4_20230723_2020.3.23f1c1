[BindPrefabAttribute(ResourcesPath.PREFAB_SELECTED_HERO_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class SelectedHeroView : ViewBase
{
    protected override void InitChild()
    {
        UiUtil.Get(GameObjectName.Heros).GameObject.AddComponent<SelectHero>();
    }
}