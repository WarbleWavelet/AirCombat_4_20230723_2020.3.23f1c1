using QFramework;
using QFramework.AirCombat;
using System;
using UnityEngine;
using UnityEngine.UI;

public interface IButtonUtil : QFramework.IUtility
{



    void ButtonAction_ClickAudio(  Transform trans, string path, Action action, bool useDefaultAudio = true);
}

public class ButtonUtil:  IButtonUtil, ICanGetSystem
{

    /// <summary>
    /// 类似于  this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(_colliderTrans,"InitParas", AddAction);
    /// <para /> 类似于this.GetUtility<IButtonUtil>().ButtonAction_ClickAudio(_colliderTrans,"OK/OnTriggerEnter", () => { this.GetSystem<IUISystem>().Open(Paths.PREFAB_LEVELS_VIEW); });
    /// </summary>
    public void ButtonAction_ClickAudio( Transform trans
        , string path
        , Action action
        , bool useDefaultAudio = true)
    {

        if (useDefaultAudio)
        {
            action += AddButtonAudio;
        }
        trans.ButtonAction(path, action);

    }


    #region pri


    private void AddButtonAudio()
    {
        this.GetSystem<IAudioSystem>().PlaySound(AudioUI.UI_ClickButton.ToString());
    }
    #endregion


    #region 实现



    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }
    #endregion

}