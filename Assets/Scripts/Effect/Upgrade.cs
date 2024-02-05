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

    [CreateAssetMenu(fileName = "Upgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
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
        public bool IsEquipped;

        [Header("Effect Tree info")]
        public UpgradeCategory UpgradeCategory;
        public EffectCategory EffectCategory;
        public TierCategory TierCategory;

        public EffectUpgradeContainer parent;
        public EffectUpgradeContainer positive;
        public EffectUpgradeContainer negative;

        public void Init()
        {
            parent.Init();
            positive.Init();
            negative.Init();
            ParentEffect parentEffect = parent.effect as ParentEffect;
            parentEffect.positive = positive.effect;
            parentEffect.negative = negative.effect;
        }

        public void SetDefaults()
        {
            IsUnlocked = false;
            AmountOwned = 0;
            IsCrafted = false;
            IsEquipped = false;

            parent.SetDefaults();
            positive.SetDefaults();
            negative.SetDefaults();
        }

        public void Unlock()
        {
            IsUnlocked = true;
        }

        public void BuyUpgrade() 
        {
            AmountOwned++;

            if (IsCrafted)
            {
                // Update the effect with the new AmountOwned
                Craft();
            }
        }

        public void Craft()
        {
            IsCrafted = true;
            parent.Craft(this);
        }

        public void ToggleEquip(bool isEquipped)
        {
            IsEquipped = isEquipped;
            parent.ToggleEquip(this, IsEquipped);
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

        // TODO: make custom property drawer for real properties
        private string effectOverrideTips => effect.GetOverrideTips();
        private Effect initialEffect;

        public void Init()
        {
            if(effect == null)
            {
                return;
            }

            initialEffect = effect;
            // Create an instance of an effect so the actual scriptable object doesn't get overwritten
            effect = (Effect)ScriptableObject.CreateInstance(effect.GetType().Name);
            effect.ApplyOverrides(overrides);
        }

        public void SetDefaults()
        {
            effect = initialEffect;
        }

        public void Craft(Upgrade upgrade)
        {
            if (effect == null)
            {
                return;
            }
            effect.GiveUpgradeInfo(upgrade.AmountOwned, upgrade.UpgradeCategory, upgrade.EffectCategory);
            effect.OnCraft(GameManager.PlayerEntity);
        }

        public void ToggleEquip(Upgrade upgrade, bool isEquipped)
        {
            if (effect == null)
            {
                return;
            }
            effect.GiveUpgradeInfo(upgrade.AmountOwned, upgrade.UpgradeCategory, upgrade.EffectCategory);
            effect.ToggleEquip(GameManager.PlayerEntity, isEquipped);
        }
    }

    [Serializable]
    public class EffectOverrides
    {
        public float impactPerStack;
        public StatImpactType impactType;

        public float chanceToBackfire;
        public float maxRange;

        [Header("Stats/On hit effects")]
        public float applicationChance;

        [Header("Status effects only")]
        public float duration;
        public float tickRate;
    }
}