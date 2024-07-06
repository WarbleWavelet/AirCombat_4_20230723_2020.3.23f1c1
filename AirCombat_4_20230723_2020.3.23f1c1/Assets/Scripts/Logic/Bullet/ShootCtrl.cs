using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


/// <summary>Emit发出。挂在飞机的子节点下</summary>
public class ShootCtrl : MonoBehaviour,QFramework.IController,IUpdate 
{

	#region 字属
	[SerializeField] private int _coroutineID;
	//
	/// <summary>0.1-1</summary>

	[SerializeField] private float _shoot=0.5f;
	[SerializeField] private int _fireSoundPara=4;
	//[SerializeField] private Transform _muzzlesTrans;
	/// <summary>可以挂父节点,也可以挂子节点好看点,推荐挂子节点</summary>
	[SerializeField] private Transform _muzzleTrans;
	[SerializeField] private BulletPointsCalcEllipseComponent _bulletPointSpawnComponent;
	[SerializeField] private BulletLoadComponent _bulletLoadCpt;
	[SerializeField] private BulletShootComponent _bulletShootCpt;
	[SerializeField] private BulletModelComponent _bulletModelCpt;
	[SerializeField] private BulletSoundComponent _bulletSoundCpt;
	[SerializeField] private BulletType _bulletType;
	[SerializeField] MessageMgrComponent _messageMgrComponent;
	[SerializeField] private IBulletModel _bulletModel;
	[SerializeField] private SpriteRenderer _sr;
	[Header("如果是玩家")]
	[Header("如果是敌人")]
	[SerializeField] private EnemyData _enemyData;
	[SerializeField] bool _canEnemyShoot;
	[SerializeField] private string _despawnKey;
	[Header("如果是敌人Boss")]
	[SerializeField] private BulletBossEventComponent _bulletBossEventComponent;


	//
	#endregion

	//
	#region 生命


	#region 敌机 导弹


	/// <summary>敌人</summary>
	public ShootCtrl InitComponentEnemy(BulletType bulletType,EnemyData enemyData,Transform muzzleTrans)
	{
		_canEnemyShoot = false;
		_bulletType = bulletType;
		_muzzleTrans = muzzleTrans;
		_enemyData = enemyData;               
		_sr = gameObject.GetComponentInParentOrLogError<SpriteRenderer>();
		_despawnKey = ResourcesPath.PREFAB_BULLET;
		_messageMgrComponent = gameObject.GetComponentInParent<MessageMgrComponent>();
		_messageMgrComponent.AddListener(MsgEvent.EVENT_CANSHOOT, args => _canEnemyShoot = (bool)args[0]) ; 
		//
		_bulletPointSpawnComponent = _muzzleTrans.GetOrAddComponent<BulletPointsCalcEllipseComponent>().InitComponent(_sr, _muzzleTrans);
		_bulletLoadCpt = _muzzleTrans.GetOrAddComponent<BulletLoadComponent>();
		{
			_bulletLoadCpt.BulletCnt = 1;
			_bulletLoadCpt.LoadTime = 0f;
			_bulletLoadCpt.LoadBullet();

        }
		{ 
		    _bulletModelCpt = _muzzleTrans.GetOrAddComponent<BulletModelComponent>().Init(_bulletType, _enemyData);
		    _bulletModel = _bulletModelCpt.GetBulletMode();
			_bulletPointSpawnComponent = _muzzleTrans.GetOrAddComponent<BulletPointsCalcEllipseComponent>().InitComponent(_sr, _muzzleTrans);
			_bulletShootCpt = _muzzleTrans.GetOrAddComponent<BulletShootComponent>().InitComponent(_muzzleTrans, _bulletType, _bulletModel, _bulletPointSpawnComponent, _sr,_despawnKey);
			{
				_bulletShootCpt.Shooting = _shoot;
				_bulletShootCpt.Shoot = _shoot;
				_bulletShootCpt.CanShoot = true;
				_bulletShootCpt.BulletSpeed = 5f;
			}        
		}

		//

		_bulletBossEventComponent = InitBulletBossEventComponent( _bulletBossEventComponent,  _bulletModel);
		InitPos(_bulletModel);

		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
		return this;
	}


	/// <summary>Power</summary>
	public ShootCtrl InitComponentPower(BulletType bulletType)
	{
		_bulletType = bulletType;
		//
		_bulletModelCpt = _muzzleTrans.GetOrAddComponent<BulletModelComponent>().Init(_bulletType);
		_bulletModel = _bulletModelCpt.GetBulletMode();
		_bulletBossEventComponent = InitBulletBossEventComponent(_bulletBossEventComponent, _bulletModel);
		InitPos(_bulletModel);
		return this;
	}

