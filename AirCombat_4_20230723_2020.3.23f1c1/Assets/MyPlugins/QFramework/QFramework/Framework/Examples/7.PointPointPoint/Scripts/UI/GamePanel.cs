using UnityEngine;
using UnityEngine.UI;

namespace QFramework.PointGame
{
    public class GamePanel : MonoBehaviour, IController
    {
        private ICountDownSystem mCountDownSystem;
        private IGameModel mGameModel;

        private void Awake()
        {
            mCountDownSystem = this.GetSystem<ICountDownSystem>();
            mGameModel = this.GetModel<IGameModel>();
            //
            mGameModel.Gold.Register(OnGoldValueChanged);
            mGameModel.Life.Register(OnLifeValueChanged);
            mGameModel.Score.Register(OnScoreValueChanged);

            // 第一次需要调用一下
            OnGoldValueChanged(mGameModel.Gold.Value);
            OnLifeValueChanged(mGameModel.Life.Value);
            OnScoreValueChanged(mGameModel.Score.Value);
        }

        private void OnLifeValueChanged(int life)
        {
            transform.Find("LifeText").SetText("生命：" + life)  ;
        }

        private void OnGoldValueChanged(int gold)
        {
            transform.Find("GoldText").SetText("金币：" + gold);
        }

        private void OnScoreValueChanged(int score)
        {
            transform.Find("ScoreText").SetText("分数:" + score);
        }

        private void Update()
        {
            // 每 20 帧 更新一次
            if (Time.frameCount % 20 == 0)
            {
                transform.Find("CountDownText").SetText(
                    mCountDownSystem.CurrentRemainSeconds + "s"
                );

                mCountDownSystem.Update();
            }
        }

        private void OnDestroy()
        {
            mGameModel.Gold.UnRegister(OnGoldValueChanged);
            mGameModel.Life.UnRegister(OnLifeValueChanged);
            mGameModel.Score.UnRegister(OnScoreValueChanged);
            mGameModel = null;
            mCountDownSystem = null;
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}