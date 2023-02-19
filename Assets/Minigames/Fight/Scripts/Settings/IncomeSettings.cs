using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum IncomeUpgradeType
    {
        IdleGoldPerMin,
        IdleTime,
        IdleGoldPercent,
        KillsPerKill,
        SaveHighestGold,
        GoldPerKill,
        DeathTimer
    }

    [CreateAssetMenu(fileName = "IncomeSettings", menuName = "ScriptableObjects/Fight/IncomeSettings", order = 1)]
    [Serializable]
    public class IncomeSettings : ScriptableObject
    {
        public float baseGoldPerMin;
        public float goldPerMinScalar;

        public float baseIdleTimeInMinutes;
        public float idleTimeScalar;

        public float idleGoldPercentScalar;

        public float killsPerKillScalar;

        public float goldPerKillScalar;

        public float baseDeathTimer;
        public float deathTimerScalar;


        public float GoldPerMinute { get; private set; }
        public float GoldPerMinuteScalarPercent => goldPerMinScalar * 100;

        private void SetGoldPerMinute(int upgradeLevel)
        {
            GoldPerMinute = baseGoldPerMin * Mathf.Pow(1 + goldPerMinScalar, upgradeLevel);
        }

        public float IdleTime { get; private set; }
        public float IdleTimeScale { get; private set; }

        private void SetIdleTime(int upgradeLevel)
        {
            IdleTimeScale = idleTimeScalar * upgradeLevel;
            IdleTime = baseIdleTimeInMinutes + IdleTimeScale;
        }

        public float IdleGoldRatio { get; private set; }
        public float IdleGoldRatioPercent => IdleGoldRatio * 100;
        public float IdleGoldScalarPercent => idleGoldPercentScalar * 100;

        private void SetIdleGoldPercent(int upgradeLevel)
        {
            IdleGoldRatio = idleGoldPercentScalar * upgradeLevel;
        }

        public float KillsPerKill { get; private set; }

        private void SetKillsPerKill(int upgradeLevel)
        {
            KillsPerKill = 1 + (killsPerKillScalar * upgradeLevel);
        }

        public bool SaveHighestGold { get; private set; }

        private void SetSaveHighestGold(int upgradeLevel)
        {
            SaveHighestGold = upgradeLevel > 0;
        }

        public float GoldPerKill { get; private set; }
        public float GoldPerKillPercent => GoldPerKill * 100;
        public float GoldPerKillScalarPercent => goldPerKillScalar * 100;

        private void SetGoldPerKill(int upgradeLevel)
        {
            GoldPerKill = 1 + (goldPerKillScalar * upgradeLevel);
        }

        public float DeathTimer { get; private set; }
        public float DeathTimeScalarPercent => deathTimerScalar * 100;

        private void SetDeathTimer(int upgradeLevel)
        {
            DeathTimer = baseDeathTimer * (1 - (deathTimerScalar * upgradeLevel));
        }


        public void ApplyUpgrade(IncomeUpgrade upgrade)
        {
            switch (upgrade.upgradeType)
            {
                case IncomeUpgradeType.IdleGoldPerMin:
                    SetGoldPerMinute(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.IdleTime:
                    SetIdleTime(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.IdleGoldPercent:
                    SetIdleGoldPercent(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.KillsPerKill:
                    SetKillsPerKill(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.SaveHighestGold:
                    SetSaveHighestGold(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.GoldPerKill:
                    SetGoldPerKill(upgrade.numberPurchased);
                    break;
                case IncomeUpgradeType.DeathTimer:
                    SetDeathTimer(upgrade.numberPurchased);
                    break;
            }
        }

        public void Init()
        {
            SetGoldPerMinute(GameManager.SettingsManager.upgradeSettings
                .GetIncomeUpgrade(IncomeUpgradeType.IdleGoldPerMin).numberPurchased);
            SetIdleTime(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.IdleTime)
                .numberPurchased);
            SetIdleGoldPercent(GameManager.SettingsManager.upgradeSettings
                .GetIncomeUpgrade(IncomeUpgradeType.IdleGoldPercent).numberPurchased);
            SetKillsPerKill(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.KillsPerKill)
                .numberPurchased);
            SetSaveHighestGold(GameManager.SettingsManager.upgradeSettings
                .GetIncomeUpgrade(IncomeUpgradeType.SaveHighestGold).numberPurchased);
            SetGoldPerKill(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.GoldPerKill)
                .numberPurchased);
            SetDeathTimer(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.DeathTimer)
                .numberPurchased);
        }
    }
}