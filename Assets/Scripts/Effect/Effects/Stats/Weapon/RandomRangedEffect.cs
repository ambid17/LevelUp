using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "RandomRangedEffect", menuName = "ScriptableObjects/Effects/RandomRangedEffect", order = 1)]
    [Serializable]
    public class RandomRangedEffect: StatModifierEffect
    {
        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            int statSelect = UnityEngine.Random.Range(0, 9);

            switch (statSelect)
            {
                case 0:
                    return entity.Stats.combatStats.projectileWeaponStats.ammoRegenRate;
                case 1:
                    return entity.Stats.combatStats.projectileWeaponStats.baseDamage;
                case 2:
                    return entity.Stats.combatStats.projectileWeaponStats.projectileMoveSpeed;
                case 3:
                    return entity.Stats.combatStats.projectileWeaponStats.projectileLifeTime;
                case 4:
                    return entity.Stats.combatStats.projectileWeaponStats.rateOfFire;
                case 5:
                    return entity.Stats.combatStats.projectileWeaponStats.maxAmmo;
                case 6:
                    return entity.Stats.combatStats.projectileWeaponStats.projectilesPerShot;
                case 7:
                    return entity.Stats.combatStats.projectileWeaponStats.projectileSpread;
                case 8:
                    return entity.Stats.combatStats.projectileWeaponStats.projectileSize;
                default:
                    return entity.Stats.combatStats.projectileWeaponStats.ammoRegenRate;
            }
        }
    }
}