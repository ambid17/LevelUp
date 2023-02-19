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

    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/Fight/PlayerSettings", order = 1)]
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
        public float MoveSpeedScale { get; private set; }
        public float MoveSpeedScalePercent => MoveSpeedScale * 100;
        public float MoveSpeedScalarPercent => moveSpeedScalar * 100;

        public void SetMoveSpeed(int upgradeLevel)
        {
            MoveSpeedScale = moveSpeedScalar * upgradeLevel;
            MoveSpeed = baseMoveSpeed * (1 + MoveSpeedScale);
        }
    
        public float Acceleration { get; private set; }
        public float AccelerationScale { get; private set; }
        public float AccelerationScalePercent => AccelerationScale * 100;
        public float AccelerationScalarPercent => accelerationScalar * 100;
        public void SetAcceleration(int upgradeLevel)
        {
            AccelerationScale = accelerationScalar * upgradeLevel;
            Acceleration = baseAcceleration * (1 + AccelerationScale);
        }
    
        public float MaxHp { get; private set; }
        public float MaxHpScale { get; private set; }
        public float MaxHpScalePercent => MaxHpScale * 100;
        public float MaxHpScalarPercent => hpScalar * 100;
        public void SetMaxHp(int upgradeLevel)
        {
            MaxHpScale = hpScalar * upgradeLevel;
            MaxHp = baseMaxHp * (1 + MaxHpScale);
        }
        
        public float LifeSteal { get; private set; }
        public float LifeStealScale { get; private set; }
        public float LifeStealScalePercent => LifeStealScale * 100;
        public float LifeStealScalarPercent => lifeStealScalar * 100;

        public void SetLifeSteal(int upgradeLevel)
        {
            LifeStealScale = lifeStealScalar * upgradeLevel;
            LifeSteal = LifeStealScale;
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