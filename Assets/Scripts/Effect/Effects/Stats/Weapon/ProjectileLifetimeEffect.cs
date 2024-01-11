using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileLifetimeEffect", menuName = "ScriptableObjects/Effects/Weapon/ProjectileLifetimeEffect", order = 1)]
    [Serializable]
    public class ProjectileLifetimeEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.projectileLifeTime;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.projectileLifeTime;
            }
        }

        public override string GetStatName()
        {
            return "Projectile Lifetime";
        }
    }
}