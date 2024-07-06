using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameBehaviour : GuideBehaviourBase,ICanGetSystem   ,ICanGetUtility
{
    private Transform _highLight;
    private Transform _hand;
    private RectTransform _startButtonTrans;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void OnEnterLogic()
    {
        var startView = this.GetSystem<IUISystem>().GetCurrentViewPrefab();
        _startButtonTrans = startView.Find(GameObjectName.Start).GetComponent<RectTransform>();

        _highLight = this.GetUtility<IGuideUtil>().GetHighLightTrans(_startButtonTrans, OnExit);

        _hand = this.GetUtility<IGuideUtil>().GetHandTrans(_startButtonTrans, new Vector2(0.9f, 0.2f));
    }

    protected override void OnExitLogic()
    {
        _highLight.Hide();
        _hand.Hide();
        _startButtonTrans.GetComponent<Button>().onClick.Invoke();
    }
}
