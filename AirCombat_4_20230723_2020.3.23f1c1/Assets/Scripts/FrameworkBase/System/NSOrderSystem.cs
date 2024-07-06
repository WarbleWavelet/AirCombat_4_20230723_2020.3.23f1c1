/****************************************************
    文件：SCOrderSystem.cs
	作者：lenovo
    邮箱: 
    日期：2024/7/1 13:9:14
	功能：发现很多单例,System,实例类等的依赖顺序,导致空指针
        所以做这个用event来控制
        即使不优雅,也算个汇总,方便以后改
//
        SIOrderSystem的SI,  Singleton+Instance,单例类常用的两个属性名
        ST,SC,static class,择用
*****************************************************/

using QFramework;
using QFramework.AirCombat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;




public class NSOrderSystem : NormalSingleton<NSOrderSystem> ,ICanRegisterEvent
{
    public bool GameDataMgrInited ;


    public void Init()
    {
        GameDataMgrInited = false;
        this.RegisterEvent<GameDataMgrInitedEvent>(_=> GameDataMgrInited = true) ;
    }


    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}



