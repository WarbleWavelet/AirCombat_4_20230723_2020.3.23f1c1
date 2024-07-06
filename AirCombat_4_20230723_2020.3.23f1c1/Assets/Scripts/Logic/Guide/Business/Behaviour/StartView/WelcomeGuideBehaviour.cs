using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeGuideBehaviour : GuideBehaviourBase ,ICanGetSystem
{
    private Transform _view;

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    protected override void OnEnterLogic()
    {
        _view = this.GetSystem<IGuideUiSystem>().Show(ResourcesPath.PREFAB_WELCOME_GUIDE);
        var button = _view.GetOrAddComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnExit);
    }

    protected override void OnExitLogic()
    {
        _view.Hide();
    }
}