	#endregion




	#region 玩家
	/// <summary>玩家</summary>
	public ShootCtrl InitComponentPlayer(BulletType bulletType,Transform muzzle)
	{
		_muzzleTrans =  muzzle;  
		_shoot = 0.1f;
		_bulletType = bulletType;
		_bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(_bulletType);
		_despawnKey = ResourcesPath.PREFAB_BULLET;
		_bulletLoadCpt = _muzzleTrans.GetOrAddComponent<BulletLoadComponent>();
		{
			_bulletLoadCpt.LoadTime = 0.5f ;
			_bulletLoadCpt.BulletCnt = 20;
			_bulletLoadCpt.BulletCnting = _bulletLoadCpt.BulletCnt;
			_bulletLoadCpt.LoadFinish = true;
		}

		{
			_bulletModelCpt = _muzzleTrans.GetOrAddComponent<BulletModelComponent>().Init(_bulletModel);  //别的地方Init Model
			_sr = gameObject.GetComponentInParentOrLogError<SpriteRenderer>();
			_bulletPointSpawnComponent = _muzzleTrans.GetOrAddComponent<BulletPointsCalcEllipseComponent>().InitComponent(_sr, _muzzleTrans);
			_bulletShootCpt = _muzzleTrans.GetOrAddComponent<BulletShootComponent>().InitComponent(_muzzleTrans, _bulletType, _bulletModel, _bulletPointSpawnComponent,_sr,_despawnKey);     
			{
				_bulletShootCpt.Shooting = 0f;
				_bulletShootCpt.Shoot = _shoot;
				_bulletShootCpt.CanShoot = true;
				_bulletShootCpt.BulletSpeed = 0.5f;
			}        
		}


		_bulletSoundCpt = _muzzleTrans.GetOrAddComponent<BulletSoundComponent>();
		{
			_bulletSoundCpt.FireSoundPara = _fireSoundPara;
		}

		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);

		return this;
	}

	#endregion  



	private void OnDisable()
	{
        this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
		this.GetSystem<ICoroutineSystem>().Stop(_coroutineID);
		//this.GetSystem<IAudioSystem>().Stop(_bulletModelCpt.BulletModel.AudioName);
		_coroutineID = -1;
	}
	#endregion


	#region IUpdate


	public int Framing { get; set; }

	public int Frame { get; }
	public void FrameUpdate()//射速 
	{
		if (_bulletType != BulletType.PLAYER )  //敌人开始射击受其它条件限制
		{
			if (_canEnemyShoot == false)
			{
				return;
			}
		}
		if ( ExtendJudge.IsOnceNullObject(_bulletLoadCpt,_bulletShootCpt,_bulletModelCpt))//,_bulletSoundCpt  敌人子弹没音效
		{

			Debug.LogError("空");
			return;
		}
	   // Test();

		if (_bulletShootCpt.ShootTimer())
		{
			if (_bulletLoadCpt.SubBullet())
			{
				_bulletSoundCpt.DoIfNotNull(()=>_bulletSoundCpt.PlaySound(_bulletLoadCpt.BulletCnting, _bulletLoadCpt.BulletCnt)) ;

				_bulletShootCpt.Fire();
			}
			else if(_bulletLoadCpt.LoadFinish)
			{
				_bulletLoadCpt.LoadBullet();
			}        
		}
	}

	#endregion


	#region pub

	public IBulletModel GetBulletModel()
	{

		return _bulletModelCpt.GetBulletMode();
	}
	public IBullet GetBulletModelComponent()
	{
		return _bulletModelCpt;
	}




    #endregion


    #region pri



    BulletBossEventComponent InitBulletBossEventComponent(BulletBossEventComponent bulletBossEventComponent, IBulletModel bulletModel)
	{
		if (bulletModel is IEnemyBossBulletModel && bulletModel.GetAttack() > 0)//一般是Boss
		{
			bulletBossEventComponent = gameObject.GetOrAddComponent<BulletBossEventComponent>().Init(bulletModel);
			return bulletBossEventComponent;
		}
		return null;
	}

	void Test()
	{
		_bulletShootCpt.Shoot = _shoot;
		_bulletSoundCpt.FireSoundPara = _fireSoundPara;
	}


	private void InitPos(IBulletModel model)
	{
		model.OnGotPathCalcArr((trajectorys) =>
		{
			var bulletDir = trajectorys[0].GetDir();
			Vector2 selfDir = transform.position - transform.parent.position;
			var angle = Vector2.Angle(selfDir, bulletDir);

			if (angle > 90)
			{
				transform.SetLocalPosY(-transform.localPosition.y);
			}
		});
	}


	#endregion


	#region GetArchitecture
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
	#endregion  

}
