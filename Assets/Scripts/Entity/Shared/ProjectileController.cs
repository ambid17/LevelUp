using UnityEditor.Animations;
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
        protected WeaponStats _myWeaponStats;

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        private Animator _anim;

        private bool _isOverridingLifetime;
        private float _lifetimeOverride;


        protected virtual void Awake()
        {
            _eventService = Platform.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);

            _spriteRenderer = GetComponent<SpriteRenderer>();
            _anim = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
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

        protected virtual bool ShouldDie()
        {
            if (_isOverridingLifetime)
            {
                return _deathTimer > _lifetimeOverride;
            }
            return _deathTimer >  _myWeaponStats.projectileLifeTime.Calculated;
        }

        protected virtual void Move()
        {
            _rb.velocity = _shootDirection * _myWeaponStats.projectileMoveSpeed.Calculated;
        }

        public virtual void Setup(Entity myEntity, WeaponStats weapon, Vector2 direction, float lifetimeOverride = 0)
        {
            _myEntity = myEntity;
            _shootDirection = direction.normalized;

            // Grab the current weapon stats on setup
            // If we did this anywhere else later on, the current weapon could be swapped out
            _myWeaponStats = weapon;

            _spriteRenderer.sprite = _myWeaponStats.sprite;
            transform.localScale *= _myWeaponStats.projectileSize.Calculated;

            if (_myWeaponStats.animation != null)
            {
                _anim.runtimeAnimatorController = _myWeaponStats.animation;
            }
            if (lifetimeOverride != 0)
            {
                _isOverridingLifetime = true;
                _lifetimeOverride = lifetimeOverride;
            }
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

            if ((_myWeaponStats.targetLayers.value & (1 << col.transform.gameObject.layer)) > 0)
            {
                Entity target = col.gameObject.GetComponent<Entity>();

                MyEntity.DealDamage(target, _myWeaponStats);
            }

            if ((_myWeaponStats.destroyOnImpactLayers.value & (1 << col.transform.gameObject.layer)) > 0)
            {
                Die();
            }
            
        }

        public virtual void OverrideVisuals(Sprite sprite, AnimatorController animation)
        {
            _spriteRenderer.sprite = sprite;
            if (animation != null)
            {
                _anim.runtimeAnimatorController = animation;
            }
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