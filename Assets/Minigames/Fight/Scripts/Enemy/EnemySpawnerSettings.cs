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

    public EnemySettings EnemySettings;
    public Enemy RandomEnemy => EnemySettings.GetRandomEnemy();
}

[Serializable]
public class EnemySettings
{
    public List<Enemy> Enemies;
    
    private int _weightTotal;
    
    // See this for more info:
    // https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
    public Enemy GetRandomEnemy()
    {
        if (_weightTotal == 0)
        {
            _weightTotal = Enemies.Sum(e => e.SpawnWeight);
        }
        
        int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
        foreach(var enemy in Enemies)
        {
            randomWeight -= enemy.SpawnWeight;
            if (randomWeight < 0)
            {
                return enemy;
            }
        }

        return Enemies[0];
    }
}

[Serializable]
public class Enemy
{
    public EnemyType EnemyType;
    public GameObject Prefab;
    public int SpawnWeight;
    public EnemyInstanceSettings Settings;
}

[Serializable]
public class EnemyInstanceSettings
{
    public float goldValue;
    public float shotSpeed;
    public float moveSpeed;
    public float maxHp = 100;
    public float weaponDamage;
}

