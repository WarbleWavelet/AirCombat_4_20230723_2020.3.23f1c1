using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Collections.Generic;

namespace QFramework.AirCombat
{
    [BindPrefabAttribute(ResourcesPath.PREFAB_SELECTED_HERO_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
    public partial class SelectHeroPanel : MonoBehaviour,IInit,IController
	{

		List<RectTransform> _heroRectLst = new List<RectTransform>();

		public  void Init()
		{

			#region ��ʼ��


			RectTransform HeroTrans = transform.GetComponentDeep<RectTransform>(GameObjectName.HerosTrans);

            foreach (RectTransform rect in HeroTrans)
			{
				_heroRectLst.Add(rect);
			}

			foreach (var rect in _heroRectLst)
			{
				Transform t=rect.transform;
				t.GetOrAddComponent<HeroSelectableCtrl>().Init();
			}
			#endregion


			#region ��ť

		   transform.GetComponentDeep<Button>(GameObjectName.StartBtn).onClick.AddListener(() =>
			{ 
				//this.GetSystem<IUISystem>().Open(Paths.PREFAB_LEVELS_VIEW);
                this.SendCommand<OpenSelectLevelPanelCommand>();
            });

            transform.GetComponentDeep<Button>(GameObjectName.StrengthenBtn).onClick.AddListener(() => 
			{
				this.SendCommand < OpenStrengthenPanelCommand>();
            });

            transform.GetComponentDeep<Button>(GameObjectName.ExitBtn).onClick.AddListener(() => 
			{
				this.SendCommand<OpenStartGamePanelCommand>();
            });
			#endregion
        }


		#region ��д

        public IArchitecture GetArchitecture()
        {
			return AirCombatApp.Interface;
        }
		#endregion

    }
}
