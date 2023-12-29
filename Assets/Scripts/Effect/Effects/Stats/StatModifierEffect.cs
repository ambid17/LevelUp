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
    public class StatModifierEffect : Effect, IImpactsStat
    {
        [Header("Stat Modifier Specific")]

        [Tooltip("For Additive/Compounding effects, 1 is 100%, 1.1 is 110%")]
        public float impactPerStack;
        public virtual StatImpactType statImpactType { get; protected set; }

        /// <summary>
        /// Handles how much a stat is impacted by an effect
        /// - Flat ex.:         +5 damage per upgrade
        /// - Additive:         +10% damage (additive) per upgrade
        /// - Compounding:      +10% damage (compounding) per upgrade
        /// - Manual set:       set to 5 move speed
        /// </summary>
        protected virtual float Impact
        {
            get
            {
                if (statImpactType == StatImpactType.Flat || statImpactType == StatImpactType.Additive)
                {
                    return impactPerStack * _amountOwned;
                }
                else if (statImpactType == StatImpactType.Compounding)
                {
                    return Mathf.Pow(impactPerStack, _amountOwned);
                }
                else
                {
                    return impactPerStack;
                }
            }
        }

        private readonly string _description = "{0}{1} {2} per stack {3}";
        public override string GetDescription()
        {
            string raisesOrLowers = "+";
            switch (statImpactType)
            {
                case StatImpactType.Flat:
                    if (impactPerStack < 0)
                    {
                        raisesOrLowers = "-";
                    }
                    break;
                case StatImpactType.Additive:
                case StatImpactType.Compounding:
                    if(impactPerStack < 1)
                    {
                        raisesOrLowers = "-";
                    }
                    break;
                default:
                    raisesOrLowers = "+";
                    break;
            }

            string impactString = impactPerStack.ToString();
            if(statImpactType == StatImpactType.Additive || statImpactType == StatImpactType.Compounding)
            {
                // convert from 1.1 -> 0.1 -> 10%
                // or from 0.9 -> -10%
                float impact = impactPerStack;
                if(impactPerStack > 1)
                {
                    impact -= 1;
                }
                else
                {
                    impact = Mathf.Abs(1 - impact);
                }
                impactString = $"{impact * 100}%";
            }

            string impactTypeString = string.Empty;
            if(statImpactType == StatImpactType.Additive || statImpactType == StatImpactType.Compounding)
            {
                impactTypeString = $"({statImpactType})";
            }
            return string.Format(_description, raisesOrLowers, impactString, GetStatName(), impactTypeString);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            impactPerStack = overrides.impactPerStack;
            statImpactType = overrides.impactType;
        }

        public override void OnCraft(Entity entity)
        {
            var statToAffect = GetStatToAffect(entity);
            statToAffect.AddOrUpdateStatEffect(this);
        }

        /// <summary>
        /// Asks the child class what stat it would like to impact
        /// </summary>
        public virtual ModifiableStat GetStatToAffect(Entity entity)
        {
            throw new NotImplementedException();
        }

        public virtual string GetStatName()
        {
            return string.Empty;
        }

        public override float ImpactStat(float stat)
        {
            if (statImpactType == StatImpactType.Flat)
            {
                return stat + Impact;
            }
            else if (statImpactType == StatImpactType.Compounding || statImpactType == StatImpactType.Additive)
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
