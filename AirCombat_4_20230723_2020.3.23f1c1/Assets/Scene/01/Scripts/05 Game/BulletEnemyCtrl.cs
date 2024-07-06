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
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace QFramework.AirCombat
{
    public class BulletEnemyCtrl : MonoBehaviour, IController, IUpdate, IBulletCtrl
    {
        #region 属性
        [SerializeField] private float _bulletSpeed;
        /// <summary>一般是距离控制</summary>
        [SerializeField]float _destroyTimeTmp = 2f;
        [SerializeField]IBulletModel _bulletModel;
        [SerializeField]BulletType _bulletType;
        [SerializeField]MoveOtherComponent _moveOther;

        [Header("处理中")]
        [SerializeField] AutoDespawnOtherComponent _autoDespawnOtherComponent;

        [SerializeField]DespawnBulletComponent _despawnBulletComponent;
        [SerializeField]CollideMsgFromBulletComponent _collideMsgFromBulletComponent;
        [SerializeField]BulletModelComponent _bulletModelComponent;
        [SerializeField]BulletEffectComponent _bulletEffectComponent;
        [SerializeField]SpriteRenderer _sr;
        [SerializeField]IPathCalc _pathCalc;
        [SerializeField]Transform _moveTrans;
        [SerializeField]Transform _bulletTrans;
        [SerializeField] Transform _lifeTrans;
        [SerializeField] Vector2 _dir;
        [SerializeField] string _despawnKey;

        #endregion

        public BulletModelComponent GetBulletModelComponent()
        {
            return _bulletModelComponent;
        }

        #region 生命


        public IBulletCtrl Init(BulletType bulletType, float bulletSpeed, IPathCalc pathCalc)
        {

            _bulletType = bulletType;
            _bulletSpeed = bulletSpeed;
            _bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(bulletType);
            _despawnKey = ResourcesPath.PREFAB_BULLET;
            _pathCalc = pathCalc;

            GameObject go = gameObject;
            {
                _sr = GetComponent<SpriteRenderer>();
                _sr.sprite = _bulletModel.Sprite();
            }
            _moveTrans = transform.FindOrNew(GameObjectName.Move);
            {
                _dir = _pathCalc.GetDir().normalized;
                _moveOther =MoveOtherComponent.InitMoveComponentKeepDesption(gameObject,_moveTrans.gameObject, _moveOther,_bulletSpeed,ISpeed.SpeedDes.BULLETSPEED);
            }
            _lifeTrans = transform.FindOrNew(GameObjectName.Life);
            { 
                _despawnBulletComponent = _lifeTrans.GetOrAddComponent<DespawnBulletComponent>().Init(transform, _despawnKey);
               _autoDespawnOtherComponent= _lifeTrans.GetOrAddComponent<AutoDespawnOtherComponent>().Init(transform, _despawnKey, _sr);            
            }
            _bulletTrans = transform.FindOrNew(GameObjectName.Bullet);
            {
                _bulletEffectComponent=  _bulletTrans.GetOrAddComponent<BulletEffectComponent>().Init(_bulletType, _bulletTrans);
                _collideMsgFromBulletComponent = _bulletTrans.GetOrAddComponent<CollideMsgFromBulletComponent>().InitComponent(_bulletModel, _despawnBulletComponent);
                _bulletModelComponent = _bulletTrans.GetOrAddComponent<BulletModelComponent>().Init(_bulletModel);
            }
           //

            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);
            this.Enable();
            return this;
        }




        #endregion


        #region IUpdate

        public int Framing { get; set; }

        public int Frame { get; }


        public void FrameUpdate()
        {

            transform.FaceTo(_dir, EDir.DOWN);
            _moveOther.Move(_dir);
        }
        private void OnDisable()
        {
            this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
            gameObject.name = _despawnKey.TrimName(TrimNameType.SlashAfter);
            _bulletType = BulletType.NULL;//防止默认Player造成混乱    
            this.Unable();   
                
        }

        #endregion


        #region 实现


        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }


        #endregion

    }
}



