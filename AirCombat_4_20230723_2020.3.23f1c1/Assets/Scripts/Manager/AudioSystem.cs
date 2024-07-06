using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using QFramework;
using QFramework.AirCombat;
using static LevelRoot;

public interface IAudioSystem : QFramework.ISystem
{
    void PasueAllAudio(object[] paras);
    void ContinueAllAudio(object[] paras);
    /// <summary>目前跟PlaySound区别，它只有一个</summary>
    void PlayMusic(string name);
    void PauseMusic( );
    void ResumeMusic( );
    void PlayVoice(string name, bool loop = false);
    void PauseVoice( );
    void StopVoice( );
    void ResumeVoice( );
    void PlaySound(string name);
    void PlaySound(string name, bool loop = false);
    void PlaySound(IBulletModel model, bool loop = false);
    void PlaySoundLoop(string name);
    void Stop(string name);
    void PauseDelay(string name);
    void ReplaySound(string name, bool loop = true);
}
public class AudioSystem :  QFramework.AbstractSystem   ,IAudioSystem
{


    #region 字属

    IAudioLoader loader;
    private Action _changeVolume;
    private readonly float _defaultVolume = 0.5f;


    private readonly List<AudioSource> _activeSourceLst = new List<AudioSource>();
    private readonly List<AudioSource> _inactiveSourceLst = new List<AudioSource>();


    private readonly Dictionary<string, float> _volumeDic = new Dictionary<string, float>();
    private readonly Dictionary<string, AudioClip> _clipDic = new Dictionary<string, AudioClip>();
    private readonly Dictionary<string, AudioSource> _clipAndSourceDic = new Dictionary<string, AudioSource>();
      const string _path = "resources://";
      const string _voicePath = "resources://Audio/Player/";

    #endregion

    protected override void OnInit()
    {
        //Transform t = Camera.main._colliderTrans.FindTopOrNew(GameObjectName.System)._colliderTrans;
        //Transform t1 = t.FindOrNew(GameObjectName.AudioSystem);
        loader = AudioKit.Config.AudioLoaderPool.AllocateLoader();
        _changeVolume = null;

        InitClips();
        InitVolume();
        InitListener();
    }

    #region pub 实现  


    public void PasueAllAudio(object[] paras=null)
    {
        AudioKit.PauseAllAudio();
    }


    public void ContinueAllAudio(object[] paras=null)
    {
        AudioKit.ResumeAllAudio(); 
    }


    #region Music
    public void PlayMusic(string name)
    {
        AudioKit.PlayMusic($"{_path}{name}");
    }
    public void PauseMusic( )
    {
        AudioKit.PauseMusic();
    }

    public void ResumeMusic()
    {
        AudioKit.ResumeMusic();
    }
    #endregion



    #region Voice
    public void PlayVoice(string name, bool loop = false)
    {
        AudioKit.PlayVoice($"{_voicePath}{name}");  //因为直接用枚举英雄类型，所以加前缀
    }
    public void PauseVoice()
    {
        AudioKit.PauseVoice();
    }

    public void ResumeVoice()
    {
        AudioKit.ResumeVoice();
    }


    public void StopVoice()
    {
        AudioKit.StopVoice();
    }
    #endregion



    #region Sound
    public void PlaySound(string name)
    {
        if (name == AudioGame.GameAudioNull)
        {
            return;
        }
        AudioKit.PlaySound($"{_path}{name}");
    }


    public void PlaySound(string name, bool loop = false)
    {
        if(name==AudioGame.GameAudioNull)
        {
            return;
        }
        AudioKit.PlaySound($"{_path}{name}", loop);    
    }

    public void PlaySoundLoop(string name)
    {
        if (name == AudioGame.GameAudioNull)
        {
            return;
        }
        AudioKit.PlaySound($"{_path}{name}", true);
    }

    public void PlaySound(IBulletModel model, bool loop = false)
    {
        var audioName = model.AudioName;
        PlaySound(audioName, false);
    }
    #endregion


    public void Stop(string name)
    {
        if (_clipAndSourceDic.ContainsKey(name))
        {
            var source = _clipAndSourceDic[name];
            source.Stop();
            source.clip = null;
            _activeSourceLst.Remove(source);
            _inactiveSourceLst.Add(source);
            _clipAndSourceDic.Remove(name);
        }
    }



    public void PauseDelay(string name)
    {
        if (_clipAndSourceDic.ContainsKey(name))
        {
            var source = _clipAndSourceDic[name];
            source.loop = false;
        }
    }

    public void ReplaySound(string name, bool loop = true)
    {
        PlaySound(name, loop);
    }
    #endregion



    #region pri


    private void InitClips()
    {
        AudioClip[] clips = this.GetUtility<ILoadUtil>().LoadAll<AudioClip>(ResourcesPath.AUDIO_FOLDER);
        foreach (AudioClip clip in clips)
        {
            _clipDic.Add(clip.name, clip);
        }
    }

    private void InitVolume()
    {
        var reader = this.GetUtility<IReaderUtil>().GetReader(ResourcesPath.CONFIG_AUDIO_VOLUME_CONFIG);
        var name = "";
        float volume = 0;
        reader.Count(count =>
        {
            for (var i = 0; i < count; i++)
            {
                TaskQueueMgr.Single.AddQueue<string>(() => reader[i][DataKeys.AUDIO_NAME]);
                TaskQueueMgr.Single.AddQueue<float>(() => reader[i][DataKeys.AUDIO_Volume]);
                TaskQueueMgr.Single.Execute(datas => { _volumeDic.Add((string)datas[0], (float)datas[1]); });
            }

            if (_changeVolume != null)
            { 
               _changeVolume();
            }
                
            _changeVolume = null;
        });
    }

    private void InitListener()
    {
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Register(state=>
        {
            if (state == GameState.PAUSE)
            {
                PasueAllAudio();
            }
        });
        this.GetModel<IAirCombatAppStateModel>().E_GameState.Register(state=>
        {
            if (state == GameState.CONTINUE)
            {
                ContinueAllAudio();
            }
        });
    }



    #endregion





    public IArchitecture GetArchitecture()
    {
        return AirCombatApp.Interface;
    }


}