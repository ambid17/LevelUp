using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseDamageEffect", menuName = "ScriptableObjects/Fight/Effects/BaseDamageEffect", order = 1)]
    [Serializable]
    public class BaseDamageEffect : Effect
    {
        [Header("Effect specific")]
        public float perStack;

        private float Impact => perStack * AmountOwned;

        private readonly string _description = "Adds {0} base damage";
        public override string GetDescription()
        {
            return string.Format(_description, Impact);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount));
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return perStack * newAmountOwned;
        }

        public override void OnCraft(Entity target)
        {
            target.Stats.combatStats.baseDamage.AddEffect(this);
        }

        public override float ImpactStat(float stat)
        {
            return stat + Impact;
        }
    }
}