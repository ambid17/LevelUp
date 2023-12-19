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
        Additive,
        Compounding,
        ManualSet
    }

    [Serializable]
    public class StatModifierEffect : Effect
    {
        [Header("Stat Modifier Specific")]

        [Tooltip("For Additive effects, 1 is 100%, 1.1 is 110%")]
        public float perStack;
        public virtual StatImpactType statImpactType { get; protected set; }
        public virtual StatImpactType impactCalculationType { get; protected set; }

        /// <summary>
        /// Handles how much a stat is impacted by an effect
        /// - Flat ex.:         +5 damage per upgrade
        /// - Additive:         +10% damage (additive) per upgrade
        /// - Compounding:      +10% damage (compounding) per upgrade
        /// - Manual set:       set to 5 move speed
        /// </summary>
        private float Impact
        {
            get
            {
                if(impactCalculationType == StatImpactType.Flat || impactCalculationType == StatImpactType.Additive)
                {
                    return perStack * _amountOwned;
                }
                else if(impactCalculationType == StatImpactType.Compounding)
                {
                    return Mathf.Pow(perStack, _amountOwned);
                }
                else
                {
                    return perStack;
                }
            }
        }

        private readonly string _description = "Adds {0} base damage per stack";
        public override string GetDescription()
        {
            return string.Format(_description, Impact);
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
            if (statImpactType == StatImpactType.Additive)
            {
                return stat + Impact;
            }
            else if (statImpactType == StatImpactType.Compounding)
            {
                return stat * Impact;
            }
            else if (statImpactType == StatImpactType.ManualSet)
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
