using System.Collections.Generic;
using Fusion;
using Pong.Game;
using UnityEngine;
using Assert = UnityEngine.Assertions.Assert;

namespace Pong
{
    public class GameManager : NetworkBehaviour
    {
        public readonly List<Player> Players = new();

        [SerializeField] private Paddle _paddleLeft;
        [SerializeField] private Paddle _paddleRight;
        [SerializeField] private Ball _ball;
        [SerializeField] private UIManager _uiManager;
        
        private static GameManager _instance;

        public static GameManager Instance
        {
            get => _instance;
            private set => _instance = value;
        }

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public void Start()
        {
            Goal.OnGoalScored.AddListener(HandleGoalScored);
        }

        public void SetPlayer(Player player)
        {
            Players.Add(player);
            player.OnScoreChanged.AddListener(HandleScoreChanged);

            if (Players.Count == 1)
            {
                _uiManager.UpdateStatusLabel("Waiting for player 2");
            }

            if (Players.Count == 2)
            {
                StartGame();
            }
        }

        private void StartGame()
        {
            Assert.AreEqual(2, Players.Count);

            Players[0].InitPlayer(PlayerPosition.Left);
            Players[1].InitPlayer(PlayerPosition.Right);
                
            _uiManager.HideNewGameOverlay();
            _ball.ResetBall();
        }

        private Player GetPlayerByPosition(PlayerPosition playerPosition)
        {
            Assert.AreEqual(2, Players.Count);
            
            return Players[0].PlayerPosition == playerPosition ? Players[0] : Players[1];
        }

        private Player GetOtherPlayer(Player player)
        {
            Assert.AreEqual(2, Players.Count);
            
            return Players[0] == player ? Players[1] : Players[0];
        }
        
        public Paddle GetPaddle(PlayerPosition playerPosition)
        {
            return playerPosition == PlayerPosition.Left ? _paddleLeft : _paddleRight;
        }

        private void HandleGoalScored(PlayerPosition playerPosition)
        {
            var player = GetOtherPlayer(GetPlayerByPosition(playerPosition));
            player.IncreaseScore();
            _ball.ResetBall();
        }

        private void HandleScoreChanged()
        {
            var playerLeft = GetPlayerByPosition(PlayerPosition.Left);
            var playerRight = GetPlayerByPosition(PlayerPosition.Right);
            
            _uiManager.UpdateScore(playerLeft.Score, playerRight.Score);

            if (playerLeft.Score >= 5)
            {
                EndGame(PlayerPosition.Left);
            }
            
            if (playerRight.Score >= 5)
            {
                EndGame(PlayerPosition.Right);
            }
        }

        public void EndGame(PlayerPosition winner)
        {
            Runner.Despawn(_ball.Object);

            _uiManager.ShowGameOverOverlay();
            _uiManager.UpdateWinnerLabel(winner.ToString());
        }
        
        protected virtual void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
        }
    }
}