/****************************************************
    文件：AirCombatInspector.cs
	作者：lenovo
    邮箱: 
    日期：2024/6/19 12:46:38
	功能：
*****************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
namespace  QFramework.AirCombat
{
    public class AirCombatInspector : MonoBehaviour   ,IController
    {
        #region 属性

        [SerializeField] float _timing = 0f;
        [SerializeField] IAirCombatAppModel _model; 
        [SerializeField] IAirCombatAppStateModel _stateModel; 





        [Header("Plane")]
        [SerializeField] int SelectPlaneID;
        [SerializeField] int SelectPlaneLevel;
        /// <summary>子弹样式</summary>
        [SerializeField] int PlaneBulletLevel;
        [SerializeField] int PlaneSpriteLevelMax;

        [SerializeField] int HpMax;
        [SerializeField] int Hp;
        [SerializeField] int PlayerCollidedByPlaneCnt;
        [SerializeField] int PlayerCollidedByBulletCnt;


        [Header("Hero")]
        /// <summary>实际使用的是在父节点中的索引</summary>
        [SerializeField] int SelectHeroID;


        [Header("Item")]
        [SerializeField] int Star;
        [SerializeField] int Score;
        [SerializeField] int Diamond;
        [SerializeField] int Shield;
        [SerializeField] int Power;

        [Header("Game")]
        [SerializeField] GameState E_GameState;


        [Header("Level")]
        [SerializeField] LevelState E_LevelState;
        /// <summary>实际使用的是在父节点中的索引</summary>
        [SerializeField] int SelectedLevel;
        [SerializeField] int CurLevel;
        /// <summary>已经通关数.+1就是最大的可以解锁的关卡</summary>
        [SerializeField] int PassedLevel;
        [SerializeField] bool IsFinishOneLevel;
        [SerializeField] int PlaneBulletLevelMax;

        #endregion

        #region 生命



        /// <summary>首次载入且Go激活</summary>
        void Start()
        {
            _timing = 0f;
            _model = this.GetModel<IAirCombatAppModel>();
            _stateModel = this.GetModel<IAirCombatAppStateModel>();
        }

         /// <summary>固定更新</summary>
        void FixedUpdate()
        {
            _timing = this.Timer(_timing, 1f,(System.Action)(()=>
            {
                IsFinishOneLevel = _stateModel.IsFinishOneLevel;
                SelectedLevel = _stateModel.SelectedLevel;
                CurLevel = _stateModel.CurLevel;
                //
                E_GameState = _stateModel.E_GameState;
                E_LevelState = _stateModel.E_LevelState;
                //
                SelectPlaneID = _stateModel.SelectedPlaneID;
                SelectPlaneLevel = _stateModel.SelectedPlaneSpriteLevel;
                SelectHeroID = _stateModel.SelectHeroID;
                PlaneSpriteLevelMax = _model.SelectedPlaneSpriteLevelMax;
                PlaneBulletLevelMax= _model.PlaneBulletLevelMax;
                PlaneBulletLevel = _stateModel.PlaneBulletLevel;
                HpMax = _model.LifeMax;
                Hp = _model.Life;
                PlayerCollidedByPlaneCnt = STest.PlayerCollidedByPlaneCnt;
                PlayerCollidedByBulletCnt = STest.PlayerCollidedByBulletCnt;
                //
                PassedLevel = _model.PassedLevel;
                Star = _model.Star;
                Score = _model.Score;
                Diamond = _model.Diamond;
                Shield = _model.ShieldCount;
                Power = _model.BombCount;  //随便了,Powe Bomb 同一种变量的命名
            })); 
        }

        
        public IArchitecture GetArchitecture()
        {
            return AirCombatApp.Interface;
        }
        #endregion
    }
}



