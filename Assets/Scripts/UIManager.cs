using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace Pong
{
    public class UIManager : MonoBehaviour
    {
        public UIDocument UIDocument;
        
        public static readonly UnityEvent OnHostBtnClicked = new();
        public static readonly UnityEvent OnClientBtnClicked = new ();

        private VisualElement _root;
        private VisualElement _newGameOverlay;
        private VisualElement _gameOverOverlay;

        private Label _statusLabel;
        private Label _scoreLabel;
        private Label _winnerLabel;

        void Start()
        {
            _root = UIDocument.rootVisualElement;
            
            _newGameOverlay = _root.Q<VisualElement>("NewGameOverlay");
            _gameOverOverlay = _root.Q<VisualElement>("GameOverOverlay");

            _statusLabel = _root.Q<Label>("StatusLabel");
            _scoreLabel = _root.Q<Label>("ScoreLabel");
            _winnerLabel = _root.Q<Label>("WinnerLabel");

            var hostBtn = _root.Q<Button>("HostButton");
            var clientBtn = _root.Q<Button>("ClientButton");

            hostBtn.clicked += OnHostBtnClicked.Invoke;
            clientBtn.clicked += OnClientBtnClicked.Invoke;
        }

        public void HideNewGameOverlay()
        {
            _newGameOverlay.visible = false;
        }

        public void UpdateStatusLabel(string text)
        {
            _statusLabel.text = text;
        }

        public void UpdateScore(int scoreLeft, int scoreRight)
        {
            _scoreLabel.text = $"{scoreLeft} - {scoreRight}";
        }
        
        public void ShowGameOverOverlay()
        {
            _gameOverOverlay.visible = true;
        }
        
        public void UpdateWinnerLabel(string winner)
        {
            _winnerLabel.text = $"{winner} player won!";
        }
    }
}
