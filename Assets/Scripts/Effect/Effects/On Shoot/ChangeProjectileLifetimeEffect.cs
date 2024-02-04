using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ChangeProjectileLifetimeEffect", menuName = "ScriptableObjects/Effects/ChangeProjectileLifetimeEffect", order = 1)]
    [Serializable]
    public class ChangeProjectileLifetimeEffect : Effect
    {
        public StatModifierEffect statModifierEffect;
        public float chanceToApply;

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            base.ApplyOverrides(overrides);
        }


        public override void Execute(Entity source, Entity target)
        {
            bool doesApply = Random.value < chanceToApply;
            if (doesApply)
            {
                if (_upgradeCategory == UpgradeCategory.Melee)
                {
                    source.Stats.combatStats.meleeWeaponStats.projectileLifeTime.AddSingleUseEffect(statModifierEffect);
                }
                else if (_upgradeCategory == UpgradeCategory.Range)
                {
                    source.Stats.combatStats.projectileWeaponStats.projectileLifeTime.AddSingleUseEffect(statModifierEffect);
                }
            }
                
        }
    }
}
