using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;
using Utils;
namespace Minigames.Fish
{
    public class Lure : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;

        private EventService _eventService;

        private bool _isThrown;
        private Projectile _projectile;
        private bool _hasGoneUnderwater;
        
        private Vector2 _movementToApply;
        private Vector2 _currentInput;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }

        private void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<LureThrownEvent>(OnThrown);
        }

        private void OnThrown(LureThrownEvent thrownEvent)
        {
            _eventService.Remove<LureThrownEvent>(OnThrown);
        }

        private void Update()
        {
            if (_isThrown)
            {
                if (_hasGoneUnderwater)
                {
                    _rigidbody.gravityScale = 0;
                    GetMovementInput();
                    Move();
                    CheckReeledIn();
                }
                else
                {
                    if (transform.position.y >= 0)
                    {
                        _rigidbody.gravityScale = 1;
                    }
                    else
                    {
                        _hasGoneUnderwater = true;
                    }
                }
            }
        }
        
        private void GetMovementInput()
        {
            Vector2 input = Vector2.zero;
            if (Input.GetKey(KeyCode.D))
            {
                input.x += 1;
            }
        
            if (Input.GetKey(KeyCode.A))
            {
                input.x -= 1;
            }

            if (transform.position.y > GameManager.ProjectileSettings.CurrentProjectile.MaxDepth)
            {
                input.y -= 1;
            }
            
            if(Input.GetMouseButton(0))
            {
                input.y = 1;
            }

            input = input.normalized;
            input.x *= GameManager.ProjectileSettings.CurrentProjectile.HorizontalMoveSpeed;
            input.y *= GameManager.ProjectileSettings.CurrentProjectile.FallSpeed;
            
            _currentInput = input;
        }
        
        private void Move()
        {
            _rigidbody.velocity = _movementToApply;
        }

        private void CheckReeledIn()
        {
            if (transform.position.y >= 0)
            {
                _isThrown = false;
                _eventService.Dispatch<ReeledInEvent>();
                Destroy(gameObject);
            }
        }
        
        private void FixedUpdate()
        {
            ApplyAcceleration();
        }
        
        private void ApplyAcceleration()
        {
            float maxAcceleration = GameManager.ProjectileSettings.CurrentProjectile.Acceleration * Time.fixedDeltaTime;
            _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, maxAcceleration);
            _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, maxAcceleration);
        }

        public void Setup(Projectile projectile)
        {
            _projectile = projectile;
            _spriteRenderer.sprite = projectile.Sprite;
        }

        public void Throw(Vector3 velocity)
        {
            _isThrown = true;
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log($"Collided with {col.gameObject.name}");
            // if its a fish add it to our list of fishies
            if (col.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                FishInstanceSettings fish = col.gameObject.GetComponent<FishController>().Fish;
                _eventService.Dispatch(new FishCaughtEvent(fish));
                Destroy(col.gameObject);
            }
        }

        private void OnDestroy()
        {
            // TODO unsubscribe from events
        }
    }
}