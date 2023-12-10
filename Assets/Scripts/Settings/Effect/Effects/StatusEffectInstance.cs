using System;
using UnityEngine;

namespace Minigames.Fight
{
    public class StatusEffectInstance : IEquatable<StatusEffectInstance>
    {
        [Header("Effect specific")]
        public Entity source;
        public Entity target;
        public float remainingTime;
        public float timeSinceLastExecution;
        public IStatusEffect effect;

        public StatusEffectInstance(HitData hit, IStatusEffect effect)
        {
            this.source = hit.Source;
            this.target = hit.Target;
            remainingTime = effect.Duration;
            this.effect = effect;
        }

        /// <summary>
        /// Creates a class instance to track the remaining time of an affect
        /// This is stored in the entity's stats
        /// </summary>
        public static void Create(HitData hit, IStatusEffect effect)
        {
            StatusEffectInstance instance = new StatusEffectInstance(hit, effect);
            // Only apply the affect if it isn't already applied
            bool canApply = hit.Target.Stats.AddStatusEffect(instance);
            if (canApply)
            {
                effect.ApplyEffect(hit.Target);
            }
        }

        public void OnTick(float delta)
        {
            timeSinceLastExecution += delta;

            if (timeSinceLastExecution > effect.TickRate)
            {
                effect.OnTick(target);
            }
            
            remainingTime -= delta;
            if (remainingTime <= 0)
            {
                effect.RemoveEffect(target);
                target.Stats.StatusEffects.Remove(this);
            }
        }

        public bool Equals(StatusEffectInstance other)
        {
            if (other == null) return false;
            return effect.Equals(other.effect);
        }
    }
}