using System;

namespace Minigames.Fight
{
    public class StatusEffectInstance : IEquatable<StatusEffectInstance>
    {
        public Entity source;
        public Entity target;
        public float remainingTime;
        public IStatusEffect effect;

        public StatusEffectInstance(Entity source, Entity target, IStatusEffect effect, float remainingTime = -1f)
        {
            this.source = source;
            this.target = target;
            this.remainingTime = remainingTime;
            this.effect = effect;
        }

        /// <summary>
        /// Creates a class instance to track the remaining time of an affect
        /// This is stored in the entity's stats
        /// </summary>
        public static void Create(Entity source, Entity target, IStatusEffect effect, float remainingTime = -1f)
        {
            StatusEffectInstance instance = new StatusEffectInstance(source, target, effect, remainingTime);
            bool success = target.Stats.AddStatusEffect(instance);
            if (success)
            {
                effect.OnAdd(target);
            }
        }

        public bool OnTick(float delta)
        {
            effect.OnTick();
            remainingTime -= delta;

            return remainingTime <= 0;
        }

        public bool Equals(StatusEffectInstance other)
        {
            if (other == null) return false;
            return effect.Equals(other.effect);
        }
    }
}