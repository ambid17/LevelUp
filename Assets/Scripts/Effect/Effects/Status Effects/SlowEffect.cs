using System;
using Minigames.Fight;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "SlowEffect", menuName = "ScriptableObjects/Effects/SlowEffect", order = 1)]
    [Serializable]
    public class SlowEffect : Effect, IStatusEffect
    {
        [Header("Effect specific")]
        public float duration = 2f;
        public float Duration => duration;
        private float tickRate = 0;
        public float TickRate => tickRate;

        public float chanceToApply = 0.5f;
        public float chanceToBackfire = 0.5f;
        public float slowPerStack = 0.05f;
        public float SlowAmount => 1 - (slowPerStack * _amountOwned);
        
        private readonly string _description = "{0}% chance to slow Player or Hit target by {1}% per stack for {2} seconds";
        
        public override string GetDescription()
        {
            return string.Format(_description, chanceToBackfire * 100, slowPerStack * 100, duration);
        }

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chanceToApply = overrides.applicationChance;
            slowPerStack = overrides.impactPerStack;
            chanceToBackfire = overrides.chanceToBackfire;
            tickRate = overrides.tickRate;
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
            bool doesSlow = Random.value < chanceToApply;
            if (doesSlow)
            {
                bool doesBackfire = Random.value < chanceToBackfire;

                if (doesBackfire)
                {
                    source.Stats.movementStats.moveSpeed.AddOrRefreshStatusEffect(this, source, source);
                }
                else
                {
                    target.Stats.movementStats.moveSpeed.AddOrRefreshStatusEffect(this, source, target);
                }
            }
        }

        public override float ImpactStat(float stat)
        {
            return stat * SlowAmount;
        }

        public void OnTick(Entity source, Entity target)
        {
        }

        public void OnComplete()
        {
        }
    }
}