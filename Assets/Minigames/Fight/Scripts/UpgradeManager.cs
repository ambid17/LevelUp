using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public EnemySpawnerSettings enemySettings;
    public WeaponSettings weaponSettings;
    public UpgradeSettings upgradeSettings;
    
    void Start()
    {
        UpgradeItem.upgradePurchased += OnUpgradePurchased;

        LoadDefaults();
    }

    private void LoadDefaults()
    {
        playerSettings.SetDefaults();

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
    
    public void LoadSerializedUpgrades(UpgradeModelContainer container)
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
            }
        }
    }

    private void UpdatePlayerUpgrades(PlayerUpgradeModel model)
    {
        PlayerUpgrade upgrade = upgradeSettings.PlayerUpgrades.First(u => u.upgradeType == model.upgradeType);
        upgrade.numberPurchased = model.numberPurchased;
    }

    public List<Upgrade> GetAllUpgrades()
    {
        List<Upgrade> toReturn = new List<Upgrade>();
        
        toReturn.AddRange(upgradeSettings.PlayerUpgrades);
        toReturn.AddRange(upgradeSettings.WeaponUpgrades);
        
        return toReturn;
    }
}
