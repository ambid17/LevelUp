using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "DamageOnKillStatusEffect", menuName = "ScriptableObjects/Effects/OnKill/DamageOnKillStatusEffect", order = 1)]
    [Serializable]
    public class DamageOnKillStatusEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float duration = 2f;
        public float Duration => duration;
        private float tickRate = 0;
        public float TickRate => tickRate;

        public float damagePerStack = 0.05f;
        public float DamageAmount => 1 + (damagePerStack * _amountOwned);

        private float currentKillCount;
        [Header("Set by override.MaxRange")]
        public float KillsToTrigger;

        private readonly string _description = "{0}% bonus damage for {1} seconds after killing {2} enemies";

        public override string GetDescription()
        {
            return string.Format(_description, damagePerStack * 100, Duration * 100, duration);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            damagePerStack = overrides.impactPerStack;
            duration = overrides.duration;
            tickRate = overrides.tickRate;
            KillsToTrigger = overrides.maxRange;
        }

        public override void OnCraft(Entity target)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnKillEffects.Add(this);
            }
            else
            {
                target.Stats.combatStats.meleeWeaponStats.OnKillEffects.Add(this);
            }
        }

        public override void Execute(Entity source, Entity target)
        {
            currentKillCount++;

            if(currentKillCount >= KillsToTrigger)
            {
                if (_upgradeCategory == UpgradeCategory.Range)
                {
                    target.Stats.combatStats.projectileWeaponStats.baseDamage.AddOrRefreshStatusEffect(this, source, source);
                }
                else
                {
                    target.Stats.combatStats.meleeWeaponStats.baseDamage.AddOrRefreshStatusEffect(this, source, source);
                }
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * DamageAmount;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
        }
    }
}
