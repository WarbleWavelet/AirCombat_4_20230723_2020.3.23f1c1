using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.AirCombat
{
    [BindPrefabAttribute(ResourcesPath.PREFAB_GAME_UI_PANEL, Const.BIND_PREFAB_PRIORITY_VIEW)]
    public  class GamePanel:MonoBehaviour,ICanGetModel 
	{


        void Awake()
		{
			Text scoreText= transform.Find("RightTopPin/Score").GetComponent<Text>();
			Text starText= transform.Find("RightTopPin/Star/Value").GetComponent<Text>();
			Text lifeText= transform.Find("LeftTopPin/Life/Value").GetComponent<Text>();
			Text shieldText = transform.Find("RightBottomPin/Shield/Num").GetComponent<Text>();
			Text bombText = transform.Find("RightBottomPin/Bomb/Num").GetComponent<Text>();
			Text hpText = transform.Find("LeftTopPin/Life/Value").GetComponent<Text>();
			this.GetModel<IAirCombatAppModel>().Score.RegisterWithInitValue((score)=>
            {
                scoreText.SetText(score);
            }).UnRegisterWhenGameObjectDestroyed(this);
            this.GetModel<IAirCombatAppModel>().Star.RegisterWithInitValue((star) =>
            {
                starText.SetText(star);
            }).UnRegisterWhenGameObjectDestroyed(this);
            this.GetModel<IAirCombatAppModel>().Life.RegisterWithInitValue((life) =>
            {
                lifeText.SetText(life);
            }).UnRegisterWhenGameObjectDestroyed(this);
            //
            this.GetModel<IAirCombatAppModel>().ShieldCount.RegisterWithInitValue((shield) =>
            {
                shieldText.SetText(shield);
            }).UnRegisterWhenGameObjectDestroyed(this);
            this.GetModel<IAirCombatAppModel>().BombCount.RegisterWithInitValue((bomb) =>
            {
                bombText.SetText(bomb);
            }).UnRegisterWhenGameObjectDestroyed(this);
        }


        #region QF
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion

    }
}
