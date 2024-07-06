using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent  
{
    
}


/// <summary>用不了，说不清是MSU,CQ</summary>
public abstract class QFUtilityComponentBase :  IComponent  ,ICanGetUtility
{

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}


/// <summary>
/// 不能多个基类，所以 : MonoBehaviour谢在这里
/// </summary>
public abstract class QFCommandMonohaviourBase : MonoBehaviour, ICanSendCommand
{

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}
