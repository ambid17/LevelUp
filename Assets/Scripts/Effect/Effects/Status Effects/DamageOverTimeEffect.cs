using System;
using Minigames.Fight;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "DamageOverTimeEffect", menuName = "ScriptableObjects/Effects/DamageOverTimeEffect", order = 1)]
    [Serializable]
    public class DamageOverTimeEffect : Effect, IStatusEffect
    {
        [Header("Overriden Stats")]
        public float chance = 0;
        public float chanceToBackfire;
        public float duration = 0;
        public float Duration => duration;
        public float tickRate = 1f;
        public float TickRate => tickRate;
        public float damagePerStack = 1f;
        public float HitDamage => damagePerStack * _amountOwned;
        
        private readonly string _description = "{0}% chance to burn enemies for {1} damage each second for {2} seconds";
        
        public override string GetDescription()
        {
            return string.Format(_description, chance * 100, HitDamage, duration);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chance = overrides.applicationChance;
            duration = overrides.duration;
            damagePerStack = overrides.impactPerStack;
            tickRate = overrides.tickRate;
            chanceToBackfire = overrides.chanceToBackfire;
        }

        public override void OnCraft(Entity target)
        {
            if(_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnHitEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnHitEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            bool doesApply = Random.value < chance;
            if (doesApply)
            {
                bool applyToSource = Random.value < chanceToBackfire;
                if (applyToSource)
                {
                    source.Stats.combatStats.AddOrRefreshStatusEffect(this, source, source);
                }
                else
                {
                    target.Stats.combatStats.AddOrRefreshStatusEffect(this, source, target);
                }
            }
        }

        public void OnTick(Entity source, Entity target)
        {
            target.TakeHit(HitDamage, source);
        }

        public void OnComplete()
        {
        }
    }
}