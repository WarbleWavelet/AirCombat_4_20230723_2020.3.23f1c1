using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class SwitchPlayerView : ViewBase,ICanSendCommand
{
    protected override void InitChild()
    {
    }


    public override void Show()
    {
        base.Show();
        UpdateSprite();
    }

    public override void FrameUpdate()
    {
        UpdateSprite();
    }


    private void UpdateSprite()
    {
        Sprite planeSprite = this.SendCommand(new GetPlaneSpriteCommand());
        UiUtil.Get(GameObjectName.Icon).SetSprite(planeSprite);
    }

    public IArchitecture GetArchitecture()
    {
       return AirCombatApp.Interface;
    }
}