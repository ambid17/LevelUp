using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class UpgradeData
{
    public List<UpgradeModel> upgrades;

    // The empty constructor needs to be here for JSON .NET to have a way to create this class
    // If this gets deleted it will try to use the other constructor, bad juju
    public UpgradeData() { }
    
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
                typedUpgrade.playerUpgradeType = playerUpgrade.upgradeType;
                newUpgrade = typedUpgrade; 
                break;
            case WeaponUpgrade weaponUpgrade:
                var typedUpgrade2 = new WeaponUpgradeModel();
                typedUpgrade2.numberPurchased = weaponUpgrade.numberPurchased;
                typedUpgrade2.weaponUpgradeType = weaponUpgrade.upgradeType;
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
    public PlayerUpgradeType playerUpgradeType;
}

[Serializable]
public class WeaponUpgradeModel : UpgradeModel
{
    public WeaponUpgradeType weaponUpgradeType;
}