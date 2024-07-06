/****************************************************
    文件：AirCombatInspectorCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2024/7/5 23:24:5
	功能：z作为AirCombatInspector的因变量输入控制,放在一起好看点
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace QFramework.AirCombat
{
    public class AirCombatInspectorCtrl : MonoBehaviour ,IController
    {
        [SerializeField] PlanePlayerCtrl Player;
        [SerializeField] bool _listenPlayer;
        [SerializeField] int _planeBulletLevel;
        //
        /// <summary>升满级了,就不能突出飞机升级图片的变化</summary>
        [SerializeField] bool _clearAllPlaterPres;

        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }

        private void Start()
        {
            { //InitpLayer之前监听
                _listenPlayer = false;
                this.RegisterEvent<InitPlayerEvent>(_ => _listenPlayer = true);
            }
            //

            if (_clearAllPlaterPres)
            {
                this.GetUtility<IStorageUtil>().ClearAll();
            }
        }
        private void Update()
        {  
             this.GetModel<IAirCombatAppStateModel>().PlaneBulletLevel.Value = _planeBulletLevel;
            return;
            if (Input.GetKeyDown(KeyCode.L))
            {
                this.SendCommand(new SetPlaneSpriteLevelCommand(_planeBulletLevel));
            }
          
            if (Input.GetKeyDown(KeyCode.B))
            {
                this.SendCommand<AwakeBossCreatorEventCommand>();
            }
            if (_listenPlayer)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    this.SendCommand(new SpawnARewardCommand(RewardType.SHIELD, Player.transform.position));
                }
                if (Input.GetKeyDown(KeyCode.P))
                {
                    this.SendCommand(new SpawnARewardCommand(RewardType.POWER, Player.transform.position));
                }

            }
        }
    }
}



