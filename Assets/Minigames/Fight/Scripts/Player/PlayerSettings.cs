using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public enum PlayerUpgradeType
    {
        MoveSpeed,
        MoveAcceleration,
        MaxHp,
        LifeSteal,
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
        public float lifeStealScalar;


        public float MoveSpeed { get; private set; }
        public void SetMoveSpeed(int upgradeLevel)
        {
            MoveSpeed = baseMoveSpeed * Mathf.Pow(1 + moveSpeedScalar, upgradeLevel);
        }
    
        public float Acceleration { get; private set; }
        public void SetAcceleration(int upgradeLevel)
        {
            Acceleration = baseAcceleration * Mathf.Pow(1 + accelerationScalar, upgradeLevel);
        }
    
        public float MaxHp { get; private set; }
        public void SetMaxHp(int upgradeLevel)
        {
            MaxHp = baseMaxHp * Mathf.Pow(1 + hpScalar, upgradeLevel);
        }
        
        public float LifeSteal { get; private set; }

        public void SetLifeSteal(int upgradeLevel)
        {
            LifeSteal = upgradeLevel * lifeStealScalar;
        }
        
        public void Init()
        {
            SetMoveSpeed(GameManager.SettingsManager.upgradeSettings.GetPlayerUpgrade(PlayerUpgradeType.MoveSpeed).numberPurchased);
            SetAcceleration(GameManager.SettingsManager.upgradeSettings.GetPlayerUpgrade(PlayerUpgradeType.MoveAcceleration).numberPurchased);
            SetMaxHp(GameManager.SettingsManager.upgradeSettings.GetPlayerUpgrade(PlayerUpgradeType.MaxHp).numberPurchased);
            SetLifeSteal(GameManager.SettingsManager.upgradeSettings.GetPlayerUpgrade(PlayerUpgradeType.LifeSteal).numberPurchased);
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
                case PlayerUpgradeType.MaxHp:
                    SetMaxHp(upgrade.numberPurchased);
                    break;
                case PlayerUpgradeType.LifeSteal:
                    SetLifeSteal(upgrade.numberPurchased);
                    break;
            }
        }
    }
}