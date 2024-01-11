using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ExplodeAndBurnOnKill", menuName = "ScriptableObjects/Effects/OnKill/ExplodeAndBurnOnKill", order = 1)]
    [Serializable]
    public class ExplodeAndBurnOnKill : Effect
    {
        public DamageOverTimeEffect burnEffect;
        [Header("overrides.duration")]
        public float burnDuration = 3;
        [Header("overrides.tickRate")]
        public float burnTickRate = 1;
        [Header("overrides.maxRange")]
        public float explosionRadius = 1;
        [Header("overrides.applicationChance")]
        public float chanceToApply = 0.5f;
        [Header("overrides.chanceToBackfire")]
        public float chanceToBurn = 0.05f;

        public float damagePerStack = 0.05f;
        public float Damage => damagePerStack * _amountOwned;

        private readonly string _description = "{0}% chance on kill to apply. When applied deal {1}% of weapon damage. {2}% chance to burn each nearby enemy for {1}% of damage {3} seconds";

        public override string GetDescription()
        {
            return string.Format(_description, chanceToApply * 100, damagePerStack * 100, chanceToBurn * 100, burnDuration);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chanceToApply = overrides.applicationChance;
            damagePerStack = overrides.impactPerStack;
            chanceToBurn = overrides.chanceToBackfire;
            burnDuration = overrides.duration;
            burnTickRate = overrides.tickRate;
            explosionRadius = overrides.maxRange;
        }


        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
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
            bool doesApply = Random.value < chanceToApply;
            if (doesApply)
            {
                List<Entity> entitiesInMaxRange = EffectUtils.GetEntitiesInRange(target, explosionRadius, true, true);

                var weaponStats = source.Stats.combatStats.projectileWeaponStats;
                if (_upgradeCategory == UpgradeCategory.Melee)
                {
                    weaponStats = target.Stats.combatStats.meleeWeaponStats;
                }

                foreach (var entity in entitiesInMaxRange)
                {
                    entity.Stats.combatStats.TakeDamage(weaponStats.baseDamage.Calculated * Damage);

                    bool doesBurn = Random.value < chanceToBurn;

                    if (doesBurn)
                    {
                        entity.Stats.combatStats.AddOrRefreshStatusEffect(burnEffect, source, entity);
                    }
                }
            }
        }
    }
}