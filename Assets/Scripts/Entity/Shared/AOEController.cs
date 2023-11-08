using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class AOEController : MonoBehaviour
    {
        protected bool canTriggerEffect => isCollidingWithTarget && localEffectInterval <= 0;
        protected HitData storedHitData;

        [SerializeField]
        private float lifeTime;
        [SerializeField]
        private float effectInterval;

        [SerializeField]
        protected bool overrideEffectDamage;
        [SerializeField]
        protected float damageOverrideValue;
        [SerializeField]
        protected bool overrideStatusEffects;
        [SerializeField]
        protected List<Effect> statusEffectOverrides;

        protected List<Entity> effectedEntities = new();

        protected bool isCollidingWithTarget;

        private float localEffectInterval;

        protected virtual void Start()
        {
            Destroy(gameObject, lifeTime);
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            
        }
        public virtual void SetUp(Entity entity)
        {
            storedHitData = new HitData(entity, damageOverrideValue);
            if (overrideStatusEffects)
            {
                storedHitData.Effects = statusEffectOverrides;
            }
        }

        protected virtual void Update()
        {
            
            if (localEffectInterval > 0)
            {
                localEffectInterval -= Time.deltaTime;
                return;
            }
            if (!isCollidingWithTarget)
            {
                return;
            }
            localEffectInterval = effectInterval;
        }
    }
}