/****************************************************
    文件：MainCameraCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using ShootingEditor2D;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace QFramework.AirCombat
{
    public class BulletPlayerCtrl : MonoBehaviour,IController , IBulletCtrl,IUpdate 
    {
        #region 属性

        /// <summary>需要大于飞机速度，不然与飞机一起方向事相对静止很难看</summary>
        [SerializeField] private float _bulletSpeed=0f;
        /// <summary>一般是距离控制</summary>
        /// float _destroyTimeTmp = 2f;  // 距离控制有外部控制了
        [SerializeField]  BulletType _bulletType;
        [SerializeField]  IBulletModel _bulletModel;
        [SerializeField]  MoveOtherComponent _moveOther;
        [SerializeField]  SpriteRenderer _sr;
        [SerializeField]  BulletEffectComponent _bulletEffectCpt;
        [SerializeField]  DespawnBulletComponent _bulletDestroyCpt;
        [SerializeField]  BulletModelComponent _bulletModelCpt;
        [SerializeField]  CollideMsgFromBulletComponent _collideMsgFromBulletCpt;
        [SerializeField]  AutoDespawnOtherComponent _autoDespawnOtherCpt;
        [SerializeField]  Transform _moveTrans;
        [SerializeField]  Transform _bulletTrans;
        [SerializeField]  Transform _lifeTrans;
        [SerializeField] IPathCalc _pathCalc;
        [SerializeField] Vector2 _dir;
        [SerializeField] string _despawnKey;



        #endregion



        #region 生命


        public IBulletCtrl Init(BulletType bulletType,float bulletSpeed, IPathCalc pathCalc)
        {
            _despawnKey = ResourcesPath.PREFAB_BULLET;
            _pathCalc = pathCalc;
            _bulletType = bulletType;
            _bulletSpeed = bulletSpeed;
            _bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(bulletType);
            GameObject go = gameObject;
            {
                _sr = go.GetComponent<SpriteRenderer>();
                _sr.sprite = _bulletModel.Sprite();
            }
            _lifeTrans = transform.FindOrNew(GameObjectName.Life);
            { 
                _autoDespawnOtherCpt = _lifeTrans.GetOrAddComponent<AutoDespawnOtherComponent>().Init(transform,   _despawnKey,_sr);
                _bulletDestroyCpt= _lifeTrans.GetOrAddComponent<DespawnBulletComponent>().Init(transform, _despawnKey);
            
            }
            _moveTrans = transform.FindOrNew(GameObjectName.Move);
            {
                _dir = _pathCalc.GetDir().normalized;
                _moveOther = MoveOtherComponent.InitMoveComponentKeepDesption(gameObject, _moveTrans.gameObject, _moveOther, _bulletSpeed, ISpeed.SpeedDes.BULLETSPEED);
            }                
            _bulletTrans = transform.FindOrNew(GameObjectName.Bullet);
            { 
                _bulletModelCpt = _bulletTrans.GetOrAddComponent<BulletModelComponent>().Init(bulletType);
                _bulletEffectCpt= _bulletTrans.GetOrAddComponent<BulletEffectComponent>().Init(_bulletType, transform);
                _collideMsgFromBulletCpt = _bulletTrans.GetOrAddComponent<CollideMsgFromBulletComponent>().InitComponent(_bulletModelCpt, _bulletDestroyCpt) ;
            }


            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
            this.Enable();
            return this;


        }



        #region IUpdate
        public int Framing { get; set; }

        public int Frame {  get;   }
        public void FrameUpdate()
        {

            transform.FaceTo(_dir,EDir.UP);
            _moveOther.Move(_dir); 
        }
        #endregion

        private void OnDisable()
        {
            this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
            gameObject.name = _despawnKey.TrimName(TrimNameType.SlashAfter);
            _bulletType = BulletType.NULL;//防止默认Player造成混乱    
            this.Unable();
        }
        #endregion

       public BulletModelComponent GetBulletModelComponent()
        {
            return GetComponentInChildren<BulletModelComponent>();
        }


        #region QF
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }




        #endregion

    }
}



