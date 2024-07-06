/****************************************************
    文件：MainCameraCtrl.cs
	作者：lenovo
    邮箱: 
    日期：2023/8/16 21:11:50
	功能：
*****************************************************/

using SnakeGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using static ISpeed;
using Random = UnityEngine.Random;
 
namespace QFramework.AirCombat
{
    public class PlanePlayerCtrl : MonoBehaviour,IController  
    {
        #region 属性


        [SerializeField] PlayerEnterAniComponent _playerEnterAniCpt;
        [SerializeField] BulletType _bulletType;
        /// <summary>一个用来初始化子弹位置的节假电脑,避免玩家子弹和玩家自己碰撞</summary>
        [SerializeField] Transform _muzzlesTrans;
        [SerializeField] ShootCtrl _shootCtrl;
        [SerializeField] Transform _moveTrans;

        [SerializeField] CameraMoveOtherComponent _cameraMoveCpt;
        [SerializeField] ADWSMoveOtherComponent _planeMoveCpt;
        [SerializeField] Transform _colliderTrans;
        [SerializeField] PlayerDestroyComponent _playerDestroyCpt;
        [SerializeField] CollideMsgFromPlaneComponent _collideMsgFromPlaneComponent;
        [SerializeField] InvincibleComponent _invincibleComponent;
        [SerializeField] SpriteRenderer _sr;
        #endregion


        #region 生命
        public void InitStrictly(BulletType bulletType)
        {
            gameObject.tag = Tags.PLAYER;
            _bulletType = bulletType;
            _sr=GetComponent<SpriteRenderer>(); 
            // _muzzlesTrans = transform.FindOrNew(GameObjectName.BulletRoot);
            _muzzlesTrans = transform.FindOrNew(GameObjectName.Muzzles); 
            _moveTrans = transform.FindOrNew(GameObjectName.Move);
            _colliderTrans = transform.FindOrNew(GameObjectName.Collider);

            this.RegisterEvent<PlayerCanControlEvent>(_ =>
            {    
                gameObject.GetOrAddComponent<Trigger2DComponent>();
                 //
                _cameraMoveCpt = _moveTrans.GetOrAddComponent<CameraMoveOtherComponent>().Init(transform, this.GetModel<IAirCombatAppStateModel>().CameraSpeed);
              float speed=  this.GetModel<IAirCombatAppStateModel>().PlaneSpeed;
                _planeMoveCpt = _moveTrans.GetOrAddComponent<ADWSMoveOtherComponent>().Init(transform, speed);
                //                                                                  
                _shootCtrl = this.SendCommand(new  InitShootCtrlPlayerCommand(_muzzlesTrans, _bulletType));
               _playerDestroyCpt = _colliderTrans.GetOrAddComponent<PlayerDestroyComponent>().InitEntity(transform);
                //
                float invincibleTime = 1f;
                _invincibleComponent = _colliderTrans.GetOrAddComponent<InvincibleComponent>().InitParas( invincibleTime,_sr);
                _collideMsgFromPlaneComponent = _colliderTrans.GetOrAddComponent<CollideMsgFromPlaneComponent>().InitComponent(_shootCtrl.GetBulletModelComponent(),_playerDestroyCpt,_invincibleComponent);

                Destroy(_playerEnterAniCpt);
            });
            //
            _playerEnterAniCpt = gameObject.GetOrAddComponent<PlayerEnterAniComponent>().InitComponent();

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



