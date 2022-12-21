using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    public List<UpgradeModel> upgrades;

    public UpgradeData(List<Upgrade> upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            TrackUpgrade(upgrade);
        }
    }
    
    public void TrackUpgrade(Upgrade upgrade)
    {
        if (upgrades == null)
        {
            upgrades = new List<UpgradeModel>();
        }

        UpgradeModel newUpgrade = null;
        switch (upgrade)
        {
            case PlayerUpgrade playerUpgrade:
                var typedUpgrade = new PlayerUpgradeModel();
                typedUpgrade.numberPurchased = playerUpgrade.numberPurchased;
                typedUpgrade.upgradeType = playerUpgrade.upgradeType;
                newUpgrade = typedUpgrade; 
                break;
            case WeaponUpgrade weaponUpgrade:
                var typedUpgrade2 = new WeaponUpgradeModel();
                typedUpgrade2.numberPurchased = weaponUpgrade.numberPurchased;
                typedUpgrade2.upgradeType = weaponUpgrade.upgradeType;
                newUpgrade = typedUpgrade2; 
                break;
        }
        
        upgrades.Add(newUpgrade);
    }
}

[Serializable]
public class UpgradeModel
{
    public int numberPurchased;
}

[Serializable]
public class PlayerUpgradeModel : UpgradeModel
{
    public PlayerUpgradeType upgradeType;
}

[Serializable]
public class WeaponUpgradeModel : UpgradeModel
{
    public WeaponUpgradeType upgradeType;
}