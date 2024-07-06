using QFramework;
using QFramework.AirCombat;
using UnityEngine;

public class PlayerDestroyComponent:MonoBehaviour, IDespawnCase, QFramework.IController ,IInitEntity<PlayerDestroyComponent>
{
    Transform _planeTrans;

    public PlayerDestroyComponent InitEntity(Transform entity)
    {
        _planeTrans = entity;
        return this;
    }
    /// <summary>伤害</summary>
    public void Injure(int change)
    {
        int cur = this.GetModel<IAirCombatAppModel>().Life.Value;
        this.GetModel<IAirCombatAppModel>().Life.Value = this.Injure(change, cur,null, Dead);
    }

    public void Dead()
    {
        this.SendCommand(new DeadPlayerCommand(_planeTrans)); 

    }


    #region 重写
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }




    #endregion
}