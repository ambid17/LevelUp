using System.Collections;
using TMPro;
using UnityEngine;
using Utils;
using DG.Tweening;

namespace Minigames.Fight
{
    public class EnemyMovementController : MovementController
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] private TextMeshPro _damageText;
        [SerializeField] protected Transform targetTransform;
        protected Rigidbody2D _rigidbody2D;
        protected SpriteRenderer _spriteRenderer;
    
        protected float currentHp;
        [SerializeField] protected EnemyInstanceSettings settings;

        [SerializeField] private float idealDistanceFromPlayer;

        private const float MaxDistanceFromPlayer = 100;

        private bool isMarkedForDeath;

        private float _flashTimer;
        private float _flashTime = 0.1f;
        private bool _isFlashing;
        private Color _defaultColor;
        protected EventService eventService;
        protected float moveSpeed;
        
        void Start()
        {
            eventService = GameManager.EventService;
            eventService.Add<PlayerDiedEvent>(Cull);
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _spriteRenderer.color = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
            _damageText.enabled = false;
        }

        public void Setup(EnemyInstanceSettings settings, Transform targetTransform)
        {
            this.settings = settings;
            currentHp = settings.MaxHp;
            moveSpeed = settings.MoveSpeed;
            this.targetTransform = targetTransform;
        }

        protected virtual void Update()
        {
            TryMove();
            TryCull();
            CheckFlash();
        }

        public override void ApplyMoveEffect(float speedRatio)
        {
            moveSpeed *= speedRatio;
        }

        public override void RemoveMoveEffect(float speedRatio)
        {
            moveSpeed /= speedRatio;
        }

        protected virtual void TryMove()
        {
            Vector2 velocity = _rigidbody2D.velocity;
            Vector2 offset = targetTransform.position - transform.position;

            Vector2 targetVelocity;

            if (offset.magnitude > idealDistanceFromPlayer)
            {
                targetVelocity = offset.normalized * moveSpeed;
            }
            else
            {
                targetVelocity = Vector2.zero;
            }

            velocity = Vector2.MoveTowards(velocity, targetVelocity, settings.acceleration * Time.deltaTime);
        
            _rigidbody2D.velocity = velocity;

            FlipSpriteOnDirection();
        }

        // If the player runs too far from the enemy, kill it off
        private void TryCull()
        {
            Vector2 offsetFromPlayer = targetTransform.position - transform.position;
            if (offsetFromPlayer.magnitude > MaxDistanceFromPlayer)
            {
                Cull();
            }
        }

        public void ShowDamageFx(float damage)
        {
            _spriteRenderer.material = _flashMaterial;
            if (!_isFlashing)
            {
                _defaultColor = _spriteRenderer.color;
            }
            _spriteRenderer.color = Color.white;
            _isFlashing = true;
            currentHp -= damage;

            StartCoroutine(ShowDamage(damage));
            
            if (currentHp <= 0)
            {
                Die();
            }
        }

        private IEnumerator ShowDamage(float damage)
        {
            _damageText.enabled = true;
            _damageText.text = damage.ToString();
            Sequence sequence = _damageText.transform.DOJump(transform.position, 0.5f, 1, 1);
            yield return sequence.WaitForCompletion();
            _damageText.enabled = false;
        }

        public void Knockback(Vector2 force)
        {
            _rigidbody2D.velocity = force;
        }

        private void CheckFlash()
        {
            if (_isFlashing)
            {
                _flashTimer += Time.deltaTime;

                if (_flashTimer > _flashTime)
                {
                    _spriteRenderer.material = _defaultMaterial;
                    _spriteRenderer.color = _defaultColor;
                    _flashTimer = 0;
                    _isFlashing = false;
                }
            }
        }

        protected void  FlipSpriteOnDirection()
        {
            //Flip the sprite based on velocity
            if(_rigidbody2D.velocity.x < 0) 
                _spriteRenderer.flipX = true;
            else 
                _spriteRenderer.flipX = false;
        }

        private void Die()
        {
            // Since we are shooting so many projectiles...
            // OnTriggerEnter() gets called in Projectile multiple times before we get destroyed
            // This prevents duplicate deaths
            if (isMarkedForDeath)
            {
                return;
            }
        
            isMarkedForDeath = true;
        
            GameManager.EnemySpawner.EnemyCount--;
            GameManager.CurrencyManager.EnemyKilled(settings);
        
            Destroy(gameObject);
        }

        private void Cull()
        {
            if (isMarkedForDeath)
            {
                return;
            }
        
            isMarkedForDeath = true;
        
            GameManager.EnemySpawner.EnemyCount--;

            Destroy(gameObject);
        }
    }
}
