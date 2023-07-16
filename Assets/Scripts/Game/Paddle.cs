using Fusion;
using UnityEngine;

namespace Pong.Game
{
    public class Paddle : NetworkBehaviour
    {
        private const float Speed = 10f;
        private const float BoundY = 3.25f;
        private Rigidbody2D _rigidBody;
    
        private void Awake()
        {
            _rigidBody ??= GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            if (!GetInput(out NetworkInputData data)) return;

            _rigidBody.position += Vector2.up * data.Direction * Speed * Runner.DeltaTime;

            if (_rigidBody.position.y > BoundY)
            {
                _rigidBody.position = new Vector2(_rigidBody.position.x, BoundY);
            }
            
            if (_rigidBody.position.y < -BoundY)
            {
                _rigidBody.position = new Vector2(_rigidBody.position.x, -BoundY);
            }
        }
    }
}