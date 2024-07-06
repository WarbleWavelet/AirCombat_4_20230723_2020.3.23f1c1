using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeedDes = ISpeed.SpeedDes;

/// <summary>导弹</summary>
public class MissileView : PlaneLevelView,IUpdate, QFramework.IController    ,ICollidePlayer
{

	[SerializeField]private int _numOfWarning;
	[SerializeField]private float _eachWarningTime;
	[SerializeField] private Action _endAction;
	[Header("主体")]

	[SerializeField] SpriteRenderer _sr;
	[SerializeField]Trigger2DComponent _trigger2DComponent;
    [SerializeField] BulletType _bulletType;
    [SerializeField] IBulletModel _bulletModel;
    [SerializeField] string _despawnKey;


	[Header("Bullet")]
    [SerializeField] private Transform _bulletTrans;
    [SerializeField] BulletModelComponent _bulletModelComponent;
    [Header("Move")]
	[SerializeField] private Transform _moveTrans;
	[SerializeField]private float _bulletSpeed;
	[SerializeField]private float _cameraSpeed;
	[SerializeField] MoveOtherComponent _moveOther;
	[SerializeField]private CameraMoveSelfComponent _cameraMove;
	[SerializeField]private bool _startMove;
	[SerializeField] private Vector2 _dir;

	[Header("Life")]
    [SerializeField] private Transform _lifeTrans;
    [SerializeField] AutoDespawnOtherComponent _autoDespawnOtherComponent;
    [SerializeField] DespawnBulletComponent _despawnBulletComponent;


    [Header("碰撞Collider")]
	[SerializeField] private Transform _colliderTrans;
	[SerializeField] private CollideMsgComponent _collideMsgComponent;
	//取决将火箭导弹看成是子弹还是飞机敌人
	//[SerializeField] private CollideMsgFromBulletComponent _collideMsgFromBulletComponent;
	[SerializeField] private CollideMsgFromPlaneComponent _collideMsgFromPlaneComponent;



	#region 生命	
	protected override void InitComponent()
	{
		GameObject go = gameObject;
		{
			_bulletType=BulletType.POWER;
			_despawnKey = ResourcesPath.PREFAB_ENEMY_MISSILE;
            _bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(_bulletType);
            _trigger2DComponent = go.GetOrAddComponent<Trigger2DComponent>().InitComponent();
			_sr = go.GetComponentOrLogError<SpriteRenderer>();

        }
        go.GetOrAddComponent<Trigger2DComponent>();
		transform.FindOrNew(GameObjectName.Collider).GetOrAddComponent<CollideMsgFromItemComponent>().Init(CollidePlayer);
		//导弹自身还有另外的MoveComponent，所以不能GetOrAdd
		float _cameraSpeed = this.GetModel<IAirCombatAppStateModel>().CameraSpeed;
		_cameraMove = go.GetOrAddComponent<CameraMoveSelfComponent>().InitComponent(_cameraSpeed);
		//
		_moveTrans = transform.FindOrNew(GameObjectName.Move);
		{
			//_dir = _pathCalc.GetDir().normalized;
			_dir = Vector2.down; ;

            _moveOther = MoveOtherComponent.InitMoveComponentKeepDesption(gameObject, _moveTrans.gameObject, _moveOther, _bulletSpeed, ISpeed.SpeedDes.BULLETSPEED);
		}
		_lifeTrans = transform.FindOrNew(GameObjectName.Life);
		{
			_despawnBulletComponent = _lifeTrans.GetOrAddComponent<DespawnBulletComponent>().Init(transform, _despawnKey);
			_autoDespawnOtherComponent = _lifeTrans.GetOrAddComponent<AutoDespawnOtherComponent>().Init(transform, _despawnKey, _sr);
		}
		_colliderTrans = transform.FindOrNew(GameObjectName.Collider);	//自己就是子弹||自己就是飞机的一种
		{
			_collideMsgComponent = _colliderTrans.GetOrAddComponent<CollideMsgComponent>().InitComponent(_trigger2DComponent);
			//_bulletEffectComponent = _bulletTrans.GetOrAddComponent<BulletEffectComponent>().Init(_bulletType, _bulletTrans);
			//_collideMsgFromBulletComponent = _colliderTrans.GetOrAddComponent<CollideMsgFromPlaneComponent>().InitComponent(_bulletModel, _despawnBulletComponent);
			_collideMsgFromPlaneComponent = _colliderTrans.GetOrAddComponent<CollideMsgFromPlaneComponent>().InitComponent(_bulletModel, _despawnBulletComponent);
			_bulletModelComponent = _colliderTrans.GetOrAddComponent<BulletModelComponent>().Init(_bulletModel);

		}
	}
	public void Init(float x,int numOfWarning,float eachWarningTime,float speed)
	{
		_startMove = false;
		_numOfWarning = numOfWarning;
		_eachWarningTime = eachWarningTime;
		_bulletSpeed = speed;
		base.Init();
		InitPos(x);
		InitLight();
	}



	protected override void OnEnable()
	{
		base.OnEnable();
		this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
	}
	
	
	private void OnDisable()
	{
		_endAction.DoIfNotNull();
		this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE,this);
	}


	public int Framing { get; set; }
	public int Frame { get; }
	public void FrameUpdate()
	{
		if (_startMove )
		{													
			_moveOther.Move(_dir);
		}
	}
	#endregion

	public void CollidePlayer()
	{
		this.GetSystem<IGameObjectPoolSystem>().DespawnWhileKeyIsName(gameObject);
		this.GetSystem<IAudioSystem>().PlaySound(AudioGame.Explode_Plane);
		this.GetSystem<IAniSystem>().OnPlaneDestroyAni(new PlaneDestroyAniEvent { pos = transform.position });
	}
	#region pro


	#endregion


	#region pri
	private void InitPos(float x)
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		float srH = sr.BoundsSizeY();
		float y = transform.CameraSizeMax().y + srH / 2;
		transform.position = ExtendVector3.SetXY( transform.position, x, y);

	}

	private void InitLight()
	{
		var go = this.GetSystem<IGameObjectPoolSystem>().Spawn(ResourcesPath.PREFAB_ITEM_LIGHT);
		var warning = go.GetOrAddComponent<MissileWarningView>();
		go.SetParent(transform);
		warning.AddEndListener(StartMove);
		warning.Init(transform.position.x, _numOfWarning, _eachWarningTime);

	}

	private void StartMove()
	{
		_startMove = true;
		Destroy(gameObject.GetComponent<CameraMoveSelfComponent>());
	}


	#endregion


	#region pub

	public void AddEndListener(Action endAction)
	{
		_endAction = endAction;
	}
	#endregion

	#region 重写
	public IArchitecture GetArchitecture()
	{
		return AirCombatApp.Interface;
	}
	#endregion

}
