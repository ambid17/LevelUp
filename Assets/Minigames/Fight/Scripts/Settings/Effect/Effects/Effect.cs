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
        OnPurchase, // Typically used to unlock something
        OnTimer
    }

    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Fight/Effect", order = 1)]
    [Serializable]
    public abstract class Effect : ScriptableObject, IEquatable<Effect>
    {
        public string Name;
        public string Description;
        public Sprite Icon;
        public int AmountOwned;
        public int ExecutionOrder;
        public abstract EffectTriggerType TriggerType { get; }
        public int DropWeight = 1;

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name);
        }

        public virtual void Execute(HitData hit)
        {
            
        }
    }
}