using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class Projectile : MonoBehaviour
    {
        public enum OwnerType
        {
            Player,
            Enemy
        }
    
        [SerializeField] private float timeToLive = 5;
        [SerializeField] private float moveSpeed = 5;
        private OwnerType _owner;
        private float _damage;
        private float _deathTimer = 0;
        private Vector2 _shootDirection;
        private int _penetrationsLeft;
        private bool _isMarkedForDeath;

        private EventService _eventService;
    
        void Start()
        {
            _eventService = Services.Instance.EventService;
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

        public void SetupForEnemy(float damage, Vector2 direction)
        {
            _owner = OwnerType.Enemy;
            _damage = damage;
            _shootDirection = direction.normalized;
        }

        public void SetupForPlayer(float damage, Vector2 direction)
        {
            _owner = OwnerType.Player;
            _damage = damage;
            _shootDirection = direction.normalized;
            _penetrationsLeft = GameManager.SettingsManager.weaponSettings.ProjectilePenetration;
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
            else if (col.gameObject.layer == PhysicsUtils.EnemyLayer && _owner == OwnerType.Player)
            {
                EnemyController enemy = col.gameObject.GetComponent<EnemyController>();
                enemy.TakeDamage(_damage);
            
                if (_penetrationsLeft <= 0)
                {
                    Die();
                }

                if (GameManager.SettingsManager.playerSettings.LifeSteal > 0)
                {
                    GameManager.GameStateManager.CurrentPlayerHP += _damage;
                }
            
                _penetrationsLeft--;
            }
            else if (col.gameObject.layer == PhysicsUtils.PlayerLayer && _owner == OwnerType.Enemy)
            {
                GameManager.GameStateManager.TakeDamage(_damage);
                Die();
            }
        }

        private void Die()
        {
            _isMarkedForDeath = true;
            Destroy(gameObject);
        }
    }
}
