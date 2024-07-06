using System;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.PointGame
{
    public class GamePassPanel : MonoBehaviour, IController
    {
        private void Start()
        {
            transform.Find("RemainSecondsText").SetText(
                "剩余时间:" + this.GetSystem<ICountDownSystem>().CurrentRemainSeconds + "s");

            var gameModel = this.GetModel<IGameModel>();

            transform.Find("BestScoreText").SetText( "最高分数:" + gameModel.BestScore.Value);

            transform.Find("ScoreText").SetText( "分数:" + gameModel.Score.Value);
        }
      

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}