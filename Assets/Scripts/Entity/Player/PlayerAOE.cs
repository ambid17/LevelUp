using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerAOE : AOEController
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity entity = collision.GetComponent<Entity>();

                effectedEntities.Add(entity);
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity entity = collision.GetComponent<Entity>();
                foreach (Entity effectedEntity in effectedEntities)
                {
                    if (effectedEntity == entity)
                    {
                        effectedEntities.Remove(effectedEntity);
                    }
                }
            }
        }
        protected override void Update()
        {
            base.Update();
            if (canTriggerEffect)
            {
                foreach (Entity effectedEntity in effectedEntities)
                {
                    effectedEntity.TakeHit(storedHitData);
                }
            }
        }
    }
}