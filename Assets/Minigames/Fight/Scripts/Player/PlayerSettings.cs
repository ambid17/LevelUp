using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
[Serializable]
public class PlayerSettings : ScriptableObject
{
    public float moveSpeed;
    public float acceleration;
    public float shotSpeed;
    public float shotDamage;


    // Set on:
    // - buying an upgrade
    // - loading the game's scene
    private float upgradedShotSpeed;
    public float UpgradedShotSpeed => upgradedShotSpeed;
    
    public void SetShotSpeed(Upgrade upgrade)
    {
        float multiplier = upgrade.numberPurchased * 1.1f;
        upgradedShotSpeed = moveSpeed * multiplier;
    }
}
