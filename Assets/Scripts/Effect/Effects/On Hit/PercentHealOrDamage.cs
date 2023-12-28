using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "PercentHealOrDamage", menuName = "ScriptableObjects/Effects/PercentHealOrDamage", order = 1)]
    [Serializable]
    public class PercentHealOrDamage : Effect
    {
        [Header("Effect specific")]
        public float chanceToApply = 0;
        public float impactPerStack = 1f;
        public float Impact => impactPerStack * _amountOwned;


        public override void ApplyOverrides(EffectOverrides overrides)
        {
            chanceToApply = overrides.applicationChance;
            impactPerStack = overrides.impactPerStack;
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
            bool doesApply = UnityEngine.Random.value < chanceToApply;
            if (doesApply)
            {
                bool hurtsSource = UnityEngine.Random.value < 0.5f;
                var damage = source.Stats.combatStats.projectileWeaponStats.baseDamage.Calculated * Impact;

                if (hurtsSource)
                {
                    // clamp the damage to the source, so that we cannot kill ourselves with this effect
                    float sourceDamage = Mathf.Clamp(damage, 0, (source.Stats.combatStats.currentHp - 1));
                    source.TakeHit(sourceDamage, source);
                    target.TakeHeal(damage, source);
                }
                else
                {
                    target.TakeHit(damage, source);
                    source.TakeHeal(damage, source);
                }
            }
        }
    }
}