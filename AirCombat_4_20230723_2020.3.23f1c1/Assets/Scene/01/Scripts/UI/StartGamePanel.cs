using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System.Threading;

namespace QFramework.AirCombat
{
    [BindPrefabAttribute(ResourcesPath.PREFAB_STARTGAME_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
    public partial class StartGamePanel :MonoBehaviour,IInit, IController
	{
		public  void Init()
		{
			GetComponentInChildren<Button>().onClick.AddListener(() =>
			{
                this.GetSystem<IAudioSystem>().PlaySound(AudioUI.UI_StartGame);
				this.SendCommand<OpenSelectHeroPanelCommand>();
                
            });
            this.GetSystem<IAudioSystem>().PlayMusic(AudioBG.Game_BGM);
        }

        public IArchitecture GetArchitecture()
        {
			return AirCombatApp.Interface;
        }
    }
}
