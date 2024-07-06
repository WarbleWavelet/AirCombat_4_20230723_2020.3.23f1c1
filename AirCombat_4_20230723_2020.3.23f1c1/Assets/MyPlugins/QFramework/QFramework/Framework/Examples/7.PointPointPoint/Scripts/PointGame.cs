using QFramework;
using System;

namespace QFramework.PointGame
{
    class Framework
    {
        void All()
        {
            LiuCheng();
            UI();
            Command();
            Controller();
            Event();
            Model();
            System();
            Utility();
        }

        private void LiuCheng()
        {
            GameStartPanel gameStartPanel;
            StartGameCommand startGameCommand;
            BuyLifeCommand buyLifeCommand;
        }

        private void Controller()
        {
            Enemy enemy;
            ErrorArea errorArea;
            Game game;
        }

        private void Utility()
        {
            IStorage storage;
        }

        private void System()
        {
            IAchievementSystem iAchievementSystem;
            AchievementSystem achievementSystem;
            AchievementItem achievementItem;
            ICountDownSystem iCountDownSystem;
            CountDownSystem countDownSystem;
            IScoreSystem scoreSystem;
            ScoreSystem iScoreSystem;
        }

        private void UI()
        {
            GameStartPanel gameStartPanel;
            //
            GamePanel gamePanel;
            GamePassPanel gamePassPanel;
            //
            UI ui;
            Game game;
        }

        private void Model()
        {
            IGameModel iGameModel;
            GameModel gameModel;
        }

        private void Event()
        {
            GamePassEvent gamePassEvent;
            GameStartEvent gameStartEvent;
            OnCountDownEndEvent onCountDownEndEvent;
            OnEnemyKillEvent onEnemyKillEvent;
            OnMissEvent onMissEvent;
        }

        private void Command()
        {
            BuyLifeCommand buyLifeCommand;
            KillEnemyCommand killEnemyCommand;
            MissCommand missCommand;
            StartGameCommand startGameCommand;
        }
    }
    public class PointGame : Architecture<PointGame>
    {
        protected override void Init()
        {
            RegisterSystem<IScoreSystem>(new ScoreSystem());
            RegisterSystem<ICountDownSystem>(new CountDownSystem());
            RegisterSystem<IAchievementSystem>(new AchievementSystem());

            RegisterModel<IGameModel>(new GameModel());

            RegisterUtility<IStorage>(new PlayerPrefsStorage());
        }
    }
}
