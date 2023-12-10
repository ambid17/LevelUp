using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "BaseMoveReductionEffect", menuName = "ScriptableObjects/Fight/Effects/BaseMoveReductionEffect", order = 1)]
    [Serializable]
    public class BaseMoveReductionEffect : Effect
    {
        [Header("Effect specific")]
        public float perStack;

        private float Total => -(perStack * AmountOwned);

        private readonly string _description = "Slows player move speed by {0}%";
        public override string GetDescription()
        {
            return string.Format(_description, Total * 100);
        }
        public override string GetNextUpgradeDescription(int purchaseCount)
        {
            return string.Format(_description, NextUpgradeChance(purchaseCount) * 100);
        }

        private float NextUpgradeChance(int purchaseCount)
        {
            int newAmountOwned = AmountOwned + purchaseCount;
            return 1 + (perStack * newAmountOwned);
        }

        public override void Apply(Entity target)
        {
            target.Stats.movementStats.moveSpeed.BaseModifiers.Add(Total);
        }

        public override void Execute(Entity target, Entity source)
        {

        }
    }
}