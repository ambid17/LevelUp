using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyType
{
    easy,
    medium,
    hard
}

public class EnemySpawner : MonoBehaviour
{
    

    public Dictionary<int, EnemyType> weightTable = new Dictionary<int, EnemyType>()
    {
        { 10, EnemyType.easy},
        { 2, EnemyType.medium},
        { 1, EnemyType.hard},
    };

    public EnemyDataContainer enemyDataContainer;

    [SerializeField] private float spawnInterval;
    private float spawnTimer;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0;
        }
    }
    
    public EnemyType GetWeightedRandomEnemy ()
    {
        int[] weights = weightTable.Keys.ToArray();
        int randomWeight = UnityEngine.Random.Range(0, weights.Sum());
        for (int i = 0;i < weights.Length; ++i)
        {
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return weightTable[i];
            }
        }

        return EnemyType.easy;
    }
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
[Serializable]
public class EnemyDataContainer : ScriptableObject
{
    public List<EnemyData> enemyContainer;
}


[Serializable]
public class EnemyData
{
    public EnemyType enemyType;
    public GameObject prefab;
}
