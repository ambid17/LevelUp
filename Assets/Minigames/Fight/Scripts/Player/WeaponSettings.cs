using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponUpgradeType
{
    FireRate,
    Damage,
    CritChance,
    CritDamage,
    ProjectileCount,
    ProjectilePenetration,
}

[CreateAssetMenu(fileName = "WeaponSettings", menuName = "ScriptableObjects/WeaponSettings", order = 1)]
[Serializable]
public class WeaponSettings : ScriptableObject
{
    public float baseFireRate;
    public float fireRateScalar;

    public float baseDamage;
    public float damageScalar;

    public float baseCritChance;
    public float critChanceScalar;

    public float baseCritDamage;
    public float critDamageScalar;

    public int baseProjectileCount;
    public int projectileCountScalar;
    
    public int baseProjectilePenetration;
    public int projectilePenetrationScalar;

    
    private float fireRate;
    public float FireRate => fireRate;
    public void SetFireRate(int upgradeLevel)
    {
        fireRate = baseFireRate * Mathf.Pow(1 - fireRateScalar, upgradeLevel);
    }
    
    
    private float damage;
    public float Damage => damage;
    public void SetDamage(int upgradeLevel)
    {
        damage = baseDamage * Mathf.Pow(1 + damageScalar, upgradeLevel);
    }
    
    
    private float critChance;
    public float CritChance => critChance;
    public void SetCritChance(int upgradeLevel)
    {
        critChance = baseCritChance + (critChanceScalar * upgradeLevel);
    }
    
    private float critDamage;
    public float CritDamage => critDamage;
    public void SetCritDamage(int upgradeLevel)
    {
        critDamage = baseCritDamage * Mathf.Pow(1 + critDamageScalar, upgradeLevel);
    }
    
    private int projectileCount;
    public int ProjectileCount => projectileCount;
    public void SetProjectileCount(int upgradeLevel)
    {
        projectileCount = baseProjectileCount + (upgradeLevel * projectileCountScalar);
    }
    
    private int projectilePenetration;
    public int ProjectilePenetration => projectilePenetration;
    public void SetProjectilePenetration(int upgradeLevel)
    {
        projectilePenetration = baseProjectilePenetration + (upgradeLevel * projectilePenetrationScalar);
    }
    
    
    public void ApplyUpgrade(WeaponUpgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case WeaponUpgradeType.FireRate:
                SetFireRate(upgrade.numberPurchased);
                break;
            case WeaponUpgradeType.Damage:
                SetDamage(upgrade.numberPurchased);
                break;
            case WeaponUpgradeType.CritChance:
                SetCritChance(upgrade.numberPurchased);
                break;
            case WeaponUpgradeType.CritDamage:
                SetCritDamage(upgrade.numberPurchased);
                break;
            case WeaponUpgradeType.ProjectileCount:
                SetProjectileCount(upgrade.numberPurchased);
                break;
            case WeaponUpgradeType.ProjectilePenetration:
                SetProjectilePenetration(upgrade.numberPurchased);
                break;
            
        }
    }

    public void Init()
    {
        SetFireRate(0);
        SetDamage(0);
        SetCritChance(0);
        SetCritDamage(0);
        SetProjectileCount(0);
        SetProjectilePenetration(0);
    }
}
