using UnityEngine;
using UnityEngine.Events;

namespace Pong.Game
{
    public class Goal : MonoBehaviour
    {
        public static UnityEvent<PlayerPosition> OnGoalScored = new();

        private Collider2D _collider;
        [SerializeField] private PlayerPosition _playerPosition;
        
        private void Awake()
        {
            _collider ??= GetComponent<Collider2D>();
        }

        // Update is called once per frame
        private void OnTriggerEnter2D(Collider2D other)
        {
            var ball = other.GetComponent<Ball>();
            if (!ball) return;

            OnGoalScored.Invoke(_playerPosition);
        }
    }    
}

