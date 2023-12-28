using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSizeEffect", menuName = "ScriptableObjects/Effects/Weapon/ProjectileSizeEffect", order = 1)]
    [Serializable]
    public class ProjectileSizeEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.projectileSize;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.projectileSize;
            }
        }

        public override string GetStatName()
        {
            return "Projectile Size";
        }
    }
}