using Minigames.Fight;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    // TODO handle types other than float
    public class ModifiableStat
    {
        [SerializeField]
        [JsonProperty]
        private float baseValue;
        public float BaseValue => baseValue;

        [field: SerializeField, ReadOnlyField]
        private float calculated { get; set; }
        [JsonIgnore]
        public float Calculated
        {
            get
            {
                var finalCalculation = calculated;
                foreach(var effect in percentChanceEffects)
                {
                    effect.ImpactStat(finalCalculation);
                }

                foreach (var effect in singleUseEffects)
                {
                    effect.ImpactStat(finalCalculation);
                }
                singleUseEffects.Clear();
                return finalCalculation;
            }
        }

        public List<IImpactsStat> flatEffects;
        public List<IImpactsStat> additiveEffects;
        public List<IImpactsStat> compoundingEffects;
        [JsonIgnore]
        public List<IImpactsStat> singleUseEffects;
        [JsonIgnore]
        public List<IImpactsStat> percentChanceEffects;
        [JsonIgnore]
        public List<StatusEffectData> statusEffects;
        // If this effect is set, it will override all other effects and negate them
        public IImpactsStat overrideEffect;

        //public List<Effect> OnKillEffects = new();
        //public List<Effect> OnDeathEffects = new();
        //public List<Effect> OnTakeDamageEffects = new();
        //public List<Effect> OnTimerEffects = new();
        //public List<Effect> OnPurchaseEffects = new();

        public void Init()
        {
            if (flatEffects == null)
            {
                flatEffects = new();
            }

            if (additiveEffects == null)
            {
                additiveEffects = new();
            }

            if (compoundingEffects == null)
            {
                compoundingEffects = new();
            }

            if (singleUseEffects == null)
            {
                singleUseEffects = new();
            }

            if (statusEffects == null)
            {
                statusEffects = new();
            }

            if(percentChanceEffects == null)
            { 
                percentChanceEffects = new(); 
            }

            RecalculateStat();
        }

        /// <summary>
        /// When adding the effect for the first time, it gets populated in the list.
        /// When updating an effect, the Impact is recalculated using AmountOwned, so we just ignore adding it to the list again
        /// </summary>
        /// <param name="effect"></param>
        public void AddOrUpdateStatEffect(IImpactsStat effect)
        {
            if (effect.statImpactType == StatImpactType.Flat && !flatEffects.Contains(effect))
            {
                flatEffects.Add(effect);
            }
            else if (effect.statImpactType == StatImpactType.Additive && !additiveEffects.Contains(effect))
            {
                additiveEffects.Add(effect);
            }
            else if (effect.statImpactType == StatImpactType.Compounding && !compoundingEffects.Contains(effect))
            {
                compoundingEffects.Add(effect);
            }

            RecalculateStat();
        }

        /// <summary>
        /// Currently not fully implemented
        /// </summary>
        /// <param name="effect"></param>
        public void AddSingleUseEffect(IImpactsStat effect)
        {
            singleUseEffects.Add(effect);
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            StatusEffectData existing = null;

            foreach(var statusEffectData in statusEffects)
            {
                if (statusEffectData.statusEffect.GetType() == statusEffect.GetType())
                {
                    existing = statusEffectData;
                }
            }
            if (existing != null)
            {
                existing.Reapply();
            }
            else
            {
                var newStatusEffect = new StatusEffectData(statusEffect, source, target);
                statusEffects.Add(newStatusEffect);
                RecalculateStat();
            }
        }

        public void RecalculateStat()
        {
            // Allows the designer to set a hard override for any stat with an effect
            if (overrideEffect != null)
            {
                calculated = overrideEffect.ImpactStat(calculated);
                return;
            }

            calculated = baseValue;

            foreach (var effect in flatEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in additiveEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in compoundingEffects)
            {
                calculated = effect.ImpactStat(calculated);
            }

            foreach (var effect in statusEffects)
            {
                calculated = effect.statusEffect.ImpactStat(calculated);
            }
        }

        public void Clear()
        {
            calculated = 0;
        }

        public void TickStatuses()
        {
            foreach (var status in statusEffects.ToList())
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    statusEffects.Remove(status);
                    RecalculateStat();
                }
            }
        }

        /// <summary>
        /// Only used for enemies to randomize their move speed so they don't clump up
        /// </summary>
        /// <param name="random"></param>
        public void Randomize(float random)
        {
            calculated *= random;
        }
    }
}