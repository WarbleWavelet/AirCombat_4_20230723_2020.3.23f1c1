using UnityEngine;
using UnityEngine.UI;
using QFramework;
using QFramework.AirCombat;
using UnityEngine.Purchasing;

namespace QFramework.AirCombat
{

	public  class LevelItemClickable : MonoBehaviour, IController
    {
		public void Init()
		{
			this.SendCommand(new InstantiateLevelItemClickableCommand(transform));
            //
            this.RegisterEvent<UpdateLevelSelectableLockStateEvent>(eventId =>
			{
				//����ս�Ѿ�ͨ�ص���һ��
				int cur = transform.GetSiblingIndex();
				int tar = this.GetModel<IAirCombatAppModel>().PassedLevel ;
				Button maskBtn = transform.GetButtonDeep(GameObjectName.MaskBtn);

				bool show = (cur <= tar);
				if (show)
				{
                    maskBtn.Hide();
				}
				else
				{
                    maskBtn.Show();
				}
			});

		}

		#region ��дʵ�� 

        public IArchitecture GetArchitecture()
        {
			return AirCombatApp.Interface;
        }
		#endregion

    }
}
