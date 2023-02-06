using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "UpgradeSettings", menuName = "ScriptableObjects/Fight/UpgradeSettings", order = 1)]
    [Serializable]
    public class UpgradeSettings : ScriptableObject
    {
        public List<PlayerUpgrade> PlayerUpgrades;
        public List<WeaponUpgrade> WeaponUpgrades;
        public List<EnemyUpgrade> EnemyUpgrades;
        public List<IncomeUpgrade> IncomeUpgrades;
        
        public void SetDefaults()
        {
            foreach (var upgrade in PlayerUpgrades)
            {
                upgrade.numberPurchased = 0;
            }

            foreach (var upgrade in WeaponUpgrades)
            {
                upgrade.numberPurchased = 0; 
            }
            
            foreach (var upgrade in EnemyUpgrades)
            {
                upgrade.numberPurchased = 0; 
            }
            
            foreach (var upgrade in IncomeUpgrades)
            {
                upgrade.numberPurchased = 0; 
            }
        }

        public PlayerUpgrade GetPlayerUpgrade(PlayerUpgradeType upgradeType)
        {
            return PlayerUpgrades.FirstOrDefault(u => u.upgradeType == upgradeType);
        }
    
        public WeaponUpgrade GetWeaponUpgrade(WeaponUpgradeType upgradeType)
        {
            return WeaponUpgrades.FirstOrDefault(u => u.upgradeType == upgradeType);
        }
        
        public EnemyUpgrade GetEnemyUpgrade(EnemyUpgradeType upgradeType)
        {
            return EnemyUpgrades.FirstOrDefault(u => u.upgradeType == upgradeType);
        }
        
        public IncomeUpgrade GetIncomeUpgrade(IncomeUpgradeType upgradeType)
        {
            return IncomeUpgrades.FirstOrDefault(u => u.upgradeType == upgradeType);
        }
    }

    public enum UpgradeCostType
    {
        Additive,
        Exponential,
    }

    [Serializable]
    public class Upgrade
    {
        public string name;
        public string description;
        public string bonusDescription;
        public int numberPurchased;
        public int maxPurchases;
        public int tier;
        public Sprite icon;
    
        public int baseCost;
        public float costScalar;
        public UpgradeCostType costType;

        public string GetUpgradeCountText()
        {
            string upgradeCountText = $"{numberPurchased}";

            if (maxPurchases > 0)
            {
                upgradeCountText += $" / {maxPurchases}";
            }

            upgradeCountText += " Purchased";

            return upgradeCountText;
        }

        public virtual float GetCost(int purchaseCount)
        {
            switch (costType)
            {
                case UpgradeCostType.Additive:
                    return GetAdditiveCost(purchaseCount);
                case UpgradeCostType.Exponential:
                    return GetExponentialCost(purchaseCount);
                default:
                    return float.MaxValue;
            }
        }
        
        public virtual string GetDescription()
        {
            return string.Empty;
        }

        public virtual string GetBonusDescription()
        {
            return string.Empty;
        }

        // example:
        // base cost = 10, scalar = 1
        // 10, 11, 12, 13, 14
        private float GetAdditiveCost(int purchaseCount)
        {
            float totalCost = 0;
            for (int currentNumPurchased = numberPurchased; currentNumPurchased < numberPurchased + purchaseCount; currentNumPurchased++)
            {
                totalCost += baseCost + (costScalar * currentNumPurchased);
            }
            
            return totalCost;
        }

        // example:
        // base cost = 100, scalar (percentage) = 0.5;
        // 100, 150, 225
        private float GetExponentialCost(int purchaseCount)
        {
            float totalCost = 0;
            for (int currentNumPurchased = numberPurchased; currentNumPurchased < numberPurchased + purchaseCount; currentNumPurchased++)
            {
                totalCost += baseCost * Mathf.Pow(costScalar, currentNumPurchased);
            }
            
            return totalCost;
        }
    }

    [Serializable]
    public class PlayerUpgrade : Upgrade
    {
        public PlayerUpgradeType upgradeType;
        
        public override string GetDescription()
        {
            string desc = description;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case PlayerUpgradeType.MaxHp:
                    value = GameManager.SettingsManager.playerSettings.MaxHpScalarPercent.ToString();
                    break;
                case PlayerUpgradeType.MoveSpeed:
                    value = GameManager.SettingsManager.playerSettings.MoveSpeedScalarPercent.ToString();
                    break;
                case PlayerUpgradeType.MoveAcceleration:
                    value = GameManager.SettingsManager.playerSettings.AccelerationScalarPercent.ToString();
                    break;
                case PlayerUpgradeType.LifeSteal:
                    value = GameManager.SettingsManager.playerSettings.LifeStealScalarPercent.ToString();
                    break;
            }
            
            desc = string.Format(desc, value);
            return desc;
        }
        
        public override string GetBonusDescription()
        {
            string description = bonusDescription;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case PlayerUpgradeType.MaxHp:
                    value = GameManager.SettingsManager.playerSettings.MaxHpScalePercent.ToString();
                    break;
                case PlayerUpgradeType.MoveSpeed:
                    value = GameManager.SettingsManager.playerSettings.MoveSpeedScalePercent.ToString();
                    break;
                case PlayerUpgradeType.MoveAcceleration:
                    value = GameManager.SettingsManager.playerSettings.AccelerationScalePercent.ToString();
                    break;
                case PlayerUpgradeType.LifeSteal:
                    value = GameManager.SettingsManager.playerSettings.LifeStealScalePercent.ToString();
                    break;
            }
            
            description = string.Format(description, value);
            return description;
        }
    }

    [Serializable]
    public class WeaponUpgrade : Upgrade
    {
        public WeaponUpgradeType upgradeType;
        public Weapon weapon;
    }
    
    [Serializable]
    public class EnemyUpgrade : Upgrade
    {
        public EnemyUpgradeType upgradeType;
        
        public override string GetDescription()
        {
            string desc = description;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case EnemyUpgradeType.Hp:
                    value = GameManager.SettingsManager.enemySpawnerSettings.HpScalarPercent.ToString();
                    break;
                case EnemyUpgradeType.FireRate:
                    value = GameManager.SettingsManager.enemySpawnerSettings.FireRatePercent.ToString();
                    break;
                case EnemyUpgradeType.WaveInterval:
                    value = GameManager.SettingsManager.enemySpawnerSettings.WaveIntervalScalarPercent.ToString();
                    break;
                case EnemyUpgradeType.WaveSize:
                    value = GameManager.SettingsManager.enemySpawnerSettings.WaveSizeScalar.ToString();
                    break;
            }
            
            desc = desc.Replace("{0}",value);
            return desc;
        }
        
        public override string GetBonusDescription()
        {
            string description = bonusDescription;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case EnemyUpgradeType.Hp:
                    value = GameManager.SettingsManager.enemySpawnerSettings.HpScalePercent.ToString();
                    break;
                case EnemyUpgradeType.FireRate:
                    value = GameManager.SettingsManager.enemySpawnerSettings.FireRatePercent.ToString();
                    break;
                case EnemyUpgradeType.WaveInterval:
                    value = GameManager.SettingsManager.enemySpawnerSettings.WaveIntervalScalePercent.ToString();
                    break;
                case EnemyUpgradeType.WaveSize:
                    value = GameManager.SettingsManager.enemySpawnerSettings.WaveSize.ToString();
                    break;
            }
            
            description = description.Replace("{0}",value);
            return description;
        }
    }
    
    [Serializable]
    public class IncomeUpgrade : Upgrade
    {
        public IncomeUpgradeType upgradeType;
        
        public override string GetDescription()
        {
            string desc = description;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case IncomeUpgradeType.DeathTimer:
                    value = GameManager.SettingsManager.incomeSettings.DeathTimeScalarPercent.ToString();
                    break;
                case IncomeUpgradeType.IdleTime:
                    value = GameManager.SettingsManager.incomeSettings.idleTimeScalar.ToString();
                    break;
                case IncomeUpgradeType.GoldPerKill:
                    value = GameManager.SettingsManager.incomeSettings.GoldPerKillScalarPercent.ToString();
                    break;
                case IncomeUpgradeType.IdleGoldPercent:
                    value = GameManager.SettingsManager.incomeSettings.IdleGoldScalarPercent.ToString();
                    break;
                case IncomeUpgradeType.KillsPerKill:
                    value = GameManager.SettingsManager.incomeSettings.killsPerKillScalar.ToString();
                    break;
                case IncomeUpgradeType.SaveHighestGold:
                    value = GameManager.SettingsManager.incomeSettings.DeathTimer.ToString();
                    break;
                case IncomeUpgradeType.IdleGoldPerMin:
                    value = GameManager.SettingsManager.incomeSettings.GoldPerMinuteScalarPercent.ToString();
                    break;
            }
            
            desc = desc.Replace("{0}",value);
            return desc;
        }
        
        public override string GetBonusDescription()
        {
            string description = bonusDescription;
            
            string value = String.Empty;

            switch (upgradeType)
            {
                case IncomeUpgradeType.DeathTimer:
                    value = GameManager.SettingsManager.incomeSettings.DeathTimer.ToString();
                    break;
                case IncomeUpgradeType.IdleTime:
                    value = GameManager.SettingsManager.incomeSettings.IdleTimeScale.ToString();
                    break;
                case IncomeUpgradeType.GoldPerKill:
                    value = GameManager.SettingsManager.incomeSettings.GoldPerKillPercent.ToString();
                    break;
                case IncomeUpgradeType.IdleGoldPercent:
                    value = GameManager.SettingsManager.incomeSettings.IdleGoldRatioPercent.ToString();
                    break;
                case IncomeUpgradeType.KillsPerKill:
                    value = GameManager.SettingsManager.incomeSettings.KillsPerKill.ToString();
                    break;
                case IncomeUpgradeType.SaveHighestGold:
                    if (GameManager.SettingsManager.incomeSettings.SaveHighestGold)
                    {
                        value = "Saving highest gold";
                    }
                    break;
                case IncomeUpgradeType.IdleGoldPerMin:
                    value = GameManager.SettingsManager.incomeSettings.GoldPerMinute.ToString();
                    break;
            }
            
            description = description.Replace("{0}",value);
            return description;
        }
    }
}