/****************************************************
    文件：BulletModelComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/7 14:16:28
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;


/// <summary>内容是BulletModel来装进Ibullet</summary>
public class BulletModelComponent : MonoBehaviour, IBullet, ICanGetUtility
{

    [SerializeField] private IBulletModel _bulletModel;


    public IBulletModel GetBulletMode()
    {
        return _bulletModel;
    }


    #region 生命
    BulletModelComponent Init(IBulletModel bulletModel)
    { 
        if (bulletModel == null)
        {
            Debug.LogErrorFormat("ShootCtrl的BulletModel为空");
            return null;
        }
        _bulletModel= bulletModel;  
        return this;    
    }


    /// <summary></summary>
    public BulletModelComponent Init(BulletType bulletType,EnemyData enemyData=null)
    {
        IBulletModel bulletModel = this.GetUtility<IBulletModelUtil>().GetBulletModel(bulletType);
        return Init(bulletModel,enemyData);
    }


    public BulletModelComponent Init(IBulletModel bulletModel, EnemyData enemyData = null)
    {
        Init(bulletModel);
        if (enemyData != null)
        {
            IEnemyBulletModel enemyBulletModel = _bulletModel as IEnemyBulletModel;
            enemyBulletModel.Init(enemyData);
        }
        return this;
    }
    #endregion


    #region IBullet
    public BulletOwner Owner { get { return  _bulletModel.Owner;} }
    public BulletOwner[] ShootOwners => _bulletModel.ShootOwners;
    public int GetAttack()
    {
        return _bulletModel.GetAttack();
    }

    public bool ContainsDeadFrom(string toTag)
    {
        return _bulletModel.ContainsDeadFrom(toTag);
    }
    public bool ContainsDamageTo(BulletOwner owner)
    {
        return _bulletModel.ContainsDamageTo(owner);
    }
    public HashSet<string> GetShootTags()
    {
        return _bulletModel.GetShootTags();
    }

    #endregion
  
    

    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}




