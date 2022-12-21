using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerUpgradeType
{
    MoveSpeed,
    MoveAcceleration,
    MaxHp
}

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
[Serializable]
public class PlayerSettings : ScriptableObject
{
    public float baseMoveSpeed;
    public float moveSpeedScalar;
    public float baseAcceleration;
    public float accelerationScalar;
    public float baseMaxHp;
    public float hpScalar;
    

    private float moveSpeed;
    public float MoveSpeed => moveSpeed;
    public void SetMoveSpeed(int upgradeLevel)
    {
        moveSpeed = baseMoveSpeed * Mathf.Pow(1 + moveSpeedScalar, upgradeLevel);
    }
    
    private float acceleration;
    public float Acceleration => acceleration;
    public void SetAcceleration(int upgradeLevel)
    {
        acceleration = baseAcceleration * Mathf.Pow(1 + accelerationScalar, upgradeLevel);
    }
    
    private float maxHp;
    public float MaxHp => maxHp;
    public void SetMaxHp(int upgradeLevel)
    {
        maxHp = baseMaxHp * Mathf.Pow(1 + hpScalar, upgradeLevel);
    }

    public void SetDefaults()
    {
        SetMoveSpeed(0);
        SetAcceleration(0);
        SetMaxHp(0);
    }

    // Set on:
    // - buying an upgrade
    // - loading the game's scene
    public void ApplyUpgrade(PlayerUpgrade upgrade)
    {
        switch (upgrade.upgradeType)
        {
            case PlayerUpgradeType.MoveSpeed:
                SetMoveSpeed(upgrade.numberPurchased);
                break;
            case PlayerUpgradeType.MoveAcceleration:
                SetAcceleration(upgrade.numberPurchased);
                break;
        }
    }
}
