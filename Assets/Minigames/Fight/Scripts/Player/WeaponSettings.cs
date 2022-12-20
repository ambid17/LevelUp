using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponUpgradeType
{
    FireRate,
    Damage,
    Unlock
}

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "ScriptableObjects/WeaponSettings", order = 1)]
[Serializable]
public class WeaponSettings : ScriptableObject
{
    public List<WeaponSetting> weapons;
    
    public void ApplyUpgrade(WeaponUpgrade upgrade)
    {
        // switch (upgrade.upgradeType)
        // {
        //     case WeaponUpgradeType.FireRate:
        //         SetFireRate(upgrade.numberPurchased);
        //         break;
        // }
    }
}

[Serializable]
public class WeaponSetting
{
    public string name;
    public float baseFireRate;
    public float fireRateScalar;

    public float baseDamage;
    public float damageScalar;

    
    private float fireRate;
    public float FireRate => fireRate;
    public void SetFireRate(int upgradeLevel)
    {
        float multiplier = upgradeLevel * 1.1f;
        fireRate = baseFireRate * multiplier;
    }
    
    private float damage;
    public float Damage => damage;
    public void SetDamage(int upgradeLevel)
    {
        float multiplier = upgradeLevel * 1.1f;
        damage = baseFireRate * multiplier;
    }
    
    
    

    public float GetUpgrade(WeaponUpgradeType upgradeType)
    {
        switch (upgradeType)
        {
            case WeaponUpgradeType.FireRate:
                return FireRate;
        }

        return 0;
    }
}
