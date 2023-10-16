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
        public float percentDamagePerStack;
        public float executePercent;

        private float Total => 1 + (percentDamagePerStack * AmountOwned);
        private readonly string _description = "Deal {0}% more damage to enemies <{1}% hp";

        public override string GetDescription()
        {
            return string.Format(_description, Total * 100, executePercent * 100);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100, executePercent * 100);
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return 1 + (percentDamagePerStack * newAmountOwned);
        }

        public override void Execute(HitData hit)
        {
            if (hit.Target.Stats.currentHp / hit.Target.Stats.maxHp < executePercent)
            {
                hit.BaseDamageMultipliers.Add(Total);
            }
        }
    }
}