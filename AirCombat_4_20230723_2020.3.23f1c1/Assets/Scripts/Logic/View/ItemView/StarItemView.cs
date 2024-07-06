using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[BindPrefab(ResourcesPath.PREFAB_START_VIEW, Const.BIND_PREFAB_PRIORITY_VIEW)]
public class StarItemView : ItemInPlaneLevelViewBase, QFramework.IController   ,IInitParas
{
    public override EItemType E_ItemType { get { return EItemType.STAR; } }
   public  void InitParas(params object[] os)
    {
        transform.position = (Vector3)os[0];
    }
    #region protected override

    protected override IEffectContainer GetEffectContainer()
    {
        return SEffectContainerFactory.New(E_ItemType);
    }
                        
    protected override string GetGameAudio()
    {
        return AudioGame.Get_Gold;
        
    }
    protected override string SpritePath()
    {
        return ResourcesPath.PICTURE_STAR;
    }
    protected override void ItemLogic()
    {                                         
         //base.ItemLogic();
        this.GetModel<IAirCombatAppModel>().Star.Value++;
        this.GetModel<IAirCombatAppModel>().Score.Value++;
        this.GetSystem<IGameObjectPoolSystem>().Despawn(gameObject,ResourcesPath.PREFAB_ITEM_ITEM);//父节点的AutoDespawn会执行
       // STest.LogItemName(SpritePath());//没错,暂时注释
    }
    #endregion


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }

    #endregion
}