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

    [SerializeField] private int enemyMaxCount;
    private int _enemyCount;
    public int EnemyCount { get; set; }

    [SerializeField] private float minSpawnRadius;
    [SerializeField] private float maxSpawnRadius;
    
    void Start()
    {
        GameManager.Instance.enemySpawner = this;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer > spawnInterval && _enemyCount < enemyMaxCount)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        spawnTimer = 0;
        EnemyType typeToSpawn = GetWeightedRandomEnemy();
        GameObject enemyGO = enemyDataContainer.enemyContainer.First(e => e.enemyType == typeToSpawn).prefab;
        GameObject instance = Instantiate(enemyGO);
        instance.transform.position = GetRandomInDonut(minSpawnRadius, maxSpawnRadius);
        _enemyCount++;
    }
    
    // See this for more info:
    // https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
    public EnemyType GetWeightedRandomEnemy ()
    {
        int[] weights = weightTable.Keys.ToArray();
        int randomWeight = Random.Range(0, weights.Sum());
        for (int i = 0; i < weights.Length; ++i)
        {
            randomWeight -= weights[i];
            if (randomWeight < 0)
            {
                return weightTable.ElementAt(i).Value;
            }
        }

        return EnemyType.easy;
    }
    
    public Vector2 GetRandomInDonut(float minDistance, float maxDistance)
    {
        Vector2 point = Random.insideUnitCircle;
        point = point.normalized;
        point *= Random.Range(minDistance, maxDistance);

        return point;
    }
}