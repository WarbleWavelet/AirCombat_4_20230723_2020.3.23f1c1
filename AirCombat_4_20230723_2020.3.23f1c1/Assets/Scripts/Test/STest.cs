/****************************************************
    文件：STest.cs
	作者：lenovo
    邮箱: 
    日期：2024/5/26 16:36:50
	功能：
*****************************************************/

using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
 
public static partial class STest  
{
    public static void TestScalePos(this EnemyLifeComponent enemyLifeComponent)
    {
        enemyLifeComponent.gameObject.GetOrAddComponent<PRSInspector>();
    }


    /// <summary>打印Item,看看Logic有没有在跑</summary>
    public static void LogItemName(string spritePath)
    {
                                           
        Debug.Log(GameObjectName.Item+"--"+ spritePath.TrimName(TrimNameType.SlashAfter).ToString());
    }
    public static void LogArr(Vector3[] posArr)
    {
        posArr.Log();
    }


    public static void Log_CanShoot()
    {
        //Debug.Log("MsgEvent.EVENT_CANSHOOT");
    }
    public static void NotPlayerBullet(BulletType bulletType)
    {
        if (bulletType != BulletType.PLAYER)
        {
           // Debug.Log("STest.NotPlayerBullet");
        }
    }

    public static bool IsBossPlane(Transform t)
    {
        if (t.gameObject.name.Contains(EnemyType.BOSS.Enum2String()))
        {
            return true;
        }

        return false;
    }
    public static bool IsBossPlane(BulletType bulletType)
    {
        if (bulletType == BulletType.ENEMY_BOSS_0 || bulletType == BulletType.ENEMY_BOSS_1)
        {
            return true;
        }
        return false;
    }
    public static bool IsBossPlane(EnemyType enemyType)
    {
        if (enemyType == EnemyType.BOSS )
        {
            return true;
        }
        return false;
    }

    public static void Injure(IDespawnCase _destroyCase,IBullet otherBullet, Transform _other, InvincibleComponent invincibleComponent = null)
    {
        _destroyCase.DoIfNotNull(() =>
        {
            if (invincibleComponent != null)
            {
                //_destroyCase.Injure(-otherBullet.GetAttack());
                _destroyCase.Injure(-1);
                invincibleComponent.Invincible = true;
            }
            else 
            {
                _destroyCase.Injure(-otherBullet.GetAttack());

            }
        });

    }




    public static void IsBossPlane(BulletModelComponent bulletModelComponent)
    {
       
            if (bulletModelComponent.GetBulletMode().E_BulletType == BulletType.ENEMY_BOSS_0
        || bulletModelComponent.GetBulletMode().E_BulletType == BulletType.ENEMY_BOSS_1)
        {
            Debug.Log("IsBossPlane");
        }
    }

    internal static void LogSwitchPlane(int id, int change)
    {
        string str = (change == -1) ? "左" : "右";
        (str + "键：" + id).LogInfo();
    }

    public  static int PlayerCollidedByPlaneCnt = 0;
  public  static int PlayerCollidedByBulletCnt = 0;

}



