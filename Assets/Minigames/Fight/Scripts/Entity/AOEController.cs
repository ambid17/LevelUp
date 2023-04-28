using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class AOEController : MonoBehaviour
    {
        protected bool canTriggerEffect => isCollidingWithTarget && localEffectInterval <= 0;

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

        protected bool isCollidingWithTarget;
        protected Entity storedEntity;

        private float localEffectInterval;

        public virtual void SetUp(Entity entity)
        {
            storedEntity = entity;
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