using System;
using UnityEngine;

namespace Minigames.Fight
{
    public enum EnemyUpgradeType
    {
        Hp,
        WaveSize,
        WaveInterval,
        FireRate,
    }
    
    [CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "ScriptableObjects/Fight/EnemySpawnerSettings", order = 1)]
    [Serializable]
    public class EnemySpawnerSettings : ScriptableObject
    {
        public float MinSpawnRadius;
        public float MaxSpawnRadius;

        public int MaxEnemyCount;

        public float hpScalar;
        
        public int baseWaveSize;
        public int waveSizeScalar;
        
        public float baseWaveInterval;
        public float waveIntervalScalar;

        public float fireRateScalar;
        
        public float Hp { get; private set; }
        private void SetHp(int upgradeLevel)
        {
            Hp = Mathf.Pow(1 - hpScalar, upgradeLevel);
        }

        public int WaveSize { get; private set; }
        private void SetWaveSize(int upgradeLevel)
        {
            WaveSize = baseWaveSize + (waveSizeScalar * upgradeLevel);
        }
        
        public float WaveInterval { get; private set; }
        private void SetWaveInterval(int upgradeLevel)
        {
            WaveInterval = baseWaveInterval * Mathf.Pow(1 - waveIntervalScalar, upgradeLevel);
        }
        
        public float FireRate { get; private set; }
        private void SetFireRate(int upgradeLevel)
        {
            FireRate = Mathf.Pow(1 - fireRateScalar, upgradeLevel);
        }
        
        
        public void ApplyUpgrade(EnemyUpgrade upgrade)
        {
            switch (upgrade.upgradeType)
            {
                case EnemyUpgradeType.Hp:
                    SetHp(upgrade.numberPurchased);
                    break;
                case EnemyUpgradeType.WaveSize:
                    SetWaveSize(upgrade.numberPurchased);
                    break;
                case EnemyUpgradeType.WaveInterval:
                    SetWaveInterval(upgrade.numberPurchased);
                    break;
                case EnemyUpgradeType.FireRate:
                    SetFireRate(upgrade.numberPurchased);
                    break;
            }
        }

        public void Init()
        {
            SetHp(GameManager.SettingsManager.upgradeSettings.GetEnemyUpgrade(EnemyUpgradeType.Hp).numberPurchased);
            SetWaveSize(GameManager.SettingsManager.upgradeSettings.GetEnemyUpgrade(EnemyUpgradeType.WaveSize).numberPurchased);
            SetWaveInterval(GameManager.SettingsManager.upgradeSettings.GetEnemyUpgrade(EnemyUpgradeType.WaveInterval).numberPurchased);
            SetFireRate(GameManager.SettingsManager.upgradeSettings.GetEnemyUpgrade(EnemyUpgradeType.FireRate).numberPurchased);
        }
    }
}