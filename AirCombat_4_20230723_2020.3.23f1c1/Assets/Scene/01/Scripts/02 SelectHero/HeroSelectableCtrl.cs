/****************************************************
    文件：HeroSelectableCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/12 19:4:2
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using DG.Tweening;
using System.Runtime.ConstrainedExecution;
using UnityEngine.Purchasing;

namespace QFramework.AirCombat
{
    public class HeroSelectableCtrl : MonoBehaviour , IController
    {
        #region 属性

        Image _image;
        int _index;
        #endregion

        #region 生命



        /// <summary>首次载入且Go激活</summary>
       public void Init()
        {
            _image = GetComponent<Image>();
             _index = transform.GetSiblingIndex();
            //
            SetSelectState();
            this.GetModel<IAirCombatAppStateModel>().SelectHeroID.Register(id=>
            {
                SetSelectState();
            }); 

           //
            GetComponent<Button>().onClick.AddListenerAfterRemoveAll(() =>
            { 
                Debug.Log("HeroSelectable");
                //
                this.GetModel<IAirCombatAppStateModel>().SelectHeroID.Value = transform.GetSiblingIndex();
            });
        }
        #endregion

        public void SetSelectState()
        {
            bool selected = _index == this.GetModel<IAirCombatAppStateModel>().SelectHeroID;
            IAirCombatAppModel model = this.GetModel<IAirCombatAppModel>();
            if (selected)
            {
                this.GetSystem<IAudioSystem>().PlayVoice(_index.Int2String<Hero>());
                _image.DOKill();
                _image.DOColor(model.SelectHeroColor, model.SelectHeroColorTime);
            }
            else
            {
                _image.DOKill();
                _image.DOColor(model.UnSelectHeroColor, model.SelectHeroColorTime);
            }
        }

        #region 系统

        #endregion




        #region 实现
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion
    }
}



