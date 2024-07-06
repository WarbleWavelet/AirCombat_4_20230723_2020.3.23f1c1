using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IAniSystem : ISystem
{
    public void OnPlaneDestroyAni(PlaneDestroyAniEvent e);
    public void OnBulletDestroyAni(BulletDestroyAniEvent e);
}
public class AniSystem : AbstractSystem, IAniSystem
{
    protected override void OnInit()
    {
        this.RegisterEvent<PlaneDestroyAniEvent>(OnPlaneDestroyAni);
        this.RegisterEvent<BulletDestroyAniEvent>(OnBulletDestroyAni);
    }

    public void OnPlaneDestroyAni(PlaneDestroyAniEvent e)
    {
        Vector3 pos = e.pos;
        var go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.EFFECT_FRAME_ANI);
        var view = go.GetOrAddComponent<PlaneDestroyAniView>();
        view.Init();
        view.SetScale(Vector3.one * 1.0f);
        view.SetPos(pos);
    }

    public void OnBulletDestroyAni(BulletDestroyAniEvent e)
    {
        Vector3 pos = e.pos;
        var go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.EFFECT_FRAME_ANI);
        var view = go.GetOrAddComponent<BulletDestroyAniView>();
        view.Init();
        view.SetScale(Vector3.one * 0.002f);
        view.SetPos(pos);
    }
}
