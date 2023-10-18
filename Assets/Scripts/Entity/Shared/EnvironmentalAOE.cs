using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnvironmentalAOE : AOEController
    {
        protected override void Start()
        {
            base.Start();
            SetUp(GameManager.PlayerEntity);
            isCollidingWithTarget = true;
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                isCollidingWithTarget = true;
                Entity entity = collision.GetComponent<Entity>();

                effectedEntities.Add(entity);
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                Entity entity = collision.GetComponent<Entity>();
                foreach (Entity effectedEntity in effectedEntities.ToList())
                {
                    if (effectedEntity == entity)
                    {
                        effectedEntities.Remove(entity);
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