using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneDestroyAniView : EffectLevelView  ,QFramework.IController
{
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void InitComponent()
    {
        var ani = gameObject.GetOrAddComponent<FrameAniComponent>();
        var sprites = this.GetUtility<ILoadUtil>().LoadAll<Sprite>(ResourcesPath.PICTURE_PLANE_DESTROY_FOLDER);
        ani.Init(sprites,AniEnd);
    }

    private void AniEnd()
    {
        this.GetSystem<IGameObjectPoolSystem>().DespawnWhileKeyIsName(gameObject);
    }
}
