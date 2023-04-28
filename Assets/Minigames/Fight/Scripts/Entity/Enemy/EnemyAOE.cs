using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyAOE : AOEController
    {
        private HitData storedHitData;
        public override void SetUp(Entity entity)
        {
            base.SetUp(entity);
            storedHitData = new HitData(entity, GameManager.PlayerEntity);
            if (overrideEffectDamage)
            {
                storedHitData.BaseDamage = damageOverrideValue;
            }
            if (overrideStatusEffects)
            {
                storedHitData.Effects = statusEffectOverrides;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                isCollidingWithTarget = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
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
                storedHitData.Target.TakeHit(storedHitData);
            }
        }
    }
}