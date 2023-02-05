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
        public float HpScalePercent => Hp * 100;
        public float HpScalarPercent => hpScalar * 100;
        private void SetHp(int upgradeLevel)
        {
            Hp = 1 - (hpScalar * upgradeLevel);
        }

        public int WaveSize { get; private set; }
        public int WaveSizeScalar => waveSizeScalar;
        private void SetWaveSize(int upgradeLevel)
        {
            WaveSize = baseWaveSize + (waveSizeScalar * upgradeLevel);
        }
        
        public float WaveInterval { get; private set; }
        public float WaveIntervalScale { get; private set; }
        public float WaveIntervalScalePercent => WaveIntervalScale * 100;
        public float WaveIntervalScalarPercent => waveIntervalScalar * 100;
        private void SetWaveInterval(int upgradeLevel)
        {
            WaveIntervalScale = 1 - (waveIntervalScalar * upgradeLevel);
            WaveInterval = baseWaveInterval * WaveIntervalScale;
        }
        
        public float FireRate { get; private set; }
        public float FireRatePercent => FireRate * 100;
        public float FireRateScalarPercent => fireRateScalar * 100;
        private void SetFireRate(int upgradeLevel)
        {
            FireRate = 1 + (fireRateScalar * upgradeLevel);
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