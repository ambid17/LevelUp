using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "FlatArmorPenetrationEffect", menuName = "ScriptableObjects/Fight/Effects/FlatArmorPenetrationEffect", order = 1)]
    [Serializable]
    public class FlatArmorPenetrationEffect : Effect
    {
        public float flatPenPerStack;
        private float Total => flatPenPerStack * AmountOwned;
        
        private readonly string _description = "Grants +{0} armor penetration";

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
            return flatPenPerStack * newAmountOwned;
        }
        public override EffectTriggerType TriggerType => EffectTriggerType.OnHit;
        public override string UpgradePath => "upgrades/weapon/flatArmorPen";

        public override void Execute(HitData hit)
        {
            hit.FlatArmorPenetration += Total;
        }
    }
}
