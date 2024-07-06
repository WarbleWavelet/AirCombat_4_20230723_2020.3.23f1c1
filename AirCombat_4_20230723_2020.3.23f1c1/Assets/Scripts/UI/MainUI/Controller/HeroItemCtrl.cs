using QFramework.AirCombat;
using QFramework;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HeroItemCtrl : ControllerBase, QFramework.IController
{
    private Hero _hero;



    #region 生命


    protected override void InitChild()
    {
        var heroName = GetComponent<Image>().sprite.name;

        try
        {
            string spriteName = transform.GetComponent<Image>().sprite.name;
            _hero = (spriteName.UpperFirstLetter()).String2Enum<Hero>();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        GetComponent<Button>().onClick.AddListenerAfterRemoveAll(Selected);
    }



    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (_hero !=   this.GetModel<IAirCombatAppStateModel>().SelectedHero)
        { 
            this.GetSystem<IAudioSystem>().Stop(_hero.ToString());
        }
    }

    public override void Hide()
    {
        base.Hide();
        this.GetSystem<IAudioSystem>().Stop(_hero.ToString());
    }
    #endregion



    #region 辅助
    private void Selected()
    {
       this.GetModel<IAirCombatAppStateModel>().SelectedHero.Value = _hero;

        string heroSound = _hero.ToString();
        this.GetSystem<IAudioSystem>().PlayVoice(heroSound);
        Debug.Log("HeroItemController "+ heroSound);
    }
    #endregion


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}