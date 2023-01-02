using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public EnemySpawnerSettings enemySpawnerSettings;
    public WeaponSettings weaponSettings;
    public UpgradeSettings upgradeSettings;
    public ProgressSettings progressSettings;

    void Start()
    {
        UpgradeItem.upgradePurchased += OnUpgradePurchased;
    }

    /// <summary>
    /// Reset settings to empty run-time stats where necessary.
    /// </summary>
    public void SetDefaults()
    {
        upgradeSettings.SetDefaults();
        progressSettings.SetDefaults();
    }

    /// <summary>
    /// Sets up all of the scriptable object to have base values
    /// </summary>
    public void Init()
    {
        playerSettings.Init();
        weaponSettings.Init();
        progressSettings.Init();
    }

    private void OnUpgradePurchased(Upgrade upgrade)
    {
        switch (upgrade)
        {
            case PlayerUpgrade playerUpgrade:
                playerSettings.ApplyUpgrade(playerUpgrade);
                break;
            case WeaponUpgrade weaponUpgrade:
                weaponSettings.ApplyUpgrade(weaponUpgrade);
                break;
            default:
                Debug.LogError($"Invalid upgrade type: {upgrade.name}");
                break;
        }
    }

    public List<Upgrade> GetAllUpgrades()
    {
        List<Upgrade> toReturn = new List<Upgrade>();
        
        toReturn.AddRange(upgradeSettings.PlayerUpgrades);
        toReturn.AddRange(upgradeSettings.WeaponUpgrades);
        
        return toReturn;
    }

    public ProgressModel GetProgressForSerialization()
    {
        ProgressModel toReturn = new ProgressModel();
        
        toReturn.Currency = GameManager.GameStateManager.Currency;
        toReturn.WorldData = progressSettings.GetWorldData();

        return toReturn;
    }
}
