using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "DamageEffect", menuName = "ScriptableObjects/Effects/DamageEffect", order = 1)]
    [Serializable]
    public class DamageEffect : Effect
    {
        public float impactPerStack = 1f;
        public float Impact => impactPerStack * _amountOwned;
        public float chanceToApply = 0;

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
                var damage = source.Stats.combatStats.projectileWeaponStats.baseDamage.Calculated * Impact;
                target.TakeHit(damage, source);
            }
        }
    }
}