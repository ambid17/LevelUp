using UnityEngine;

namespace Minigames.Fight
{
    public class EnvironmentalAOE : AOEController
    {
        private void Start()
        {
            SetUp(GameManager.PlayerEntity);
        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity entity = collision.GetComponent<Entity>();
                HitData hitToAdd = storedHitData;
                hitToAdd.Target = entity;

                hits.Add(hitToAdd);
            }
        }
        protected override void OnTriggerExit2D(Collider2D collision)
        {
            base.OnTriggerExit2D(collision);
            if (collision.gameObject.layer == PhysicsUtils.EnemyLayer || collision.gameObject.layer == PhysicsUtils.EnemyLayer)
            {
                Entity entity = collision.GetComponent<Entity>();
                foreach (HitData hit in hits)
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