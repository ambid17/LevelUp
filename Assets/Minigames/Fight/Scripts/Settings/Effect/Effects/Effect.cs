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
        public abstract string Description { get; }
        public abstract string NextUpgradeDescription { get; }
        public Sprite Icon;
        public int AmountOwned;
        public int MaxAmountOwned;
        public int ExecutionOrder;
        public abstract EffectTriggerType TriggerType { get; }
        public int DropWeight = 1;
        public abstract string UpgradePath { get; }

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name);
        }

        public abstract void Execute(HitData hit);

        public abstract void Unlock(EffectSettings settings);
        
        public string GetUpgradeCountText()
        {
            string upgradeCountText = $"{AmountOwned}";

            if (MaxAmountOwned > 0)
            {
                upgradeCountText += $" / {MaxAmountOwned}";
            }
            
            return $"({upgradeCountText})";
        }

        public abstract float GetCost(int purchaseCount);


    }
}