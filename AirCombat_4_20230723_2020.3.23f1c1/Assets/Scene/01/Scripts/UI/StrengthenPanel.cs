using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;
using ItemKey = PropertyItem.ItemKey;
using LitJson;
using System.Numerics;
using Newtonsoft.Json.Linq;
using UniRx.Triggers;
using System;
using System.Data;

namespace QFramework.AirCombat
{
    [BindPrefabAttribute(ResourcesPath.PREFAB_STRENGTHEN_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
    public partial class StrengthenPanel : MonoBehaviour,IInit,IController 
    {

        #region 字属
        Button UpgradesBtn;
        List<RectTransform> _propertyItemLst;      
        #endregion


        public  void Init()
        {
            _propertyItemLst = new List<RectTransform>();
            _propertyItemLst.Add(transform.GetComponentDeep<RectTransform>(GameObjectName.PropertyItem_Attack));
            _propertyItemLst.Add(transform.GetComponentDeep<RectTransform>(GameObjectName.PropertyItem_FireRate));
            _propertyItemLst.Add(transform.GetComponentDeep<RectTransform>(GameObjectName.PropertyItem_Life));
             UpgradesBtn = transform.GetComponentDeep<Button>(GameObjectName.UpgradesBtn);
          //   Text _attackCos = transform.FindByPath<Text>("Property/PropertyItem_Attack/Cost");


            #region Top

            this.GetModel<IAirCombatAppModel>().Star.RegisterWithInitValue(value=>
            {
                transform.GetComponentDeep<Text>(GameObjectName.StarText).SetText(value);

            }).UnRegisterWhenGameObjectDestroyed(this); 
            this.GetModel<IAirCombatAppModel>().Diamond.RegisterWithInitValue(value =>
            {
                transform.GetComponentDeep<Text>(GameObjectName.DiamondText).SetText(value);
            }).UnRegisterWhenGameObjectDestroyed(this);

            #endregion


            #region Center


            transform.GetComponentDeep<Button>(GameObjectName.LeftBtn).onClick.AddListener(() =>
            {
                this.SendCommand(new SwitchPlaneCommand(EDir.LEFT));
            });
            transform.GetComponentDeep<Button>(GameObjectName.RightBtn).onClick.AddListener(() =>
            {
                this.SendCommand(new SwitchPlaneCommand(EDir.RIGHT));
            });


            this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID.RegisterWithInitValue(id=>
            { 
                SetPlaneSprite();
                UpdatePropertyItems();
                UpdateUpgradesButton();            
            }).UnRegisterWhenGameObjectDestroyed(this); 

            this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel.RegisterWithInitValue(level =>
            {
                SetPlaneSprite();
                UpdatePropertyItems();
                UpdateUpgradesButton();
            }).UnRegisterWhenGameObjectDestroyed(this);

            this.RegisterEvent<IncreasePlanePropertyEvent>(eventId =>//增加属性时
            {
             
                  UpdatePropertyItems();
            });
            #endregion


            #region Bottom


            transform.GetComponentDeep<Button>(GameObjectName.BackBtn).onClick.AddListener(() => 
            {
                //SelectHeroPanel的push由SelectHeroPanel操作
                this.SendCommand<OpenSelectHeroPanelCommand>();
            });
            UpgradesBtn.onClick.AddListenerAfterRemoveAll(() =>
            {
                this.SendCommand(new SetPlaneSpriteLevelCommand(this.GetModel<IAirCombatAppStateModel>().SelectedPlaneSpriteLevel+1));
                SetPlaneSprite();
            });
            #endregion


            #region 初始化
            {

                this.SendCommand(new SetSelectedPlaneIDCommand(0));//在跑一次，去触发Register的
            }
            #endregion

        }


        #region pri
        void UpdatePropertyItems()
        {
            UpdateValueText();
            UpdateSlider();
            UpdateAddButton();
            UpdateCostStarText();
        }

        private void UpdateAddButton()
        {
            foreach (var rect in _propertyItemLst)
            {

               //
                Button addBtn = rect.Find(GameObjectName.Add).GetComponent<Button>();
                addBtn.onClick.AddListenerAfterRemoveAll(() =>
                {   

                        this.SendCommand (new IncreasePlanePropertyCommand( rect)  );
                });
            }
        }


        private void UpdateUpgradesText()
        {
            int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
            string levelKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + DataKeys.LEVEL);
            int levelValue = this.GetUtility<IStorageUtil>().Get<int>(levelKey);
            string costKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + DataKeys.UPGRADES + levelValue);
            int costValue = this.GetUtility<IStorageUtil>().Get<int>(costKey);
            //
           UpgradesBtn.SetButtonText(costValue);
        }


        private void UpdateUpgradesButton()
        {

            #region 说明
            /**
            "planes": [
                "planeId": 0,
                  "level": 0,
                  "attackTime":1,
                  "attack": { "name":"攻击","costValue":5,"costValue":180,"costUnit":"star","grouth":10,"maxVaue": 500},
                  "fireRate": { "name":"攻速","costValue":80,"costValue":200,"costUnit":"star","grouth":1,"maxVaue": 100},
                  "life": { "name":"生命","costValue":100,"costValue":200,"costUnit":"star","grouth":50,"maxVaue": 1000},
                  "upgrades": { "name":"升级","coefficient": 2,"max":4,"0": 100,"1": 200,"2": 300,"3": 400,"costUnit":"diamond"}
                        },
            **/
            #endregion


            //
            UpdateUpgradesText();
        }

        private void UpdateCostStarText()
        {
            foreach (var rect in _propertyItemLst)
            {
                string property = (rect.name).TrimName(TrimNameType.DashAfter).LowerFirstLetter();//attack,fireRate,life
                int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
                string pre = planeID + property;
                string costKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(pre + PropertyItem.ItemKey.cost);
                int costValue = this.GetUtility<IStorageUtil>().Get<int>(costKey);
                //
                rect.Find(GameObjectName.Cost).SetText(costValue);
            }
        }

        private void UpdateValueText()
        {


            #region 说明
            /*
               "planes": [
                {
                "planeId": 0,
                "level": 0,
                "attackTime":1,
                "attack": {"name":"攻击","costValue":5,"costValue":180,"costUnit":"star","grouth":10,"maxVaue": 500},
                "fireRate": {"name":"攻速","costValue":80,"costValue":200,"costUnit":"star","grouth":1,"maxVaue": 100},
                "life": {"name":"生命","costValue":100,"costValue":200,"costUnit":"star","grouth":50,"maxVaue": 1000},
                "upgrades": {"name":"升级","coefficient": 2,"max":4,"0": 100,"1": 200,"2": 300,"3": 400,"costUnit":"diamond"}
                },
             */
            #endregion
            foreach (var rect in _propertyItemLst)
            {
                string property = (rect.name).TrimName(TrimNameType.DashAfter).LowerFirstLetter();//attack,fireRate,life
                int planeID = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
                string valueKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(planeID + property + PropertyItem.ItemKey.value);
                int value = this.GetUtility<IStorageUtil>().Get<int>(valueKey);
                //
                rect.Find(GameObjectName.Value).SetText(value);
            }
        }

        void SetPlaneSprite()
        {

            Sprite planeSprite = this.SendQuery(new SelectedPlaneSpriteQuery());
           transform.GetComponentDeep<Image>(GameObjectName.IconImg).SetSprite(planeSprite);
        }




        /// <summary>改成根据父节点，去更新子节点</summary>    
        private void UpdateSlider()
        {
            foreach (var rect in _propertyItemLst)
            {
                int id = this.GetModel<IAirCombatAppStateModel>().SelectedPlaneID;
                string property = (rect.name).TrimName(TrimNameType.DashAfter).LowerFirstLetter();//attack
                string pre = id + property;
                Slider slider = rect.Find(GameObjectName.Slider).GetComponent<Slider>();
                string maxVaueKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(pre + ItemKey.maxVaue); //0attackmaxVaue(原文没加l)
                string valueKey = this.GetUtility<IKeysUtil>().GetPropertyKeys(pre + ItemKey.value);//0attackvalue
                float maxValue=this.GetUtility<IStorageUtil>().Get<int>(maxVaueKey);
                float value= this.GetUtility<IStorageUtil>().Get<int>(valueKey);
                Debug.LogFormat("Slider：{0},{1}", valueKey, maxVaueKey);
                slider.minValue = 0;
                slider.maxValue = maxValue;
                slider.value = value;
                Debug.LogFormat("Slider：{0},{1},{2}", slider.minValue, slider.value, slider.maxValue);
            }
        }
        #endregion


        #region QF
        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion
    }
}
