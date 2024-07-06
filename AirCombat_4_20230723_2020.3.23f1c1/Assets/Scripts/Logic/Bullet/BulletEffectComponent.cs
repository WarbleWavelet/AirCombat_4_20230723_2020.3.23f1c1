using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffectComponent : MonoBehaviour	 ,ICanSendCommand
{
	private BulletEffectContainerBase _effect;

    public BulletEffectComponent Init(BulletType bulletType,Transform initTrans)
	{
		switch (bulletType)
		{
			case BulletType.ENEMY_BOSS_1:
				_effect = this.SendCommand(new SpawnBulletEffectViewCommand(bulletType));
				break;
		}

		if (_effect == null)
		{ 
			return this;
		}
		
		_effect.Init(initTrans);
		_effect.Begin();

		return this;
	}

	private void OnDisable()
	{
		if (_effect == null)
		{ 
			return;
		}
		
		_effect.Stop(null);
		_effect.Recycle2Cache();
        _effect = null;
	}

    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}
