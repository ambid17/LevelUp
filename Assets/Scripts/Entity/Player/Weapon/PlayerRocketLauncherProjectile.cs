using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class PlayerRocketLauncherProjectile : PlayerProjectile
    {
        private float _blastRadius = 3;
        [SerializeField] private LayerMask _layerMask;

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
                Die();
            }
        }

        protected override void Die()
        {
            if (_isMarkedForDeath) return;

            _isMarkedForDeath = true;
            
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _blastRadius, _layerMask);
            foreach (var hit in hits)
            {
                Entity enemyEntity = hit.gameObject.GetComponent<Entity>();
                enemyEntity.TakeHit(this.hit);
            }

            Destroy(gameObject);
        }
    }
}