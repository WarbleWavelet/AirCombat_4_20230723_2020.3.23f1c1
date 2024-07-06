using QFramework.AirCombat;

namespace QFramework.PointGame
{
    public interface IGameModel : IModel
    {
        BindableProperty<int> KillCount { get; }

        BindableProperty<int> Gold { get; }

        BindableProperty<int> Score { get; }

        BindableProperty<int> BestScore { get; }
        
        BindableProperty<int> Life { get; } 

    }

    public class GameModel : AbstractModel, IGameModel
    {
        public BindableProperty<int> KillCount { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public BindableProperty<int> Gold { get; } = new BindableProperty<int>()
        {
            Value = 0
        };

        public BindableProperty<int> Score { get; } = new BindableProperty<int>()
        { 
            Value = 0 
        };

        public BindableProperty<int> BestScore { get; } = new BindableProperty<int>()
        { 
            Value = 0
        };

        public BindableProperty<int> Life { get; }= new BindableProperty<int>();

        protected override void OnInit()
        {
            IStorageUtil storage = this.GetUtility<IStorageUtil>();

            BestScore.Value = storage.Get<int>(nameof(BestScore));
            BestScore.Register(v => storage.Set(nameof(BestScore), v));
            
            Life.Value = storage.Get<int>(nameof(Life)); 
            Life.Register(v => storage.Set(nameof(Life), v)); 
          
            Gold.Value = storage.Get<int>(nameof(Gold)); 
            Gold.Register((v) => storage.Set(nameof(Gold), v)); 
        }
    }
}