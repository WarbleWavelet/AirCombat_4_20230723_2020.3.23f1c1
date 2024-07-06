using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UniRx.Async.Triggers;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace QFramework.AirCombat
{
    [BindPrefabAttribute(ResourcesPath.PREFAB_LOADING_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
    public  class LoadingPanel : MonoBehaviour, IInit, IController
    {
        public void Init()
        {
            // please add init code here
            this.GetModel<IAirCombatAppStateModel>().E_GameState.Register(state =>
            {
                if (state != GameState.START) return;
                this.GetSystem<IUISystem>().Close(ResourcesPath.PREFAB_LOADING_PANEL);
            });
        }


        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
    }
}
