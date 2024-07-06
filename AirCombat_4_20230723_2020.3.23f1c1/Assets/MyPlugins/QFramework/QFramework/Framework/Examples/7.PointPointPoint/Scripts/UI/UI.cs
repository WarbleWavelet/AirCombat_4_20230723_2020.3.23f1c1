using UnityEngine;
using QFramework;

namespace QFramework.PointGame
{
    public class UI : MonoBehaviour,IController
    {
        void Start()
        {
            this.RegisterEvent<GamePassEvent>(OnGamePass);
            
            this.RegisterEvent<OnCountDownEndEvent>(e =>
            {
                transform.Find("Canvas/GamePanel").Hide();
                transform.Find("Canvas/GameOverPanel").Show();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnGamePass(GamePassEvent e)
        {
            transform.Find("Canvas/GamePanel").Hide();
            transform.Find("Canvas/GamePassPanel").Show();
        }

        void OnDestroy()
        {
            this.UnRegisterEvent<GamePassEvent>(OnGamePass);
        }

        public IArchitecture GetArchitecture()
        {
            return PointGame.Interface;
        }
    }
}