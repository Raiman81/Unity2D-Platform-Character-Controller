using Checks;
using Controllers;
using UnityEngine;

namespace Capabilities
{
    public class WallInteraction : MonoBehaviour
    {
        private bool WallJumping { get; set; }
        
        [Header("Wall Slide")] 
        [SerializeField] [Range(0.1f, 5f)] private float wallSlideMaxSpeed = 2f;

        [Header(("Wall Jump"))] 
        [SerializeField] private Vector2 wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] private Vector2 wallJumpLeap = new Vector2(14f, 12f);
        [SerializeField, Range(0.05f, 0.5f)] private float wallStickTime = 0.25f;
        
        private CollisionDataRetriever _collisionDataRetriever;
        private Rigidbody2D _rb;
        private Controller _controller;
        
        
        private Vector2 _velocity;
        private bool _onWall, _onGround, _desiredJump;
        private float _wallDirectionX, _wallStickCounter;

        private void Start()
        {
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _rb = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();
            

        }

        private void Update()
        {
            if (_onWall && !_onGround)
            {
                _desiredJump |= _controller.input.RetrieveJumpInput(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _velocity = _rb.velocity;
            _onWall = _collisionDataRetriever.OnWall;
            _onGround = _collisionDataRetriever.OnGround;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;
            
            #region Wall Stick
            if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && !WallJumping)
            {
                if (_wallStickCounter > 0)
                {
                    _velocity.x = 0;

                    if (_controller.input.RetrieveMoveInput(gameObject) == _collisionDataRetriever.ContactNormal.x)
                    {
                        _wallStickCounter -= Time.deltaTime;
                    }
                    else
                    {
                        _wallStickCounter = wallStickTime;
                    }
                } 
                else
                {
                    _wallStickCounter = wallStickTime;
                }
            }
            #endregion
            #region Wall Slide
            if (_onWall)
            {
                if (_velocity.y < -wallSlideMaxSpeed)
                {
                    _velocity.y = -wallSlideMaxSpeed;
                }  
            }
            #endregion

            #region  Wall Jump

            if ((_onWall && _velocity.x == 0) || _onGround)
            {
                WallJumping = false;
            }
            
            if (_desiredJump)
            {
                if (-_wallDirectionX == _controller.input.RetrieveMoveInput(gameObject))
                {
                    _velocity = new Vector2(wallJumpClimb.x * _wallDirectionX, wallJumpClimb.y);
                    WallJumping = true;
                    _desiredJump = false;
                }
                else if (_controller.input.RetrieveMoveInput(gameObject) == 0)
                {
                    _velocity = new Vector2(wallJumpBounce.x * _wallDirectionX, wallJumpBounce.y);
                    WallJumping = true;
                    _desiredJump = false;
                }
                else
                {
                    _velocity = new Vector2(wallJumpLeap.x * _wallDirectionX, wallJumpLeap.y);
                    WallJumping = true;
                    _desiredJump = false;
                }
            }
            #endregion

            _rb.velocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionDataRetriever.EvaluateCollision(collision);

            if (_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
            {   
                _rb.velocity = Vector2.zero;
            }
        }
    }
}
