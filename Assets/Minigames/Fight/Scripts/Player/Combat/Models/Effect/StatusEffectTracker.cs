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
        
        target.StatusEffects.Add(this);
        effect.OnAdd(target);
    }

    public void OnTick(float delta)
    {
        if (effect is IStatusEffectTick mrTicky)
        {
            mrTicky.Tick(delta, this);
        }
        
        if (remainingTime > -1)
        {
            OnTickEffect(delta);
            remainingTime -= delta;

            if (remainingTime <= 0)
            {
                effect.OnRemove(target);
                target.StatusEffects.Remove(this);
            }
        }
    }

    private void OnTickEffect(float delta)
    {
        
    }
}