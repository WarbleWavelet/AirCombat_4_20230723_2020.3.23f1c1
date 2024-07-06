using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroyAniView : EffectLevelView ,ISpritesPath ,QFramework.IController
{
    public string SpritesPath { get { return ResourcesPath.PICTURE_PLANE_DESTROY_FOLDER; } }



    protected override void InitComponent()
    {
        var ani = gameObject.GetOrAddComponent<FrameAniComponent>();
        var sprites = this.GetUtility<ILoadUtil>().LoadAll<Sprite>(SpritesPath);
        sprites = GetSprites(sprites);
        ani.Init(sprites, OnAniEnd);
    }


    #region pri
    private Sprite[] GetSprites(Sprite[] sprites)
    {
        Sprite[] temp = new Sprite[sprites.Length/4];
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i % 4 == 0)
            {
                temp[i / 4] = sprites[i];
            }
        }
        
        return temp;
    }

    private void OnAniEnd()
    {
        this.GetSystem<IGameObjectPoolSystem>().DespawnWhileKeyIsName(gameObject);
    }
    #endregion



    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}
