using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerMeleeAoE : PlayerProjectile
    {
        [SerializeField]
        private Animator anim;

        protected override bool ShouldDie()
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        }

        public override void Setup(Entity myEntity, Vector2 direction, WeaponController controller)
        {
            base.Setup(myEntity, direction, controller);
            transform.rotation = PhysicsUtils.LookAt(transform, GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition), 180);
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
            }
        }
    }
}