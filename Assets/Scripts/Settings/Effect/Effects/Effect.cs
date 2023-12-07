using System;
using System.Collections;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum EffectCostType{
        Additive,
        Exponential
    }
    public enum EffectTriggerType
    {
        OnHit,
        OnKill,
        OnDeath,
        OnTakeDamage,
        OnPurchase, 
        OnTimer
    }

    public enum Category
    {
        UpgradeCategory,
        EffectCategory,
        TierCategory,
        Name,
    }

    public enum UpgradeCategory
    {
        None, Melee, Range, Player
    }

    public enum EffectCategory
    {
        None,
        AoE,
        OnHit,
        Physical
    }

    public enum TierCategory
    {
        None,
        Tier1,
        Tier2,
        Tier3
    }

    [CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/Fight/Effect", order = 1)]
    [Serializable]
    public abstract class Effect : ScriptableObject, IEquatable<Effect>
    {
        public string Name;
        public abstract string GetDescription();
        public abstract string GetNextUpgradeDescription(int purchaseCount);
        
        public Sprite Icon;

        public bool IsUnlocked;
        public int AmountOwned;
        public int MaxAmountOwned;
        public float BaseCost;
        public float CostScalar;
        public EffectCostType CostType;
        
        public int ExecutionOrder;
        public EffectTriggerType TriggerType;
        public UpgradeCategory UpgradeCategory;
        public EffectCategory EffectCategory;
        public TierCategory TierCategory;
        
        public int DropWeight = 1;
        public string UpgradePath => $"upgrades/{UpgradeCategory}/{EffectCategory}/{TierCategory}/{Name}";

        public bool Equals(Effect other)
        {
            if (other == null) return false;
            return Name.Equals(other.Name);
        }

        public abstract void Execute(HitData hit);

        /// <summary>
        /// Effects are unlocked during end of country/world rewards
        /// You can get new effects which should unlock them.
        /// If you get an effect you already own, it grants a free purchase of it
        /// </summary>
        /// <param name="settings"></param>
        public virtual void Unlock(EffectSettings settings)
        {
            if (!IsUnlocked)
            {
                IsUnlocked = true;
                switch (TriggerType)
                {
                    case EffectTriggerType.OnHit:
                        settings.OnHitEffects.Add(this);
                        break;
                }
            }
            else
            {
                AmountOwned++;
            }
        }
        
        public string GetUpgradeCountText()
        {
            string upgradeCountText = $"{AmountOwned}";

            if (MaxAmountOwned > 0)
            {
                upgradeCountText += $" / {MaxAmountOwned}";
            }
            
            return $"({upgradeCountText})";
        }

        public virtual float GetCost(int purchaseCount)
        {
            switch (CostType)
            {
                case EffectCostType.Additive:
                    return GetAdditiveCost(purchaseCount);
                case EffectCostType.Exponential:
                    return GetExponentialCost(purchaseCount);
                default:
                    return float.MaxValue;
            }
        }
        
        // example:
        // base cost = 10, scalar = 1
        // 10, 11, 12, 13, 14
        private float GetAdditiveCost(int purchaseCount)
        {
            float totalCost = 0;
            for (int currentNumPurchased = AmountOwned; currentNumPurchased < AmountOwned + purchaseCount; currentNumPurchased++)
            {
                totalCost += BaseCost + (CostScalar * currentNumPurchased);
            }
            
            return totalCost;
        }

        // example:
        // base cost = 100, scalar (percentage) = 0.5;
        // 100, 150, 225
        private float GetExponentialCost(int purchaseCount)
        {
            float totalCost = 0;
            for (int currentNumPurchased = AmountOwned; currentNumPurchased < AmountOwned + purchaseCount; currentNumPurchased++)
            {
                totalCost += BaseCost * Mathf.Pow(CostScalar, currentNumPurchased);
            }
            
            return totalCost;
        }
        
        public virtual void Purchase(int purchaseCount)
        {
            AmountOwned += purchaseCount;
        }

    }
}