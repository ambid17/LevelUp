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

        private float calculated;
        [JsonIgnore]
        public float Calculated
        {
            get
            {
                singleUseEffects.Clear();
                return calculated;
            }
        }

        public List<StatModifierEffect> flatEffects;
        public List<StatModifierEffect> additiveEffects;
        public List<StatModifierEffect> compoundingEffects;
        [JsonIgnore]
        public List<StatModifierEffect> singleUseEffects;
        [JsonIgnore]
        public List<StatusEffectData> statusEffects;
        // If this effect is set, it will override all other effects and negate them
        public StatModifierEffect overrideEffect;

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

            RecalculateStat();
        }

        /// <summary>
        /// When adding the effect for the first time, it gets populated in the list.
        /// When updating an effect, the Impact is recalculated using AmountOwned, so we just ignore adding it to the list again
        /// </summary>
        /// <param name="effect"></param>
        public void AddOrUpdateStatEffect(StatModifierEffect effect)
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

        public void AddSingleUseEffect(StatModifierEffect effect)
        {
            singleUseEffects.Add(effect);
            RecalculateStat();
        }

        public void AddOrRefreshStatusEffect(IStatusEffect statusEffect, Entity source, Entity target)
        {
            var existing = statusEffects.First(e => e.statusEffect.Equals(statusEffect));
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

            foreach (var effect in singleUseEffects)
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
            foreach (var status in statusEffects)
            {
                bool didFinish = status.OnTick();
                if (didFinish)
                {
                    statusEffects.Remove(status);
                    RecalculateStat();
                }
            }
        }

        public void Randomize(float random)
        {
            calculated *= random;
        }
    }
}