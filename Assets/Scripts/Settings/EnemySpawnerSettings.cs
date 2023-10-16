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
            Hp = 1 - (hpScalar * upgradeLevel);
        }

        public int WaveSize { get; private set; }
        private void SetWaveSize(int upgradeLevel)
        {
            WaveSize = baseWaveSize + (waveSizeScalar * upgradeLevel);
        }
        
        public float WaveInterval { get; private set; }
        public float WaveIntervalScale { get; private set; }
        private void SetWaveInterval(int upgradeLevel)
        {
            WaveIntervalScale = 1 - (waveIntervalScalar * upgradeLevel);
            WaveInterval = baseWaveInterval * WaveIntervalScale;
        }
        
        public float FireRate { get; private set; }
        private void SetFireRate(int upgradeLevel)
        {
            FireRate = 1 + (fireRateScalar * upgradeLevel);
        }

        public void Init()
        {
            SetHp(0);
            SetWaveSize(0);
            SetWaveInterval(0);
            SetFireRate(0);
        }
    }
}