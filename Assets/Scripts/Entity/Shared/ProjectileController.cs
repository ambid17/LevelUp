using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class ProjectileController : MonoBehaviour
    {
        protected bool spawnAoeOnDeath;
        protected AOEController aoePrefab; 

        protected Entity _myEntity;
        protected float _deathTimer = 0;
        protected Vector2 _shootDirection;
        protected bool _isMarkedForDeath;

        protected HitData hit;

        private EventService _eventService;

        public Entity MyEntity => _myEntity;

        protected virtual void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);
        }

        private void OnDestroy()
        {
            if (spawnAoeOnDeath)
            {
                AOEController aOEController = Instantiate(aoePrefab, transform.position, new Quaternion(0, 0, 0, 0));
                aOEController.SetUp(_myEntity);
            }
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

        protected virtual bool ShouldDie()
        {
            return false;
        }

        protected virtual void Move()
        {
            
        }

        public virtual void Setup(Entity myEntity, Vector2 direction, WeaponController controller)
        {
            _myEntity = myEntity;
            _shootDirection = direction.normalized;

            hit = controller.Hit;

            ProjectileWeaponController projectileController = controller as ProjectileWeaponController;
            spawnAoeOnDeath = projectileController.SpawnAoeOnDeath;
            aoePrefab = projectileController.AoePrefab;
        }

        protected virtual void OnTriggerEnter2D(Collider2D col)
        {
            
        }

        protected virtual void Die()
        {
            if (_isMarkedForDeath) return;

            _isMarkedForDeath = true;

            Destroy(gameObject);
        }
    }
}