namespace Minigames.Fight
{
    public class StatusEffectInstance
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
            target.Stats.StatusEffects.Add(instance);
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