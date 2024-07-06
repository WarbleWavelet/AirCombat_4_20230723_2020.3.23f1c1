using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHerosBehaviour : GuideBehaviourBase  ,ICanGetSystem ,ICanGetUtility
{
    
    private Transform _highLight;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void OnEnterLogic()
    {
        var go = this.GetSystem<IUISystem>().GetCurrentViewPrefab();
        Transform parent = go.Find(GameObjectName.Heros);
        RectTransform[] children = parent.GetComponentsInChildren<RectTransform>();
        _highLight = this.GetUtility<IGuideUtil>().GetHighLightTrans(children, OnExit);
    }

    protected override void OnExitLogic()
    {
        _highLight.Hide();
    }
}
