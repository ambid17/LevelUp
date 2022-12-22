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

    public void SetDefaults()
    {
        upgradeSettings.SetDefaults();
        playerSettings.SetDefaults();
        weaponSettings.SetDefaults();
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
    
    public void LoadSerializedUpgrades(UpgradeData container)
    {
        if (container == null || container.upgrades == null)
        {
            return;
        }
        
        foreach (var upgrade in container.upgrades)
        {
            switch (upgrade)
            {
                case PlayerUpgradeModel model:
                    UpdatePlayerUpgrades(model);
                    break;
                case WeaponUpgradeModel model:
                    UpdateWeaponUpgrades(model);
                    break;
            }
        }
    }

    private void UpdatePlayerUpgrades(PlayerUpgradeModel model)
    {
        PlayerUpgrade upgrade = upgradeSettings.PlayerUpgrades.First(u => u.upgradeType == model.playerUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
        playerSettings.ApplyUpgrade(upgrade);
    }
    
    private void UpdateWeaponUpgrades(WeaponUpgradeModel model)
    {
        WeaponUpgrade upgrade = upgradeSettings.WeaponUpgrades.First(u => u.upgradeType == model.weaponUpgradeType);
        upgrade.numberPurchased = model.numberPurchased;
        weaponSettings.ApplyUpgrade(upgrade);
    }

    public List<Upgrade> GetAllUpgrades()
    {
        List<Upgrade> toReturn = new List<Upgrade>();
        
        toReturn.AddRange(upgradeSettings.PlayerUpgrades);
        toReturn.AddRange(upgradeSettings.WeaponUpgrades);
        
        return toReturn;
    }
}
