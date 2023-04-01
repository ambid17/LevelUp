using System;

namespace Minigames.Fight
{
    public class StatusEffectInstance : IEquatable<StatusEffectInstance>
    {
        public Entity source;
        public Entity target;
        public float remainingTime;
        public float timeSinceLastExecution;
        public IStatusEffect effect;

        public StatusEffectInstance(Entity source, Entity target, IStatusEffect effect)
        {
            this.source = source;
            this.target = target;
            remainingTime = effect.Duration;
            this.effect = effect;
        }

        /// <summary>
        /// Creates a class instance to track the remaining time of an affect
        /// This is stored in the entity's stats
        /// </summary>
        public static void Create(Entity source, Entity target, IStatusEffect effect)
        {
            StatusEffectInstance instance = new StatusEffectInstance(source, target, effect);
            // Only apply the affect if it isn't already applied
            bool canApply = target.Stats.AddStatusEffect(instance);
            if (canApply)
            {
                effect.ApplyEffect(target);
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