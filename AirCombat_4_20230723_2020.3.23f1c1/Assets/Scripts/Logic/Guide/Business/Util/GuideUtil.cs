using QFramework;
using QFramework.AirCombat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGuideUtil:QFramework.IUtility
{
   Transform GetHighLightTrans(RectTransform rect, Action exit);
   Transform GetHighLightTrans(RectTransform[] rects, Action exit);
   Transform GetHandTrans(RectTransform targetRect, Vector2 posRatio);
}
public class GuideUtil : IGuideUtil,ICanGetSystem
{


    public  Transform GetHighLightTrans(RectTransform rect,Action exit)
    {
        return GetHighLightTrans(new[] {rect}, exit);
    }
    
    public  Transform GetHighLightTrans(RectTransform[] rects,Action exit)
    {
        Transform highLight = this.GetSystem<IGuideUiSystem>().Show(ResourcesPath.PREFAB_HIGH_LIGHT_GUIDE);
        var highLightModule = highLight.GetOrAddComponent<HighLightModule>();
        highLightModule.Init(rects,Camera.main, exit);
        return highLight;
    }

    public  Transform GetHandTrans(RectTransform targetRect,Vector2 posRatio)
    {
        Transform hand = this.GetSystem<IGuideUiSystem>().Show(ResourcesPath.PREFAB_HAND_GUIDE);
        var handModule = hand.GetOrAddComponent<HandModule>();
        handModule.SetHand(targetRect,posRatio);
        return hand;
    }

    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
}
