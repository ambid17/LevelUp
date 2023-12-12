using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ExecuteEffect", menuName = "ScriptableObjects/Fight/Effects/ExecuteEffect", order = 1)]
    [Serializable]
    public class ExecuteDamageEffect : Effect
    {
        [Header("Effect specific")]
        public float perStack;
        public float executePercent;

        private float Impact => 1 + (perStack * _amountOwned);
        private readonly string _description = "Deal {0}% more damage to enemies <{1}% hp";

        public override string GetDescription()
        {
            return string.Format(_description, Impact * 100, executePercent * 100);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100, executePercent * 100);
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = _amountOwned + purchaseCount;
            return 1 + (perStack * newAmountOwned);
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnHitEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnHitEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            if (target.Stats.combatStats.currentHp / target.Stats.combatStats.maxHp.Calculated < executePercent)
            {
               // source.Stats.combatStats.onHitDamage.AddEffect(this);
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * Impact;
        }
    }
}