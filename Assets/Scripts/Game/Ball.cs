using System.Collections;
using Fusion;
using UnityEngine;

namespace Pong.Game
{
    public class Ball : NetworkBehaviour
    {
        [SerializeField] private float _ballSpeed = 10f;
        [SerializeField] private int _startSecondsDelay = 1;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidBody;
        
        private void Awake()
        {
            _rigidBody ??= GetComponent<Rigidbody2D>();
        }
        
        public override void FixedUpdateNetwork()
        {
            if (_rigidBody.velocity == Vector2.zero) return;

            var velocityNormalized = _rigidBody.velocity.normalized;

            if (Mathf.Abs(velocityNormalized.x) < 0.25f)
            {
                velocityNormalized.x = 0.25f * velocityNormalized.x < 0 ? -1f : 1f;
            }
            
            _rigidBody.velocity = velocityNormalized * _ballSpeed;
        }

        public void ResetBall()
        {
            _spriteRenderer.enabled = false;
            _rigidBody.velocity = Vector2.zero;
            transform.position = Vector3.zero;
            
            StartCoroutine(StartBall(_startSecondsDelay));
        }

        IEnumerator StartBall(float delaySeconds)
        {
            yield return new WaitForEndOfFrame();
            
            _spriteRenderer.enabled = true;
            
            yield return new WaitForSecondsRealtime(delaySeconds);

            var xVelocity = Random.Range(.1f, .9f);

            if (Random.value > 0.5f)
            {
                xVelocity *= -1f;
            }
    
            var yVelocity = Random.Range(.1f, .9f);
            
            if (Random.value > 0.5f)
            {
                yVelocity *= -1f;
            }
            
            var initialVelocity = new Vector2(xVelocity, yVelocity);

            _rigidBody.velocity = initialVelocity.normalized * _ballSpeed;
        }
    }
}