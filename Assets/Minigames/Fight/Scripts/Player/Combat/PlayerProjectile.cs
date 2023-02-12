using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float timeToLive = 5;
        [SerializeField] private float moveSpeed = 5;
        private float _damage;
        private float _deathTimer = 0;
        private Vector2 _shootDirection;
        private int _penetrationsLeft;
        private bool _isMarkedForDeath;

        private EventService _eventService;
    
        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<PlayerDiedEvent>(Die);
        }

        void Update()
        {
            _deathTimer += Time.deltaTime;

            if (_deathTimer > timeToLive)
            {
                Destroy(gameObject);
            }

            Move();
        }

        private void Move()
        {
            Vector2 delta = _shootDirection * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        public void Setup(float damage, Vector2 direction, int penetration)
        {
            _damage = damage;
            _shootDirection = direction.normalized;
            _penetrationsLeft = penetration;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (_isMarkedForDeath)
            {
                return;
            }
        
            if (col.gameObject.layer == PhysicsUtils.GroundLayer)
            {
                Die();
            }
            else if (col.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity entity = col.gameObject.GetComponent<Entity>();
                _eventService.Dispatch(new OnHitEvent(entity));
                // EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
                // enemy.TakeDamage(_damage);
                //
                // if (GameManager.SettingsManager.playerSettings.LifeSteal > 0)
                // {
                //     _eventService.Dispatch(new OnLifestealEvent(_damage));
                // }
            
                if (_penetrationsLeft <= 0)
                {
                    Die();
                }
            
                _penetrationsLeft--;
            }
        }

        private void Die()
        {  
            if(_isMarkedForDeath) return;
            
            _isMarkedForDeath = true;
            
            Destroy(gameObject);
        }
    }
}
