using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
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
                    var playerUpgradeModel = new PlayerUpgradeModel();
                    playerUpgradeModel.numberPurchased = playerUpgrade.numberPurchased;
                    playerUpgradeModel.playerUpgradeType = playerUpgrade.upgradeType;
                    newUpgrade = playerUpgradeModel; 
                    break;
                case WeaponUpgrade weaponUpgrade:
                    var weaponUpgradeModel = new WeaponUpgradeModel();
                    weaponUpgradeModel.numberPurchased = weaponUpgrade.numberPurchased;
                    weaponUpgradeModel.weaponUpgradeType = weaponUpgrade.upgradeType;
                    newUpgrade = weaponUpgradeModel; 
                    break;
                case IncomeUpgrade weaponUpgrade:
                    var incomeUpgradeModel = new IncomeUpgradeModel();
                    incomeUpgradeModel.numberPurchased = weaponUpgrade.numberPurchased;
                    incomeUpgradeModel.incomeUpgradeType = weaponUpgrade.upgradeType;
                    newUpgrade = incomeUpgradeModel; 
                    break;
                case EnemyUpgrade weaponUpgrade:
                    var enemyUpgradeModel = new EnemyUpgradeModel();
                    enemyUpgradeModel.numberPurchased = weaponUpgrade.numberPurchased;
                    enemyUpgradeModel.enemyUpgradeType = weaponUpgrade.upgradeType;
                    newUpgrade = enemyUpgradeModel; 
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
    
    [Serializable]
    public class EnemyUpgradeModel : UpgradeModel
    {
        public EnemyUpgradeType enemyUpgradeType;
    }
    
    [Serializable]
    public class IncomeUpgradeModel : UpgradeModel
    {
        public IncomeUpgradeType incomeUpgradeType;
    }
}