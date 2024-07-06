using UnityEngine;

/// <summary>BulletType.POWER</summary>

public class BulletPowerComponent : MonoBehaviour ,ISetActiveWhenSpawn 
{

    private void OnEnable()
    {
        gameObject.GetOrAddComponent<ShootCtrl>().InitComponentPower(BulletType.POWER);
        gameObject.GetOrAddComponent<DelayDestroy>().Delay(10);
            
    }

}