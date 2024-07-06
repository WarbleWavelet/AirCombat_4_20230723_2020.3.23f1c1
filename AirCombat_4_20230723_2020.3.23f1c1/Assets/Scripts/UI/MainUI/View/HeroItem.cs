using System;
using DG.Tweening;
using QFramework.AirCombat;
using QFramework;
using UnityEngine;
using UnityEngine.UI;

public class HeroItem : ViewBase, QFramework.IController

{
    private readonly Color _defaultColor = Color.gray;
    private readonly Color _selectedColor = Color.white;

    private Hero _hero;
    private Image _image;
    private readonly float _time = 0.5f;


    #region 重写
    protected override void InitChild()
    {

        _image = transform.GetComponent<Image>();

        try
        {
            string spriteName = transform.GetComponent<Image>().sprite.name;
            _hero = (spriteName.UpperFirstLetter()).String2Enum<Hero>();
            //Debug.LogFormat("spriteName转_hero=>{0}转{1}", spriteName, _hero);

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        Unselected();
    }

    public override void FrameUpdate()
    {
        bool isSelected;
#if QFRAMEWORK
        isSelected=_hero == this.GetModel<IAirCombatAppStateModel>().SelectedHero;
#else
        isSelected=_hero == GameStateModel.Single.SelectedHero;

#endif
        // Debug.Log($"_hero == GameStateModel.Single.SelectedHero=>{_hero}=={GameStateModel.Single.SelectedHero}" );
        if (isSelected)
            Selected();
        else
            Unselected();
    }
    #endregion



    #region 辅助
    private void Selected()
    {
        _image.DOKill();
        _image.DOColor(_selectedColor, _time);
    }

    public void Unselected()
    {
        _image.DOKill();
        _image.DOColor(_defaultColor, _time);
    }
    #endregion


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}