using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyAOE : AOEController
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                isCollidingWithTarget = true;
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                isCollidingWithTarget = false;
            }
        }
        protected override void Update()
        {
            base.Update();
            if (canTriggerEffect)
            {
                GameManager.PlayerEntity.TakeHit(storedHitData);
            }
        }
    }
}