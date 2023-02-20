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

            if (remainingTime <= 0)
            {
                return true;
                effect.OnRemove(target);
                target.Stats.StatusEffects.Remove(this);
            }

            return false;
        }

        public bool Equals(StatusEffectInstance other)
        {
            return effect.Equals(other.effect);
        }
    }
}