using System;
using System.Collections;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum EffectTriggerType
    {
        OnHit,
        OnKill,
        OnDeath,
        OnTakeDamage,
        OnCraft, 
        OnTimer
    }

    [Serializable]
    public abstract class Effect : ScriptableObject, IEquatable<Effect>
    {
        public abstract string GetDescription();
        public abstract string GetNextUpgradeDescription(int purchaseCount);

        [Header("Trigger info")]
        public int ExecutionOrder;
        public EffectTriggerType TriggerType;

        [Header("Set at runtime")]
        public int AmountOwned;

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return other.GetType().Equals(this.GetType());
        }

        public abstract void Apply(Entity target);
        public abstract void Execute(Entity source, Entity target);
    }

    public class TimerEffectData
    {
        public float timer;
        public float tickRate;
        public Effect myEffect;

        public Entity source;
        public Entity target;

        public void OnTick()
        {
            timer += Time.deltaTime;
            if (timer >= tickRate)
            {
                myEffect.Execute(source, target);
                timer = 0;
            }
        }
    }
    public class StatusEffectData
    {
        public float duration;
        public float timer;

        public Entity source;
        public Entity effectedEntity;

        // Status effect as subclass of effect with it's own tick method?
        // public StatusEffect myEffect;

        public void OnTick()
        {
            // myEffect.Tick(effectedEntity);
            timer += Time.deltaTime;
            if (timer >= duration)
            {
               // effectedEntity.AppliedStatusEffects.Remove(this);
            }
        }

        public void Reapply()
        {
            // TODO figure out how the fuck to diferentiate status effects
            // that do or don't reapply on tick.
        }
    }
}