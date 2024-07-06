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
using UnityEngine;
using Random = UnityEngine.Random;
using SpeedDes = ISpeed.SpeedDes;

namespace QFramework.AirCombat
{
    public class MapItemCtrl : MonoBehaviour,IController   ,IUpdate
    {
        #region 属性
        private float _offsetY;
        private CloudCtrl _cloud;
        private Transform _camera;
        private SpriteRenderer _sr;
        /// <summary>关卡</summary>
        private static int _curLevel;
        /// <summary>子弹和地图一样z=0,有时出现地图盖住子弹</summary>
        private Entity3DLayer E_Entity3DLayer { get { return Entity3DLayer.BACKGROUND; } }
        //CameraMoveSelfComponent _cameraMoveCpt;
        #endregion


        #region 生命

        private void OnEnable()
        {
            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE, this);

        }


        public void Init(float offsetY, Transform camera)
        {
            _offsetY = offsetY;
            _camera = camera;
            _sr = GetComponent<SpriteRenderer>();
            _curLevel = GameCurLevel();
            _cloud = transform.GetChild(0).GetOrAddComponent<CloudCtrl>();
            _cloud.Init();
            _sr.sortingOrder= (E_Entity3DLayer.Enum2Int()); //设置Y的话 ,camera-10跟枚举冲突
            SetSprite(_curLevel);
            float speed = this.GetModel<IAirCombatAppStateModel>().CameraSpeed;
            //_cameraMoveCpt = gameObject.GetOrAddComponent<CameraMoveSelfComponent>();
            //_cameraMoveCpt.InitComponentEnemy();
            this.GetSystem<ILifeCycleSystem>().Add(LifeName.UPDATE,this);
        }

        private void OnDestroy()
        {
            this.GetSystem<ILifeCycleSystem>().Remove(LifeName.UPDATE, this);
        }

        #endregion


        #region IUpdate
        public int Framing { get; set; }

        public int Frame { get ; }
        public void FrameUpdate()
        {
            if (JugdeUpdate(_offsetY, _camera))
            {
                UpdatePos(_offsetY);
                UpdateSprite();
                UpdateLevel();
            }
        }

        #endregion


        #region pri



        int GameCurLevel ()
        {
            return this.GetModel<IAirCombatAppStateModel>().CurLevel;
        }
        private bool JugdeUpdate(float offset, Transform camera)
        {
            return (camera.position.y - transform.position.y) >= offset;
        }

        private void UpdateLevel()
        {
            bool isActive = (_curLevel != GameCurLevel());
            _cloud.SetActive(isActive);
            if (isActive)
            {
                _curLevel = GameCurLevel();
            }
        }

        private void UpdatePos(float offset)
        {
            transform.SetPosY(transform.position.y + offset * 2);
        }

        private void UpdateSprite()
        {
            SetSprite(GameCurLevel());
        }

        private void SetSprite(int level)
        {
            var pre = ResourcesPath.PICTURE_MAP_FOLDER + Const.MAP_PREFIX;
            var sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(pre + level);
            if (sprite == null)
            {

                Debug.Log("MapItemCtrl 还没有所设关卡数的关卡图片=>"+ level);
                sprite = this.GetUtility<ILoadUtil>().Load<Sprite>(pre + 0);
            }

            _sr.sprite = sprite;
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



