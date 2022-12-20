using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public PlayerSettings playerSettings;
    public EnemySpawnerSettings enemySettings;
    public WeaponSettings weaponSettings;
    
    void Start()
    {
        UpgradeItem.upgradePurchased.AddListener(OnUpgradePurchased);
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
}
