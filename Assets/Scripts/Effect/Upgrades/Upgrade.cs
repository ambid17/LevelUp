using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public enum UpgradeCategory
    {
        None,
        Melee,
        Range, 
        Player
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

    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Fight/Upgrade", order = 1)]
    [Serializable]
    public class Upgrade : ScriptableObject
    {
        public string Name;
        public Sprite Icon;

        [Header("Unlock info")]
        public bool IsUnlocked;
        public int AmountOwned;
        public int MaxAmountOwned;

        [Header("Craft info")]
        public bool IsCrafted;

        [Header("Effect Tree info")]
        public UpgradeCategory UpgradeCategory;
        public EffectCategory EffectCategory;
        public TierCategory TierCategory;

        public EffectUpgradeContainer positive;
        public EffectUpgradeContainer negative;

        public virtual void Unlock()
        {
            IsUnlocked = true;
        }

        public virtual void BuyUpgrade() 
        {
            AmountOwned++;

            if (IsCrafted)
            {
                // Update the effect with the new AmountOwned
                Craft();
            }
        }

        public virtual void Craft()
        {
            IsCrafted = true;
            positive.Craft(this);
            negative.Craft(this);
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
    }


    [Serializable]
    public class EffectUpgradeContainer
    {
        public Effect effect;
        public EffectOverrides overrides;
        public void Craft(Upgrade upgrade)
        {
            effect.GiveUpgradeInfo(upgrade.AmountOwned, upgrade.UpgradeCategory, upgrade.EffectCategory);
            effect.OnCraft(GameManager.PlayerEntity);
            effect.OverrideStats(overrides);
        }
    }

    public class EffectOverrides
    {
        public float impactPerStat;
    }
}