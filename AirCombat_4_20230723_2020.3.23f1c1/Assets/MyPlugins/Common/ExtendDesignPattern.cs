/****************************************************
    文件：ExtendDesignPattern.cs
	作者：lenovo
    邮箱: 
    日期：2024/6/26 11:59:40
	功能： 设计模式
*****************************************************/

using Codice.CM.Client.Differences;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static partial class ExtendDesignPattern
{ 

}
public static partial class ExtendDesignPattern//适配器 ,把自己交给适配器.翻译器
{
    #region 示例
    class Test
    {
        void Main()
        {     
            NormalPeople normal=new NormalPeople();
            PeopleCantHear peopleCantHear = new PeopleCantHear();
            Voice2WordApp app=  new Voice2WordApp(peopleCantHear);
            Voice voice0;
            Voice voice1;
            //
            voice0 = normal.Speak();
            voice1 = app.Speak(voice0);
            voice0 = normal.Speak(voice1);
            voice1 = app.Speak(voice0);
            voice0 = normal.Speak(voice1);
            voice1 = app.Speak(voice0);
            voice0 = normal.Speak(voice1);
            voice1 = app.Speak(voice0);
            voice0 = normal.Speak(voice1);
            voice1 = app.Speak(voice0);
            voice0 = normal.Speak(voice1);

            normal.Speak();
            //现在两人都可以Speak  ,解决了PeopleCantHear的耳聋残疾带来的沟通问题
        }

        void Main1()
        {
            NormalPeople normal = new NormalPeople();
            PeopleCantSee peopleCantSee   = new PeopleCantSee();
            Word2VoiceApp app = new Word2VoiceApp(peopleCantSee);
            Word word0;
            Voice voice1;
            //
            word0 = normal.Write();
            voice1 = app.Speak(word0);
            word0 = normal.Write(voice1);
            voice1 = app.Speak(word0);
            word0 = normal.Write(voice1);
            voice1 = app.Speak(word0);
            word0 = normal.Write(voice1);
            voice1 = app.Speak(word0);
            word0 = normal.Write(voice1);
            voice1 = app.Speak(word0);
            word0 = normal.Write(voice1);
            voice1 = app.Speak(word0);
            //现在两人都可以Speak  ,解决了PeopleCantHear的耳聋残疾带来的沟通问题
        }
    }

    #endregion


    #region 沟通单位
    class Voice
    { 
    
    }

    class Word
    {
        Voice _voice;

        public Word(Voice voice)
        {
            _voice = voice ?? throw new ArgumentNullException(nameof(voice));
        }
    }
    #endregion


    #region 能力interface
    interface ICanSpeak
    {
        /// <summary>开头发起说话</summary>
        Voice Speak();
    }
    interface ICanWrite
    {
        /// <summary>开头发起说话</summary>
        Word Write();
    }
    interface ICanWriteAfterVoice
    {
        /// <summary>听到别人的声音后书写</summary>
        Word Write(Voice voice);
    }

    interface ICanSpeakAfterVoice
    {
        /// <summary>听到别人的声音,说话</summary>
        Voice Speak(Voice voice);
    }
    interface ICanSpeakAfterWord
    { 
        /// <summary>看到别人的文字后,说话</summary>
        Voice Speak(Word word);    
    }
    #endregion


    #region 不同能力的人
    class NormalPeople : ICanSpeak,ICanWrite, ICanSpeakAfterVoice, ICanSpeakAfterWord ,ICanWriteAfterVoice
    {
        public Word Write()
        {
            throw new NotImplementedException();
        }

        public Voice Speak(Voice voice)
        {
            throw new NotImplementedException();
        }

        public Voice Speak()
        {
            throw new NotImplementedException();
        }

        public Voice Speak(Word word)
        {
            throw new NotImplementedException();
        }

        public Word Write(Voice voice)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>眼瞎的人</summary>
    class PeopleCantSee : ICanSpeak, ICanSpeakAfterVoice
    {
        public Voice Speak()
        {
            throw new NotImplementedException();
        }

        public Voice Speak(Voice voice)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>耳聋的人</summary>
    class PeopleCantHear : ICanSpeak, ICanSpeakAfterWord
    {
        public Voice Speak(Word word)
        {
            throw new NotImplementedException();
        }

        public Voice Speak()
        {
            throw new NotImplementedException();
        }
    }
    #endregion


    /// <summary>适配器</summary>
    interface IAdaptor
    {

    }


    #region 耳聋

    /// <summary>语音转文字的功能</summary>
    interface IVoice2WordFunction
    {
        Word  Voice2Word(Voice voice);
    }


    class Voice2WordApp : IAdaptor, IVoice2WordFunction,ICanSpeakAfterVoice
    {
        PeopleCantHear _peopleCantHear;
        Voice _voice;
        Word _word;

        public Voice2WordApp(PeopleCantHear peopleCantHear)
        {
            _peopleCantHear = peopleCantHear;
        }

        public Voice Speak(Voice voice)
        {
            _voice = voice;
            _word = Voice2Word(_voice);
            return _peopleCantHear.Speak(_word);
      }

        public Word Voice2Word(Voice voice)
        {
            throw new NotImplementedException();
        }
    }


    #endregion

    #region 眼瞎
    /// <summary>语音转文字的功能</summary>
    interface IWord2VoiceFunction
    {
        Voice Word2Voice(Word word);
    }


    /// <summary>眼瞎辅助软件</summary>
    class Word2VoiceApp :IAdaptor, IWord2VoiceFunction, ICanSpeakAfterWord
    {
        PeopleCantSee _peopleCantSee;
        Voice _voice;
        Word _word;

        public Word2VoiceApp(PeopleCantSee peopleCantSee)
        {
            _peopleCantSee = peopleCantSee;
        }

        public Voice Speak(Word word)
        {
            _word = word;
            _voice = Word2Voice(_word);
            return  _peopleCantSee.Speak(_voice);
        }

        public Voice Word2Voice(Word word)
        {
            throw new NotImplementedException();
        }
    }
    #endregion



}



