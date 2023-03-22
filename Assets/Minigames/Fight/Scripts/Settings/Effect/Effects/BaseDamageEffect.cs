using System;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseDamageEffect", menuName = "ScriptableObjects/Fight/Effects/BaseDamageEffect", order = 1)]
    [Serializable]
    public class BaseDamageEffect : Effect
    {
        public float baseDamagePerStack;

        private float Total => baseDamagePerStack * AmountOwned;

        private readonly string _description = "+{0} base damage";
        public override string GetDescription()
        {
            return string.Format(_description, Total);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount));
        } 

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return baseDamagePerStack * newAmountOwned;
        }
        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/weapon/baseDamage";
        public override void Execute(HitData hit)
        {
            hit.BaseDamageAdditions.Add(Total);
        }
    }
}