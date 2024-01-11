using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSpeedEffect", menuName = "ScriptableObjects/Effects/Weapon/ProjectileSpeedEffect", order = 1)]
    [Serializable]
    public class ProjectileSpeedEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.projectileMoveSpeed;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.projectileMoveSpeed;
            }
        }

        public override string GetStatName()
        {
            return "Projectile Speed";
        }
    }
}