using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "HealOrPoison", menuName = "ScriptableObjects/Effects/HealOrPoison", order = 1)]
    [Serializable]
    public class HealOrPoison : Effect, IImpactsStat
    {
        public DamageOverTimeEffect poisonEffect;
        public float poisonDuration = 3;
        public float poisonTickRate = 1;
        public float chanceToApply = 0.5f;
        public float chanceToBackfire = 0.5f;

        public float damagePerStack = 0.05f;
        public float Damage => damagePerStack * _amountOwned;

        private StatImpactType impactType = StatImpactType.Additive;
        public StatImpactType statImpactType => impactType;

        private readonly string _description = "{0}% chance to apply. When applied deal {1}% additional damage. {2}% chance to heal or poison self for the extra damage";

        public override string GetDescription()
        {
            return string.Format(_description, chanceToApply * 100, damagePerStack * 100, chanceToBackfire * 100);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chanceToApply = overrides.applicationChance;
            damagePerStack = overrides.impactPerStack;
            chanceToBackfire = overrides.chanceToBackfire;
            poisonDuration = overrides.duration;
            poisonTickRate = overrides.tickRate;
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
                // apply extra on hit damage
                if (_upgradeCategory == UpgradeCategory.Range)
                {
                    source.Stats.combatStats.projectileWeaponStats.onHitDamage.AddSingleUseEffect(this);
                }
                else
                {
                    source.Stats.combatStats.meleeWeaponStats.onHitDamage.AddSingleUseEffect(this);
                }

                bool doesBackfire = Random.value < chanceToBackfire;

                // poison the source, dealing the extra on hit damage as a DoT
                if (doesBackfire)
                {
                    var poisonInstance = (DamageOverTimeEffect)ScriptableObject.CreateInstance(poisonEffect.GetType().Name);
                    poisonInstance.ApplyOverrides(new EffectOverrides
                    {
                        applicationChance = 1,
                        chanceToBackfire = 1,
                        impactPerStack = Damage / poisonDuration, // deals the extra damage over X seconds
                        tickRate = 1,
                        duration = poisonDuration

                    });

                    poisonInstance.Execute(source, source);
                }
                // Heal the source
                else
                {
                    target.Stats.combatStats.AddHp(Damage);
                }
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * Damage;
        }
    }
}
