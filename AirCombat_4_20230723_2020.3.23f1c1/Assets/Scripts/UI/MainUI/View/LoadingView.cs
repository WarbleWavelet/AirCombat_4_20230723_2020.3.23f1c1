using QFramework;
using QFramework.AirCombat;
using UnityEngine;
using UnityEngine.UI;

[BindPrefabAttribute(ResourcesPath.PREFAB_LOADING_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class LoadingView : ViewBase,ICanGetSystem
{
    private Slider _slider;
    private bool _showEnd;

    protected override void InitChild()
    {
        _slider = UiUtil.Get(GameObjectName.Slider).Get<Slider>();
    }

    public override void Show()
    {
        base.Show();
        this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
        _showEnd = true;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if(!_showEnd)
            return;
        
        UpdateProgress();
        UpdateSlider();
    }

    public override void Hide()
    {
        base.Hide();
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
        _showEnd = false;
    }

    private void UpdateProgress()
    {
        var progress = this.GetSystem<ISceneSystem>().Process();
        progress *= 100;
        transform.GetComponentDeep<Text>(GameObjectName.Progress).SetText(string.Format("{0}%", progress));
    }

    private void UpdateSlider()
    {
        _slider.value = this.GetSystem<ISceneSystem>().Process();
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}