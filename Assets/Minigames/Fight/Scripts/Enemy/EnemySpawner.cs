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
    public EnemySpawnerSettings settings;

    private float waveTimer;
    [SerializeField] private int _enemyCount;

    public int EnemyCount
    {
        get => _enemyCount;
        set => _enemyCount = value;
    }

    void Start()
    {
        GameManager.Instance.enemySpawner = this;
    }

    void Update()
    {
        if (GameManager.Instance.IsDead)
        {
            return;
        }
        
        TrySpawnWave();
    }

    private void TrySpawnWave()
    {
        waveTimer += Time.deltaTime;

        if (_enemyCount > 0) // Make sure there's always enemies on the map
        {
            if (waveTimer < settings.WaveInterval || _enemyCount > settings.MaxEnemyCount)
            {
                return;
            }
        }

        waveTimer = 0;
        
        for (int i = 0; i < settings.WaveSize; i++)
        {
            // Without this, if we have 199 enemies spawned and our max is 200, we could still potentially spawn a full wave
            if (_enemyCount > settings.MaxEnemyCount)
            {
                return;
            }
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Enemy enemyToSpawn = settings.RandomEnemy;
        GameObject instance = Instantiate(enemyToSpawn.Prefab);
        instance.transform.position = GetRandomInDonut(settings.MinSpawnRadius, settings.MaxSpawnRadius);

        EnemyController controller = instance.GetComponent<EnemyController>();
        controller.Setup(enemyToSpawn.Settings);
        _enemyCount++;
    }
    
    public Vector2 GetRandomInDonut(float minDistance, float maxDistance)
    {
        Vector2 point = Random.insideUnitCircle;
        point = point.normalized;
        point *= Random.Range(minDistance, maxDistance);

        return point;
    }
}