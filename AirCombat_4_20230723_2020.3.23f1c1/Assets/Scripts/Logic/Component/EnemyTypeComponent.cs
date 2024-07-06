using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeComponent : MonoBehaviour
{

	public EnemyType Type { get; private set; }


    public EnemyTypeComponent Init(EnemyType type)
    {
        Type = type;
        return this;
    }
}
