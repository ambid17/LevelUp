using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ExplosionEffect", menuName = "ScriptableObjects/Effects/ExplosionEffect", order = 1)]
    [Serializable]
    public class ExplosionEffect : AoeEffect
    {
        public Effect onShootEffect;

        public override void ApplyOverrides(EffectOverrides overrides)
        {
            base.ApplyOverrides(overrides);

            overrides.applicationChance *= chanceToPlace;
            onShootEffect.ApplyOverrides(overrides);
        }

        public override void OnCraft(Entity target)
        {
            base.OnCraft(target);
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                target.Stats.combatStats.projectileWeaponStats.OnShootEffects.Add(onShootEffect);
            }
            else if (_upgradeCategory == UpgradeCategory.Melee)
            {
                target.Stats.combatStats.meleeWeaponStats.OnShootEffects.Add(onShootEffect);
            }
        }
    }
}