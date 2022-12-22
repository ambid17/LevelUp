using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/ProgressSettings", order = 1)]
[Serializable]
public class ProgressSettings : ScriptableObject
{
    public List<World> Worlds;
    
    public float Currency;
    public World CurrentWorld;
    
    public void SetDefaults()
    {
        Currency = 0;
        CurrentWorld = Worlds[0];
    }
}

[Serializable]
public class World
{
    public string Name;
    public Sprite WorldSprite;
    public List<Country> Countries;
    public List<Enemy> Enemies;

    public Country CurrentCountry;

    
    public void TrySetNextCountry()
    {
        if (CurrentCountry.EnemyKillCount >= CurrentCountry.EnemyKillsToComplete)
        {
            CurrentCountry = Countries[CurrentCountry.index + 1];
            // TODO update progress UI, world sprite
        }
    }
    
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
public class Country
{
    public int index;
    public int EnemyKillsToComplete;
    public float EnemyStatScalar;
    public List<Vector2> SpritePixels;
    public Color EnemyTierColor;
    
    public int EnemyKillCount;
}

[Serializable]
public class Enemy
{
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
