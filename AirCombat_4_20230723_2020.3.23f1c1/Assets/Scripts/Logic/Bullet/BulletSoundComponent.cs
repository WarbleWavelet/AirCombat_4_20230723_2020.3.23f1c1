/****************************************************
    文件：BulletSoundComponent.cs
	作者：lenovo
    邮箱: 
    日期：2024/4/7 14:17:7
	功能：
*****************************************************/

using QFramework.AirCombat;
using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BulletSoundComponent : MonoBehaviour, QFramework.IController
{
    [SerializeField] private int _fireSoundPara;  //子弹容量内音效系数。%2=0响一半
    public int FireSoundPara { get => _fireSoundPara; set => _fireSoundPara = value; }



    public void PlaySound(int bulletCnting, int bulletCnt)
    {
        this.SendCommand(new PlaySoundFireCommand(bulletCnting, bulletCnt, _fireSoundPara));
    }


    #region over
    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion
}




