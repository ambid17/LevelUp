using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

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
    private void SetGoldPerMinute(int upgradeLevel)
    {
        GoldPerMinute = Mathf.Pow(1 + goldPerMinScalar, upgradeLevel);
    }
    
    public float IdleTime { get; private set; }
    private void SetIdleTime(int upgradeLevel)
    {
        IdleTime = baseIdleTimeInMinutes * Mathf.Pow(1 + idleTimeScalar, upgradeLevel);
    }
    
    public float IdleGoldPercent { get; private set; }
    private void SetIdleGoldPercent(int upgradeLevel)
    {
        IdleGoldPercent = idleGoldPercentScalar * upgradeLevel;
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
    private void SetGoldPerKill(int upgradeLevel)
    {
        GoldPerKill = 1 + (goldPerKillScalar * upgradeLevel);
    }
    
    public float DeathTimer { get; private set; }
    private void SetDeathTimer(int upgradeLevel)
    {
        DeathTimer = baseDeathTimer * Mathf.Pow(1 - deathTimerScalar, upgradeLevel);
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
        SetGoldPerMinute(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.IdleGoldPerMin).numberPurchased);
        SetIdleTime(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.IdleTime).numberPurchased);
        SetIdleGoldPercent(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.IdleGoldPercent).numberPurchased);
        SetKillsPerKill(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.KillsPerKill).numberPurchased);
        SetSaveHighestGold(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.SaveHighestGold).numberPurchased);
        SetGoldPerKill(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.GoldPerKill).numberPurchased);
        SetDeathTimer(GameManager.SettingsManager.upgradeSettings.GetIncomeUpgrade(IncomeUpgradeType.DeathTimer).numberPurchased);
    }
}
