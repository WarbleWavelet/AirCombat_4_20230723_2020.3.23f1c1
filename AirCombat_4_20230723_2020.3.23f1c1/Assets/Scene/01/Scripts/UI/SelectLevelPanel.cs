using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.AirCombat
{


	[BindPrefabAttribute(ResourcesPath.PREFAB_SELECTLEVEL_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]

	public partial class SelectLevelPanel : MonoBehaviour,IInit,IController
	{
		public  void Init()
		{
			// please add init code here
			//LevelsTrans._colliderTrans ;

			//LevelRoot levelRoot;
			//LevelRootController levelRootController;
			//LevelsView levelsView;
			//LevelsController levelsController;
			//LevelItem levelItem;
			//LevelItemController levelItemController;
			this.RegisterEvent<CloseSelectLevelPanelEvent>(_ => 
			{
				this.GetSystem<IUISystem>().CloseAll();
			});

			this.SendCommand(new InstantiateLeveItemClickablesCommand(transform.Find(GameObjectName.LevelsTrans).GetComponent<RectTransform>()));


            #region Bottom
            transform.GetComponentDeep<Button>(GameObjectName.BackBtn).onClick.AddListener(() =>
			{
                this.SendCommand<OpenSelectHeroPanelCommand>();
            });
			#endregion

		}


		#region ÖØÐ´


		public IArchitecture GetArchitecture()
		{
			return AirCombatApp.Interface;
		}
		#endregion
	}
}
