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
        private CombatStats _myCombatStats => MyEntity.Stats.combatStats;

        private SpriteRenderer _spriteRenderer;


        protected virtual void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);

            _spriteRenderer = GetComponent<SpriteRenderer>();
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

            Move();
        }

        public virtual void SetSprite(Sprite sprite)
        {
            _spriteRenderer.sprite = sprite;
        }

        protected  bool ShouldDie()
        {
            return _deathTimer > _myCombatStats.projectileWeaponStats.projectileLifeTime.Calculated;
        }

        protected void Move()
        {
            Vector2 delta = _shootDirection * _myCombatStats.projectileWeaponStats.projectileMoveSpeed.Calculated * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        public virtual void Setup(Entity myEntity, Vector2 direction)
        {
            _myEntity = myEntity;
            _shootDirection = direction.normalized;
        }

        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (_isMarkedForDeath)
            {
                return;
            }

            if (col.gameObject.layer == PhysicsUtils.GroundLayer)
            {
                Die();
            }

            if (!IsValidTarget(col.gameObject.layer))
            {
                return;
            }

            Entity target = col.gameObject.GetComponent<Entity>();

            MyEntity.DealDamage(target);

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

            Destroy(gameObject);
        }
    }
}