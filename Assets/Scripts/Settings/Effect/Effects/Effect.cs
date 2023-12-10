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
        public abstract void Execute(Entity target, Entity source);
    }
}