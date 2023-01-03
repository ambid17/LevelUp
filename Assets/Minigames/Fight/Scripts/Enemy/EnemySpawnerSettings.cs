using System;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "ScriptableObjects/EnemySpawnerSettings", order = 1)]
    [Serializable]
    public class EnemySpawnerSettings : ScriptableObject
    {
        public float MinSpawnRadius;
        public float MaxSpawnRadius;

        public float WaveInterval;
        public float WaveSize;

        public int MaxEnemyCount;
    }
}