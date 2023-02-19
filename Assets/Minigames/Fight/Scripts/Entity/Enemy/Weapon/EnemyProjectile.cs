using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class EnemyProjectile : MonoBehaviour
    {
        private EnemyStats _stats;
        private float _deathTimer = 0;
        private Vector2 _shootDirection;
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

            if (_deathTimer > _stats.ProjectileLifeTime)
            {
                Die();
            }

            Move();
        }

        private void Move()
        {
            Vector2 delta = _shootDirection * _stats.ProjectileSpeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        public void Setup(EnemyStats stats, Vector2 direction)
        {
            _stats = stats;
            _shootDirection = direction.normalized;
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
            else if (col.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                _eventService.Dispatch(new OnPlayerDamageEvent(_stats.WeaponDamage));
                Die();
            }
        }

        private void Die()
        {
            if (_isMarkedForDeath) return;

            _isMarkedForDeath = true;

            Destroy(gameObject);
        }
    }
}