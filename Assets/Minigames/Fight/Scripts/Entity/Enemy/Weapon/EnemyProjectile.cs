using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class EnemyProjectile : MonoBehaviour
    {
        private EnemyEntity _myEntity;
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

            if (_deathTimer > _myEntity.enemyStats.ProjectileLifeTime)
            {
                Die();
            }

            Move();
        }

        private void Move()
        {
            Vector2 delta = _shootDirection * _myEntity.enemyStats.ProjectileSpeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        public void Setup(EnemyEntity myEntity, Vector2 direction)
        {
            _myEntity = myEntity;
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
                _myEntity.OnHitOther(GameManager.PlayerEntity);
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