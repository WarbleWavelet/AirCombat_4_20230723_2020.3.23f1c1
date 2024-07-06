/****************************************************
	文件：MainCameraCtrl.cs
	作者：lenovo
	邮箱: 
	日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static ISpeed;
using Random = UnityEngine.Random;
using Common;

namespace QFramework.AirCombat
{
	public class PlaneEnemyCtrl : PlaneLevelView, IController ,IUpdate 
	{
        #region 属性
        //

        [SerializeField] EnemyType _enemyType;
        [SerializeField] private EnemyData _enemyData;

		//
		public ColliderAdaptRenderComponent RendererComponent { get; private set; }
		[SerializeField] private SpriteRenderer _sr;
		[SerializeField] private CapsuleCollider2D _capsuleCollider2D;
		[SerializeField] private Rigidbody2D _rigidbody2D;
		[SerializeField] private string  _despawnKey;
		[SerializeField] EDir[] _execuldeDirs=new EDir[0];
		[SerializeField] MessageMgrComponent _messageMgrComponent;
        [SerializeField] private Trigger2DComponent _trigger2DComponent;

		[Header("移动")]
        [SerializeField] string _pathName ; //序列化不显示
        [SerializeField] private PathState _pathState;
		private PathMgr _pathMgr;
		/// <summary>Boss撞击玩家,尝试找出是什么变量控制的</summary>
		[SerializeField] private Vector2 _moveDir;			
		[SerializeField] private Transform _moveTrans;			
		[SerializeField] private MoveOtherComponent _otherMove;
		[SerializeField] private CameraMoveOtherComponent _cameraMove;


		[Header("主体")]
		[SerializeField] private Transform _planeTrans;
		[SerializeField] private EnemyTypeComponent _enemyTypeComponent;


		[Header("子消息")]
        [SerializeField] private Transform _colliderTrans;
        [SerializeField] private CollideMsgComponent _collideMsgComponent;
		[SerializeField] private CollideMsgFromPlaneComponent _collideMsgFromPlaneComponent;
		[SerializeField] private CollideMsgFromBulletComponent _collideMsgFromBulletComponent;





		[Header("生命")]
		[SerializeField] private AutoDespawnOtherComponent _autoDespawnOtherComponent;
		/// <summary>血条上的</summary>
		[SerializeField] private EnemyLifeComponent _enemyLifeComponent;
		/// <summary>飞机上的</summary>
		[SerializeField] private LifeComponent _lifeComponent;
		[SerializeField] private EnemyDestroyComponent _deadComponent;
		[SerializeField] private Transform _lifeTrans;


		//
		[Header("枪口")]
		/// <summary>单枪口就是枪口位置,多枪口就是多枪口父节点的位置参考</summary> 
		/// <summary>防止图片上下方向多种导致的偏差,用来复原</summary>
		[SerializeField] private Vector3 _muzzlesLocalPos;
		[SerializeField] private Transform _muzzlesTrans;
		[SerializeField] private ShootCtrlsComponent _shootCtrlsComponent;
		[SerializeField] private ShootCtrl[] _shootCtrlArr;
		[SerializeField] private BulletModelComponent[] _bulletModelComponentArr;

		#region Plane数据类
		/**
		  "NORMAL": [
			{
			  "id": 0,
			  "attackTime":1,
			  "attack": 0,
			  "fireRate": 0,
			  "life": 50,
			  "speed": 1,
			  "TrajectoryType": 0,
			  "trajectoryID": -1,
			  "bulletType": [1],
			  "starNum": 1,
			  "score": 1,
			  "itemProbability": 10,
			  "itemRange": [0,0],
			  "itemCount": 1
			}, */
		#endregion
		#endregion


		#region 生命

		/// <summary>
		/// 
		/// </summary>
		/// <param name="idxInQueue">阵列中的位置ID，实例时的索引</param>
		/// <param name="enemyType"></param>
		/// <param name="enemyData"></param>
		/// <param name="sprite"></param>
		/// <param name="pathData"></param>
		public PlaneEnemyCtrl Init(int idxInQueue, EnemyType enemyType, EnemyData enemyData, Sprite sprite, IPathData pathData,string despawnKey ,Vector3 creatorPos )
		{
			
			_enemyType =enemyType;
			gameObject.tag = Tags.ENEMY;
			_despawnKey=despawnKey;
			_enemyData = enemyData;
		   GameObject go = gameObject;
			{
				_messageMgrComponent = go.GetOrAddComponent<MessageMgrComponent>().InitComponent();
				_sr = go.GetComponentOrLogError<SpriteRenderer>();
				_capsuleCollider2D = go.GetComponentOrLogError<CapsuleCollider2D>();
				RendererComponent = go.GetOrAddComponent<ColliderAdaptRenderComponent>().InitComponent(_sr, _capsuleCollider2D);
				RendererComponent.SetSprite(sprite);
				_rigidbody2D = go.GetComponentOrLogError<Rigidbody2D>();
                _trigger2DComponent = go.GetOrAddComponent<Trigger2DComponent>().InitComponent(_rigidbody2D);

            }
            _moveTrans = transform.FindOrNew(GameObjectName.Move);
			{
				_pathMgr = new PathMgr(transform, _enemyData, pathData,creatorPos);
				{
					Vector3 posFromPath = _pathMgr.GetFromPos(idxInQueue);
					transform.SetPos(  posFromPath);
				}

				float speed = (float)_enemyData.speed;
				_otherMove = MoveOtherComponent.InitMoveComponentKeepDesption(gameObject,_moveTrans.gameObject, _otherMove, speed, SpeedDes.PLANESPEED);
				_cameraMove = _moveTrans.GetOrAddComponent<CameraMoveOtherComponent>().Init(transform, this.GetModel<IAirCombatAppStateModel>().CameraSpeed);
				_cameraMove.enabled = _pathMgr.FollowCamera(); //有MoveComp
			}
			//
			_muzzlesTrans = transform.Find(GameObjectName.Muzzles);
            _muzzlesLocalPos = _muzzlesTrans.localPosition;
            {
				int length = _enemyData.bulletType.Length;
                if (length > 1)//多枪口 //尝试多单枪口统合初始化
				{
                    _muzzlesTrans.ReverseLocalPosY();   //设置局部坐标y取反	预制体默认向上
				}
                _shootCtrlsComponent = _muzzlesTrans.GetOrAddComponent<ShootCtrlsComponent>().Init(_enemyData, _muzzlesTrans);
                _shootCtrlArr = new ShootCtrl[_muzzlesTrans.childCount];
                _bulletModelComponentArr = new BulletModelComponent[_muzzlesTrans.childCount];
                //for (int i = 0; i < _muzzlesTrans.childCount; i++) //用这个要管理子节点,全销毁或者留一个
                for (int i = 0; i < length; i++)//用这个不用考虑那么多,但是管理好节点更加好看点
                {
                    Transform muzzleTrans = _muzzlesTrans.GetChild(i);
                    BulletType bulletType = _enemyData.bulletType[i];
                    _shootCtrlArr[i] = muzzleTrans.GetComponent<ShootCtrl>().InitComponentEnemy(bulletType, enemyData, muzzleTrans);
                    _bulletModelComponentArr[i] = muzzleTrans.GetComponent<BulletModelComponent>();
                    _bulletModelComponentArr[i].Init(_shootCtrlArr[i].GetBulletModel());
                }


            }


			_planeTrans = transform.FindOrNew(GameObjectName.Plane);//所属的顺序需要打乱
			_lifeTrans = transform.FindOrNew(GameObjectName.Life);
			{
				if (_enemyType == EnemyType.BOSS)
				{
					_execuldeDirs = new EDir[4] { EDir.TOP, EDir.BOTTOM, EDir.LEFT, EDir.RIGHT };
				}
				else 
				{
					_execuldeDirs = new EDir[1] { EDir.TOP};
				}
				_autoDespawnOtherComponent = _lifeTrans.GetOrAddComponent<AutoDespawnOtherComponent>().Init(transform, _despawnKey, _sr, _execuldeDirs);


                _lifeComponent = _lifeTrans.GetOrAddComponent<LifeComponent>().Init(_enemyData.life);
				if (_enemyLifeComponent==null)  //未初始化血条
				{
					//这里是实例,实例跟_lifeTrans喝不起来,分开又不好看,所以设置为子节点
					_enemyLifeComponent = this.SendCommand(new SpawnEnemyLifeViewCommand(_lifeTrans, _lifeComponent, _sr));
				}
				else
				{
					STest.IsBossPlane(_enemyType);
					_enemyLifeComponent.Init(_lifeComponent, _sr);
				}
                _enemyTypeComponent = _planeTrans.GetOrAddComponent<EnemyTypeComponent>().Init(_enemyType);
				_deadComponent= _lifeTrans.GetOrAddComponent<EnemyDestroyComponent>().Init(_enemyData, _lifeComponent,transform,_enemyTypeComponent);
			}

            _colliderTrans = transform.FindOrNew(GameObjectName.Collider);
			{
				_collideMsgComponent = _colliderTrans.GetOrAddComponent<CollideMsgComponent>().InitComponent(_trigger2DComponent);
				_collideMsgFromPlaneComponent = _colliderTrans.GetOrAddComponent<CollideMsgFromPlaneComponent>().InitComponent(_bulletModelComponentArr, _deadComponent);
																																		   
			}

            base.OnEnable();
			this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
			return this;
		}
        #endregion


        #region IShow,IHide,IUpdate
        public int Framing { get; set; }
		public int Frame { get; }



		public void FrameUpdate()
		{    
			if (_sr.BoundsMaxY() < this.GetUtility<IGameUtil>().CameraMaxPoint().y) //完全进入
			{
				_messageMgrComponent.SendMsg(MsgEvent.EVENT_CANSHOOT ,true);
				STest.Log_CanShoot();
			
			}
			if (ExtendJudge.IsOnceNull(_otherMove, _pathMgr))
			{ 
				return;
			}
            //STest.IsBossPlane(_enemyType);
            _pathName = _pathMgr.PathName();
            _pathState = _pathMgr.GetPathState();
            _moveDir = _pathMgr.GetDir();
			_otherMove.Move(_moveDir);
		}


  

		private void OnDisable()
		{
            _muzzlesTrans.localPosition=_muzzlesLocalPos;
			_muzzlesTrans.DestroyChildren();
            gameObject.name= _despawnKey.TrimName(TrimNameType.SlashAfter);
			//
			CollideMsgFromPlaneComponent collidePlane = _colliderTrans.GetComponent<CollideMsgFromPlaneComponent>();

            if (collidePlane != null)//Boss不会被玩家撞死. Boss和其他飞机又是同一个Pool,所以要管理
			{
				ExtendObject.Destroy(collidePlane);	
			}
			this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
		}
		#endregion




		#region QF
		public IArchitecture GetArchitecture()
		{
			return AirCombatApp.Interface;
		}

		#endregion

	}
}



