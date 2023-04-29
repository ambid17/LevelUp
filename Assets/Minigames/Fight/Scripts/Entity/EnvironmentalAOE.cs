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
                HitData hitToAdd = storedHitData;
                hitToAdd.Target = entity;

                hits.Add(hitToAdd);
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.PlayerLayer)
            {
                Entity entity = collision.GetComponent<Entity>();
                foreach (HitData hit in hits.ToList())
                {
                    if (hit.Target == entity)
                    {
                        hits.Remove(hit);
                    }
                }
            }
        }
        protected override void Update()
        {
            base.Update();
            if (canTriggerEffect)
            {
                foreach (HitData hit in hits)
                {
                    hit.Target.TakeHit(hit);
                }
            }
        }
    }
}