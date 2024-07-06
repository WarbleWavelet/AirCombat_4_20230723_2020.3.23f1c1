using QFramework;
using System;
using UnityEngine;


/// <summary>
/// 护盾
/// <br/>碰撞，演示销毁
/// </summary>
public class ShieldComponent : MonoBehaviour, ISetActiveWhenSpawn ,IInitParent
{
    public void InitParent(Transform parent)
    {
        transform.Position(parent.position)
            .Parent(parent);
    }

    private void OnEnable()
    {
      gameObject.GetOrAddComponent<Trigger2DComponent>();
      gameObject.GetOrAddComponent<DelayDestroy>().Delay(Const.SHIELD_TIME);            
    }


}