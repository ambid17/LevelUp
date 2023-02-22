using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerProjectile : MonoBehaviour
    {
        [SerializeField] private float timeToLive = 5;
        [SerializeField] private float moveSpeed = 5;
        private float _deathTimer = 0;
        private Vector2 _shootDirection;
        private int _penetrationsLeft;
        private bool _isMarkedForDeath;

        private Entity _sourceEntity;

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

        public void Setup(Entity sourceEntity, Vector2 direction)
        {
            _sourceEntity = sourceEntity;
            _shootDirection = direction.normalized;
            // TODO: look at effects for this
            _penetrationsLeft = 1;
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
                Entity enemyEntity = col.gameObject.GetComponent<Entity>();
                _sourceEntity.OnHitOther(enemyEntity);
            
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
