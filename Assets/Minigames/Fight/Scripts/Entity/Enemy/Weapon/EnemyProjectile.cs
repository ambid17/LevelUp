using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class EnemyProjectile : ProjectileController
    {
        public SpriteRenderer MySpriteRenderer => mySpriteRenderer;

        [SerializeField]
        private SpriteRenderer mySpriteRenderer;

        private EnemyEntity _overriddenEntity;


        protected override void Start()
        {
            base.Start();
            _overriddenEntity = _myEntity as EnemyEntity;
        }

        protected override bool ShouldDie()
        {
            return _deathTimer > _overriddenEntity.enemyStats.ProjectileLifeTime;
        }

        protected override void Move()
        {
            Vector2 delta = _shootDirection * _overriddenEntity.enemyStats.ProjectileSpeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        protected override void OnTriggerEnter2D(Collider2D col)
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
                GameManager.PlayerEntity.TakeHit(hit);
                Die();
            }
        }
    }
}