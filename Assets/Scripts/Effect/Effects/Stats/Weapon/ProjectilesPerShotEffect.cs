using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "ProjectilesPerShotEffect", menuName = "ScriptableObjects/Effects/Weapon/ProjectilesPerShotEffect", order = 1)]
    [Serializable]
    public class ProjectilesPerShotEffect : StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                return entity.Stats.combatStats.projectileWeaponStats.projectilesPerShot;
            }
            else
            {
                return entity.Stats.combatStats.meleeWeaponStats.projectilesPerShot;
            }
        }

        public override string GetStatName()
        {
            return "Projectiles Per Shot";
        }
    }
}