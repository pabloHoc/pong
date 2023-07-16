using Fusion;
using UnityEngine.Events;

namespace Pong.Game
{
    public enum PlayerPosition
    {
        Left,
        Right
    }
    
    public class Player : NetworkBehaviour
    {
        public UnityEvent OnScoreChanged = new();
        
        [Networked(OnChanged = nameof(OnPlayerScoreChanged))] 
        public int Score { get; set; }

        public PlayerPosition PlayerPosition { get; private set; }
        
        public void InitPlayer(PlayerPosition playerPosition)
        {
            if (!Object.HasStateAuthority) return;

            PlayerPosition = playerPosition;
            
            var paddle = GameManager.Instance.GetPaddle(PlayerPosition);
            paddle.Object.AssignInputAuthority(Object.InputAuthority);
        }

        public static void OnPlayerScoreChanged(Changed<Player> changed)
        {
            changed.Behaviour.OnScoreChanged.Invoke();
        }
        
        public override void Spawned()
        {
            GameManager.Instance.SetPlayer(this);
        }

        public void IncreaseScore()
        {
            Score++;
        }
    }
}