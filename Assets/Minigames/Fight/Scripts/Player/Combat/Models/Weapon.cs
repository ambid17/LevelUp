using System;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum WeaponType
    {
        Pistol,
        RocketLauncher,
        Katana
    }

    [Serializable]
    public class Weapon : ScriptableObject
    {
        public WeaponType WeaponType;
        public GameObject Prefab;
        public Sprite Icon;
        public List<Synergy> AllSynergies;
        public List<Synergy> UnlockedSynergies;
        public WeaponStats Stats;

        public void UnlockSynergy(Synergy synergy)
        {
            if (!UnlockedSynergies.Contains(synergy))
            {
                UnlockedSynergies.Add(synergy);
            }
            else
            {
                Debug.LogError("synergy already unlocked");
            }
        }

        [NonSerialized] private int _weightTotal;

        public Synergy GetRandomSynergy()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = AllSynergies.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var synergy in AllSynergies)
            {
                randomWeight -= synergy.SpawnWeight;
                if (randomWeight < 0)
                {
                    return synergy;
                }
            }

            return AllSynergies[0];
        }
    }
    
    [Serializable]
    public class Synergy
    {
        public int SpawnWeight;
    }
    
    public enum WeaponUpgradeType
    {
        FireRate,
        Damage,
        CritChance,
        CritDamage,
        ProjectileCount,
        ProjectilePenetration,
    }

    [Serializable]
    public class WeaponStats
    {
        public float baseFireRate;
        public float fireRateScalar;

        public float baseDamage;
        public float damageScalar;

        public float baseCritChance;
        public float critChanceScalar;

        public float baseCritDamage;
        public float critDamageScalar;

        public int baseProjectileCount;
        public int projectileCountScalar;
    
        public int baseProjectilePenetration;
        public int projectilePenetrationScalar;

    
        public float FireRate { get; private set; } 
        public void SetFireRate(int upgradeLevel)
        {
            FireRate = baseFireRate * Mathf.Pow(1 - fireRateScalar, upgradeLevel);
        }
    
    
        public float Damage { get; private set; }
        public void SetDamage(int upgradeLevel)
        {
            Damage = baseDamage * Mathf.Pow(1 + damageScalar, upgradeLevel);
        }
    
    
        public float CritChance { get; private set; }
        public void SetCritChance(int upgradeLevel)
        {
            CritChance = baseCritChance + (critChanceScalar * upgradeLevel);
        }
    
        public float CritDamage { get; private set; }
        public void SetCritDamage(int upgradeLevel)
        {
            CritDamage = baseCritDamage * Mathf.Pow(1 + critDamageScalar, upgradeLevel);
        }
    
        public int ProjectileCount { get; private set; }
        public void SetProjectileCount(int upgradeLevel)
        {
            ProjectileCount = baseProjectileCount + (upgradeLevel * projectileCountScalar);
        }
    
        public int ProjectilePenetration { get; private set; }
        public void SetProjectilePenetration(int upgradeLevel)
        {
            ProjectilePenetration = baseProjectilePenetration + (upgradeLevel * projectilePenetrationScalar);
        }
    
    
        public void ApplyUpgrade(WeaponUpgrade upgrade)
        {
            switch (upgrade.upgradeType)
            {
                case WeaponUpgradeType.FireRate:
                    SetFireRate(upgrade.numberPurchased);
                    break;
                case WeaponUpgradeType.Damage:
                    SetDamage(upgrade.numberPurchased);
                    break;
                case WeaponUpgradeType.CritChance:
                    SetCritChance(upgrade.numberPurchased);
                    break;
                case WeaponUpgradeType.CritDamage:
                    SetCritDamage(upgrade.numberPurchased);
                    break;
                case WeaponUpgradeType.ProjectileCount:
                    SetProjectileCount(upgrade.numberPurchased);
                    break;
                case WeaponUpgradeType.ProjectilePenetration:
                    SetProjectilePenetration(upgrade.numberPurchased);
                    break;
            
            }
        }

        public void Init()
        {
            SetFireRate(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.FireRate).numberPurchased);
            SetDamage(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.Damage).numberPurchased);
            SetCritChance(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.CritChance).numberPurchased);
            SetCritDamage(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.CritDamage).numberPurchased);
            SetProjectileCount(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.ProjectileCount).numberPurchased);
            SetProjectilePenetration(GameManager.SettingsManager.upgradeSettings.GetWeaponUpgrade(WeaponUpgradeType.ProjectilePenetration).numberPurchased);
        }
    }
}