using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "RandomWeaponEffect", menuName = "ScriptableObjects/Effects/Weapon/RandomWeaponEffect", order = 1)]
    [Serializable]
    public class RandomWeaponEffect: StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            WeaponStats weaponStats;
            if (_upgradeCategory == UpgradeCategory.Range)
            {
                weaponStats = entity.Stats.combatStats.projectileWeaponStats;
            }
            else
            {
                weaponStats = entity.Stats.combatStats.meleeWeaponStats;
            }

            int statSelect = UnityEngine.Random.Range(0, 9);

            switch (statSelect)
            {
                case 0:
                    return weaponStats.ammoRegenRate;
                case 1:
                    return weaponStats.baseDamage;
                case 2:
                    return weaponStats.projectileMoveSpeed;
                case 3:
                    return weaponStats.projectileLifeTime;
                case 4:
                    return weaponStats.rateOfFire;
                case 5:
                    return weaponStats.maxAmmo;
                case 6:
                    return weaponStats.projectilesPerShot;
                case 7:
                    return weaponStats.projectileSpread;
                case 8:
                    return weaponStats.projectileSize;
                default:
                    return weaponStats.ammoRegenRate;
            }
        }

        public override string GetStatName()
        {
            return "Random Stat";
        }
    }
}