using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public enum WeaponStat
    {
        baseDamage,
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
                if (scalingType == ImpactScalingType.Chance)
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

        private readonly string _description = "{0}% chance for {1}{2} {3} per stack {4}";
        public override string GetDescription()
        {
            string chance = $"{percentChance * 100}";
            string raisesOrLowers = "+";

            switch (statImpactType)
            {
                case StatImpactType.Flat:
                    if (impactPerStack < 0)
                    {
                        raisesOrLowers = "-";
                    }
                    break;
                case StatImpactType.Additive:
                case StatImpactType.Compounding:
                    if (impactPerStack < 1)
                    {
                        raisesOrLowers = "-";
                    }
                    break;
                default:
                    raisesOrLowers = "+";
                    break;
            }

            string impactString = impactPerStack.ToString();
            if (statImpactType == StatImpactType.Additive || statImpactType == StatImpactType.Compounding)
            {
                // convert from 1.1 -> 0.1 -> 10%
                // or from 0.9 -> -10%
                float impact = impactPerStack;
                if (impactPerStack > 1)
                {
                    impact -= 1;
                }
                else
                {
                    impact = Mathf.Abs(1 - impact);
                }
                impactString = $"{impact * 100}%";
            }

            string impactTypeString = string.Empty;
            if (statImpactType == StatImpactType.Additive || statImpactType == StatImpactType.Compounding)
            {
                impactTypeString = statImpactType.ToString();
            }
            return string.Format(_description, chance, raisesOrLowers, impactString, GetStatName(), impactTypeString);
        }

        public override string GetStatName()
        {
            switch (weaponStat)
            {
                case WeaponStat.baseDamage:
                    return "Base Damage";
                case WeaponStat.projectileMoveSpeed:
                    return "Projectile Move Speed";
                case WeaponStat.projectileLifeTime:
                    return "Projectile Lifetime";
                case WeaponStat.rateOfFire:
                    return "Rate Of Fire";
                case WeaponStat.maxAmmo:
                    return "Max Ammo";
                case WeaponStat.ammoRegenRate:
                    return "Ammo Regen Rate";
                case WeaponStat.projectilesPerShot:
                    return "Projectiles Per Shot";
                case WeaponStat.projectileSpread:
                    return "Projectile Spread";
                case WeaponStat.projectileSize:
                    return "Projectile Size";
                case WeaponStat.projectilePenetration:
                    return "Projectile Penetration";
                default:
                    return null;
            }
        }
    }
}