using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// Ex: +10% damage to enemies >60% hp
    /// </summary>
    [CreateAssetMenu(fileName = "DamageHighHealthEffect", menuName = "ScriptableObjects/Fight/Effects/DamageHighHealthEffect", order = 1)]
    [Serializable]
    public class DamageHighHealthEffect : Effect
    {
        [Header("Effect specific")]
        public float perStack;
        public float minHpPercent;

        private float Total => 1 + (perStack * AmountOwned);
        
        private readonly string _description = "Deal {0}% more damage to enemies >{1}% hp";
        public override string GetDescription()
        {
            return string.Format(_description, Total * 100, minHpPercent * 100);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100, minHpPercent * 100);
        } 
        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return 1 + (perStack * newAmountOwned);
        }

        public override void OnCraft(Entity target)
        {
            target.Stats.combatStats.OnHitEffects.Add(this);
        }

        public override void Execute(Entity source, Entity target)
        {
            if (target.Stats.currentHp / target.Stats.maxHp > minHpPercent)
            {
                source.Stats.combatStats.onHitDamage.CompoundingModifiers.Add(Total);
            }
        }
    }
}