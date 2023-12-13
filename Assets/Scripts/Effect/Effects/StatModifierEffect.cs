using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Fight
{
    public enum StatImpactType
    {
        Flat,
        Compounding,
        ManualSet
    }

    [Serializable]
    public class StatModifierEffect : Effect
    {
        [Header("Effect specific")]
        public float perStack;
        public virtual StatImpactType statImpactType { get; protected set; }

        private float Impact => perStack * _amountOwned;

        private readonly string _description = "Adds {0} base damage per stack";
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
            int newAmountOwned = _amountOwned + purchaseCount;
            return perStack * newAmountOwned;
        }

        public override void OnCraft(Entity entity)
        {
            var statToAffect = GetStatToAffect(entity);
            statToAffect.AddEffect(this);
        }

        /// <summary>
        /// Asks the child class what stat it would like to impact
        /// </summary>
        public virtual ModifiableStat GetStatToAffect(Entity entity)
        {
            throw new NotImplementedException();
        }

        public override float ImpactStat(float stat)
        {
            if (statImpactType == StatImpactType.Flat)
            {
                return stat + Impact;
            }
            else if(statImpactType == StatImpactType.Compounding)
            {
                return stat * Impact;
            }else if(statImpactType == StatImpactType.ManualSet)
            {
                return Impact;
            }

            return stat;
        }

        public virtual bool CanImpactStat(float stat, Entity source, Entity target)
        {
            return true;
        }
    }
}
