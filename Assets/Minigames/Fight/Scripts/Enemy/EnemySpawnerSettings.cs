using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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