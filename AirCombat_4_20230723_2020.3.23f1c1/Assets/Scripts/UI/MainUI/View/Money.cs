using QFramework;
using QFramework.AirCombat;

public class Money : ViewBase ,ICanGetUtility
{
    protected override void InitChild()
    {
    }

    public override void FrameUpdate()
    {
        
        UiUtil.Get(GameObjectPath.Star_Value).SetText(this.GetUtility<IStorageUtil>().Get<int>(DataKeys.STAR));
        UiUtil.Get(GameObjectPath.Diamond_BG_Text).SetText(this.GetUtility<IStorageUtil>().Get<int>(DataKeys.DIAMOND));
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}