using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public enum WeaponStat
    {
        baseDamage,
        onHitDamage,
        projectileMoveSpeed,
        projectileLifeTime,
        rateOfFire,
        maxAmmo,
        ammoRegenRate,
        projectilesPerShot,
        projectileSpread,
        projectileSize,
        projectilePenetration
    }

    public enum ImpactScalingType
    {
        Chance,
        Stat
    }

    [CreateAssetMenu(fileName = "PercentChanceWeaponEffect", menuName = "ScriptableObjects/Effects/Weapon/PercentChanceWeaponEffect", order = 1)]
    [Serializable]
    public class PercentChanceWeaponEffect : StatModifierEffect
    {
        public WeaponStat weaponStat;
        public ImpactScalingType scalingType;
        public float percentChance;

        protected override float Impact
        {
            get
            {
                // keeps overriden impact code, but allows the Impact to not be touched in favor of the stat upgrades 
                // scaling the percent likelihood of success
                if (scalingType == ImpactScalingType.Stat)
                {
                    return impactPerStack;
                }

                if (statImpactType == StatImpactType.Flat || statImpactType == StatImpactType.Additive)
                {
                    return impactPerStack * _amountOwned;
                }
                else if (statImpactType == StatImpactType.Compounding)
                {
                    return Mathf.Pow(impactPerStack, _amountOwned);
                }
                else
                {
                    return impactPerStack;
                }
            }
        }


        public override ModifiableStat GetStatToAffect(Entity entity)
        {
            var weaponStats = entity.Stats.combatStats.projectileWeaponStats;
            if(_upgradeCategory == UpgradeCategory.Melee)
            {
                weaponStats = entity.Stats.combatStats.meleeWeaponStats;
            }

            switch (weaponStat)
            {
                case WeaponStat.baseDamage:
                    return weaponStats.baseDamage;
                case WeaponStat.onHitDamage:
                    return weaponStats.onHitDamage;
                case WeaponStat.projectileMoveSpeed:
                    return weaponStats.projectileMoveSpeed;
                case WeaponStat.projectileLifeTime:
                    return weaponStats.projectileLifeTime;
                case WeaponStat.rateOfFire:
                    return weaponStats.rateOfFire;
                case WeaponStat.maxAmmo:
                    return weaponStats.maxAmmo;
                case WeaponStat.ammoRegenRate:
                    return weaponStats.ammoRegenRate;
                case WeaponStat.projectilesPerShot:
                    return weaponStats.projectilesPerShot;
                case WeaponStat.projectileSpread:
                    return weaponStats.projectileSpread;
                case WeaponStat.projectileSize:
                    return weaponStats.projectileSize;
                case WeaponStat.projectilePenetration:
                    return weaponStats.projectilePenetration;
                default:
                    return null;
            }
        }

        public override float ImpactStat(float stat)
        {
            bool doesImpact = UnityEngine.Random.value < percentChance;

            if (!doesImpact)
            {
                return stat;
            }

            if (statImpactType == StatImpactType.Flat)
            {
                return stat + Impact;
            }
            else if (statImpactType == StatImpactType.Compounding || statImpactType == StatImpactType.Additive)
            {
                return stat * Impact;
            }
            else if (statImpactType == StatImpactType.ManualSet)
            {
                return Impact;
            }

            return stat;
        }
    }
}