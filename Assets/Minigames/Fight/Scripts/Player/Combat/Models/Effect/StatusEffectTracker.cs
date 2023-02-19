namespace Minigames.Fight
{
    public class StatusEffectTracker
    {
        public Entity source;
        public Entity target;
        public float remainingTime;
        public IStatusEffect effect;

        public StatusEffectTracker(Entity source, Entity target, IStatusEffect effect, float remainingTime = -1f)
        {
            this.source = source;
            this.target = target;
            this.remainingTime = remainingTime;
            this.effect = effect;
        }

        public static void AddTracker(Entity source, Entity target, IStatusEffect effect, float remainingTime = -1f)
        {
            StatusEffectTracker tracker = new StatusEffectTracker(source, target, effect, remainingTime);
            target.Stats.StatusEffects.Add(tracker);
            effect.OnAdd(target);
        }

        public void OnTick(float delta)
        {
            effect.OnTick();
            remainingTime -= delta;

            if (remainingTime <= 0)
            {
                effect.OnRemove(target);
                target.Stats.StatusEffects.Remove(this);
            }
        }
    }
}