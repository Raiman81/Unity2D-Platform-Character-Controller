using Checks;
using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capabilities
{
    [RequireComponent(typeof(Controller))]
    public class Jump : MonoBehaviour
    {
        [SerializeField, Range(0f, 10f)] private float jumpHeight = 4.0f;
        [SerializeField, Range(0, 5)] private int maxAirJumps = 1;
        [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3.5f;
        [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.5f;
        [SerializeField, Range(0f, .3f)] private float coyoteTime = 0.15f;
        [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.1f;
        
        private Controller _controller;
        private Rigidbody2D _rb;
        private CollisionDataRetriever _ground;
        private Vector2 _velocity;

        private int _jumpPhase;
        private float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;

        private bool _desiredJump, _onGround, _isJumping;


        // Start is called before the first frame update
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ground = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();

            _defaultGravityScale = 1f;
        }

        // Update is called once per frame
        private void Update()
        {
            _desiredJump |= _controller.input.RetrieveJumpInput();
        }

        private void FixedUpdate()
        {
            _onGround = _ground.OnGround;
            _velocity = _rb.velocity;

            if (_onGround && _rb.velocity.y == 0)
            {
                _jumpPhase = 0;
                _coyoteCounter = coyoteTime;
                _isJumping = false;
            }
            else
            {
                _coyoteCounter -= Time.deltaTime;
               
            }

            if (_desiredJump)
            {
                _desiredJump = false;
                _jumpBufferCounter = jumpBufferTime;
            }
            else if (!_desiredJump && _jumpBufferCounter > 0)
            {
                _jumpBufferCounter -= Time.deltaTime;
            }

            if (_jumpBufferCounter > 0)
            {
                JumpAction();
                
            }

            if (_controller.input.RetrieveJumpHoldInput() && _rb.velocity.y > 0)
            {
                _rb.gravityScale = upwardMovementMultiplier;
            }
            else if (!_controller.input.RetrieveJumpHoldInput() || _rb.velocity.y < 0)
            {
                _rb.gravityScale = downwardMovementMultiplier;
            }
            else if(_rb.velocity.y == 0)
            {
                _rb.gravityScale = _defaultGravityScale;
            }

            _rb.velocity = _velocity;
        }
        private void JumpAction()
        {
            if (_coyoteCounter > 0f || (_jumpPhase < maxAirJumps && _isJumping))
            {
                if (_isJumping)
                {
                    _jumpPhase += 1; 
                }

                _jumpBufferCounter = 0;
                _coyoteCounter = 0;
                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                _isJumping = true;
                
                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                else if (_velocity.y < 0f)
                {
                    _jumpSpeed += Mathf.Abs(_rb.velocity.y);
                }
                _velocity.y += _jumpSpeed;
            }
        }
    }
}

