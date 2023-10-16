using System;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerProjectile : ProjectileController
    {
        [SerializeField] private float timeToLive = 5;
        [SerializeField] private float moveSpeed = 5;

        private int _penetrationsLeft;
        private bool _canPenetrateIndefinitely;

        protected override bool ShouldDie()
        {
            return _deathTimer > timeToLive;
        }
        protected override void Move()
        {
            Vector2 delta = _shootDirection * moveSpeed * Time.deltaTime;
            transform.position += new Vector3(delta.x, delta.y, 0);
        }

        public override void Setup(Entity myEntity, Vector2 direction, WeaponController controller)
        {
            base.Setup(myEntity, direction, controller);
        
            _shootDirection = direction.normalized;
            //// TODO: look at effects for this
            //_canPenetrateIndefinitely = canPenetrateIndefinitely;
            _penetrationsLeft = 1;
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
            else if (col.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity enemyEntity = col.gameObject.GetComponent<Entity>();
                enemyEntity.TakeHit(hit);

                if (_canPenetrateIndefinitely)
                {
                    return;
                }

                if (_penetrationsLeft <= 0)
                {
                    Die();
                }

                _penetrationsLeft--;
            }
        }
    }
}
