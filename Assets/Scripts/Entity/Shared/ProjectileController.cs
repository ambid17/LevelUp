using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class ProjectileController : MonoBehaviour
    {
        protected Entity _myEntity;
        protected float _deathTimer = 0;
        protected Vector2 _shootDirection;
        protected bool _isMarkedForDeath;

        private EventService _eventService;

        public Entity MyEntity => _myEntity;
        private WeaponStats _myWeaponStats;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D rb;


        protected virtual void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void OnDestroy()
        {
            _eventService.Remove<PlayerDiedEvent>(Die);
        }

        void Update()
        {
            _deathTimer += Time.deltaTime;

            if (ShouldDie())
            {
                Die();
            }

            
        }
        private void FixedUpdate()
        {
            Move();
        }

        public virtual void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        protected  bool ShouldDie()
        {
            return _deathTimer >  _myWeaponStats.projectileLifeTime.Calculated;
        }

        protected void Move()
        {
            rb.velocity = _shootDirection * _myWeaponStats.projectileMoveSpeed.Calculated;
        }

        public virtual void Setup(Entity myEntity, Vector2 direction)
        {
            _myEntity = myEntity;
            _shootDirection = direction.normalized;

            // Grab the current weapon stats on setup
            // If we did this anywhere else later on, the current weapon could be swapped out
            _myWeaponStats = MyEntity.WeaponController.CurrentWeapon;
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (_isMarkedForDeath)
            {
                return;
            }

            if (col.gameObject.layer == PhysicsUtils.ObstacleLayer || col.gameObject.layer == PhysicsUtils.wallLayer)
            {
                Die();
            }

            if (!IsValidTarget(col.gameObject.layer))
            {
                return;
            }

            Entity target = col.gameObject.GetComponent<Entity>();

            MyEntity.DealDamage(target, _myWeaponStats);

            Die();
        }

        protected virtual bool IsValidTarget(int layer)
        {
            return true;
        }

        protected virtual void Die()
        {
            if (_isMarkedForDeath) return;

            _isMarkedForDeath = true;

            foreach(var effect in _myWeaponStats.AoeEffects)
            {
                // TODO: add in offsets so that the effects aren't all on top of each other
                effect.Place(MyEntity, transform.position);
            }

            Destroy(gameObject);
        }
    }
}