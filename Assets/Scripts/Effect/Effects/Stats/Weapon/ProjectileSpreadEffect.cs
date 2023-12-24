using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectileSpreadEffect", menuName = "ScriptableObjects/Effects/Weapon/ProjectileSpreadEffect", order = 1)]
    [Serializable]
    public class ProjectileSpreadEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.projectileSpread;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.projectileSpread;
            }
        }
    }
}