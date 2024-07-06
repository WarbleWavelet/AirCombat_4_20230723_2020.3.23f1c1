using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.PointGame
{
    public class GameStartPanel : MonoBehaviour,IController
    {
        private IGameModel mGameModel;

        void Start()
        {
            transform.Find("BtnStart").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    gameObject.Hide();
                    
                    this.SendCommand<StartGameCommand>();
                });
            
            transform.Find("BtnBuyLife").GetComponent<Button>()
                .onClick.AddListener(() =>
                {
                    this.SendCommand<BuyLifeCommand>();
                });
            
            mGameModel = this.GetModel<IGameModel>();

            mGameModel.Gold.Register(OnGoldValueChanged);
            mGameModel.Life.Register(OnLifeValueChanged);
          
            // 第一次需要调用一下
            OnGoldValueChanged(mGameModel.Gold.Value);
            OnLifeValueChanged(mGameModel.Life.Value);
            OnBestScoreValueChanged(mGameModel.BestScore.Value);
        }



        #region 辅助
        private void OnBestScoreValueChanged(int bestScores)
        { 
            transform
                .Find("BestScoreText")
                .SetText("最高分:" + bestScores);
        }
        private void OnLifeValueChanged(int life)
        {
            transform
                .Find("LifeText")
                .SetText("生命：" + life);
        }

        private void OnGoldValueChanged(int gold)
        {
            if (gold > 0)
            {
                transform.Find("BtnBuyLife").Show();
            }
            else
            {
                transform.Find("BtnBuyLife").Hide();
            }
          
            transform.Find("GoldText").SetText("金币：" + gold);
        }
        #endregion

        
        

        private void OnDestroy()
        {
            mGameModel.Gold.UnRegister(OnGoldValueChanged);
            mGameModel.Life.UnRegister(OnLifeValueChanged);
            mGameModel = null;
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}
