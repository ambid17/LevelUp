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
        private bool _hasGoneUnderwater;
        private bool _isMarkedForDestroy;
        
        private Vector2 _movementToApply;
        private Vector2 _currentInput;

        private readonly float _reeledInHeight = 0; // The height at which the lure is considered "Reeled in"

        public float CurrentDepth => transform.position.y;
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
        }

        private void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<LureSnappedEvent>(OnLureSnapped);
        }

        private void Update()
        {
            if (_isThrown)
            {
                if (_hasGoneUnderwater)
                {
                    // Let the projectile's settings handle fall speed
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
            
            // Fall at the projectile's speed as long as we are above the MaxDepth
            if (transform.position.y > GameManager.ProjectileSettings.CurrentProjectile.MaxDepth)
            {
                input.y -= 1;
            }

            input = input.normalized;
            input.x *= GameManager.ProjectileSettings.CurrentProjectile.HorizontalMoveSpeed;
            input.y *= GameManager.ProjectileSettings.CurrentProjectile.FallSpeed;
            
            // Override y velocity if reeling in
            if(Input.GetMouseButton(0))
            {
                input.y = GameManager.ProjectileSettings.CurrentProjectile.ReelSpeed;
            }
            
            _currentInput = input;
        }
        
        private void Move()
        {
            _rigidbody.velocity = _movementToApply;
        }

        private void CheckReeledIn()
        {
            if (transform.position.y >= _reeledInHeight)
            {
                _isThrown = false;
                _eventService.Dispatch<ReeledInEvent>();
                Die();
            }
        }

        private void OnLureSnapped()
        {
            Die();
        }

        private void Die()
        {
            if (_isMarkedForDestroy) return;
            
            _isMarkedForDestroy = true;
            Destroy(gameObject);
        }
        
        private void FixedUpdate()
        {
            ApplyAcceleration();
        }
        
        private void ApplyAcceleration()
        {
            float xAcceleration = GameManager.ProjectileSettings.CurrentProjectile.HorizontalAcceleration * Time.fixedDeltaTime;
            float yAcceleration = GameManager.ProjectileSettings.CurrentProjectile.VerticalAcceleration * Time.fixedDeltaTime;
            
            _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, xAcceleration);
            _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, yAcceleration);
        }

        public void Setup(Projectile projectile)
        {
            _spriteRenderer.sprite = projectile.Sprite;
        }

        public void Throw(Vector3 velocity)
        {
            _isThrown = true;
            // The prefab is kinematic by default so it doesn't fall on spawn
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            // if its a fish add it to our list of fishies
            if (col.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                FishController fishController = col.gameObject.GetComponent<FishController>();
                _eventService.Dispatch(new FishCaughtEvent(fishController.Fish));
                fishController.Catch(transform);
            }
        }
    }
}