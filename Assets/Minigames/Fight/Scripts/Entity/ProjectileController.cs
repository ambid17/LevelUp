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

        protected virtual void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);
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

        protected virtual bool ShouldDie()
        {
            return false;
        }

        protected virtual void Move()
        {
            
        }

        public void Setup(Entity myEntity, Vector2 direction)
        {
            _myEntity = myEntity;
            _shootDirection = direction.normalized;
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