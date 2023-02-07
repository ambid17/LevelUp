using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _flashMaterial;
        [SerializeField] protected EnemyProjectile projectilePrefab;
        protected Rigidbody2D _rigidbody2D;
        protected SpriteRenderer _spriteRenderer;
        protected Transform playerTransform;
    
        protected float shotTimer;
        protected float currentHp;
        [SerializeField] protected EnemyInstanceSettings settings;

        [SerializeField] private float idealDistanceFromPlayer;

        private const float MaxDistanceFromPlayer = 100;

        private bool isMarkedForDeath;

        private float _flashTimer;
        private float _flashTime = 0.1f;
        private bool _isFlashing;
        private Color _defaultColor;
        
        private EventService _eventService;
        
        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<PlayerDiedEvent>(Cull);
            
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            playerTransform = GameManager.Player.transform;
            _spriteRenderer.color = GameManager.SettingsManager.progressSettings.CurrentWorld.CurrentCountry.EnemyTierColor;
        }

        public void Setup(EnemyInstanceSettings settings)
        {
            this.settings = settings;
            currentHp = settings.MaxHp;
        }

        protected virtual void Update()
        {
            TryShoot();
            TryMove();
            TryCull();
            CheckFlash();
        }

        protected virtual void TryShoot()
        {
            shotTimer += Time.deltaTime;
            if (shotTimer > settings.FireRate)
            {
                shotTimer = 0;
            
                EnemyProjectile projectile = Instantiate(projectilePrefab);
                projectile.transform.position = transform.position;
            
                Vector2 direction = playerTransform.position - transform.position;

                projectile.Setup(settings, direction);
            }
        }

        protected virtual void TryMove()
        {
            Vector2 velocity = _rigidbody2D.velocity;
            Vector2 offset = playerTransform.position - transform.position;

            Vector2 targetVelocity;

            if (offset.magnitude > idealDistanceFromPlayer)
            {
                targetVelocity = offset.normalized * settings.MoveSpeed;
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
            Vector2 offsetFromPlayer = playerTransform.position - transform.position;
            if (offsetFromPlayer.magnitude > MaxDistanceFromPlayer)
            {
                Cull();
            }
        }

        public void TakeDamage(float damage)
        {
            _spriteRenderer.material = _flashMaterial;
            if (!_isFlashing)
            {
                _defaultColor = _spriteRenderer.color;
            }
            _spriteRenderer.color = Color.white;
            _isFlashing = true;
            currentHp -= damage;

            if (currentHp <= 0)
            {
                Die();
            }
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
            GameManager.GameStateManager.EnemyKilled(settings);
        
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
