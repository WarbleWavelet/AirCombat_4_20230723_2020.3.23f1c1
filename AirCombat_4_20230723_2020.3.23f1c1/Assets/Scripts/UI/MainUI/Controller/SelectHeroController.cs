using UnityEngine;

public class SelectHeroController : ControllerBase
{
    protected override void InitChild()
    {
        foreach (Transform trans in transform)
        { 
            trans.GetOrAddComponent<HeroItemCtrl>();
        }
    }
}