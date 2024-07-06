using QFramework.AirCombat;
using QFramework;
using UnityEngine;

public class DespawnBulletComponent : MonoBehaviour, IDespawnCase ,ICanGetSystem   ,ICanSendCommand
{
    Transform _despawnTrans;
    string _despawnKey;

    public DespawnBulletComponent Init(Transform despawnTrans, string despawnKey)
    {
        _despawnTrans = despawnTrans;
        _despawnKey  =  despawnKey;
        return this;
    }


    #region IDespawnCase
    /// <summary>伤害</summary>
    public void Injure(int value)
    {
        Despawn();
    }

    public void Dead()
    {
        Despawn();                                           
    }
    #endregion

                                  
    #region pri
    private void Despawn()
    {
        this.GetSystem<IAniSystem>().OnBulletDestroyAni(new BulletDestroyAniEvent { pos= _despawnTrans.position});
        this.SendCommand( new DespawnABulletCpmmand(_despawnTrans.gameObject,_despawnKey));
    }
    #endregion


    #region QF
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}