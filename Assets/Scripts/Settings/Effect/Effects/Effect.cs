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
    public class Effect : ScriptableObject, IEquatable<Effect>
    {
        [Header("Trigger info")]
        public int ExecutionOrder;
        public EffectTriggerType TriggerType;

        [Header("Set at runtime")]
        public int AmountOwned;

        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public virtual string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Empty;
        }

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return other.GetType().Equals(this.GetType());
        }

        public virtual void SetAmountOwned(int amountOwned)
        {
            AmountOwned = amountOwned;
        }

        public virtual void OnCraft(Entity target)
        {

        }

        public virtual void Execute(Entity source, Entity target)
        {

        }
    }
}