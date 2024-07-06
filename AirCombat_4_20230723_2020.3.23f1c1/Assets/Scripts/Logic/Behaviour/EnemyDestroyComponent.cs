using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>EnemyData、LifeComponent</summary>
public class EnemyDestroyComponent : MonoBehaviour, IDespawnCase, QFramework.IController 
{

    [SerializeField] private LifeComponent _lifeComponent;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] Transform _parent;
    [SerializeField] EnemyTypeComponent _enemyTypeComponent;

    public EnemyDestroyComponent Init(EnemyData data,LifeComponent lifeComponent,Transform parnet,EnemyTypeComponent enemyTypeComponent)//因为一下的两个没用，所以先直接限定
    {
        _enemyData = data;
        _lifeComponent = lifeComponent;
        _parent = parnet; 
        _enemyTypeComponent = enemyTypeComponent;   
        return this;
    }


    #region pub IDestroyCase
    public void Injure(int change)
    {
        float cur = _lifeComponent.Life ;
        float after = this.Injure(change, cur,null,Dead);
        _lifeComponent.Life = (int)after;

    }



    public void Dead()
    {
        EnemyType enemyType=_enemyTypeComponent.Type;
        this.SendCommand(new DeadEnemyCommand(_parent,enemyType,_enemyData));


    }
    #endregion  


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


    #endregion
}