using Checks;
using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capabilities
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever),
        typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 25f;
        

        private Controller _controller;
        private Vector2 _direction, _desiredVelocity, _velocity;
        private Rigidbody2D _rb;
        private CollisionDataRetriever _collisionDataRetriever;
        

        private float _maxSpeedChange, _acceleration;
        private bool _onGround;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            
        }

        private void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetriever.Friction, 0f);
        }

        private void FixedUpdate()
        {
            _onGround = _collisionDataRetriever.OnGround;
            _velocity = _rb.velocity;

            _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);


            _rb.velocity = _velocity;
        }
    }
}
