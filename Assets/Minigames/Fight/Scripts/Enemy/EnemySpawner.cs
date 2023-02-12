using UnityEngine;

namespace Minigames.Fight
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemySpawnerSettings _spawnerSettings => GameManager.SettingsManager.enemySpawnerSettings;
        private ProgressSettings _progressSettings => GameManager.SettingsManager.progressSettings;

        private float waveTimer;
        [SerializeField] private int _enemyCount;

        public int EnemyCount
        {
            get => _enemyCount;
            set => _enemyCount = value;
        }

        void Start()
        {
        }

        void Update()
        {
            if (GameManager.PlayerStatusController.IsDead)
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
                if (waveTimer < _spawnerSettings.WaveInterval || _enemyCount > _spawnerSettings.MaxEnemyCount)
                {
                    return;
                }
            }

            waveTimer = 0;
        
            for (int i = 0; i < _spawnerSettings.WaveSize; i++)
            {
                // Without this, if we have 199 enemies spawned and our max is 200, we could still potentially spawn a full wave
                if (_enemyCount > _spawnerSettings.MaxEnemyCount)
                {
                    return;
                }
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Enemy enemyToSpawn = _progressSettings.CurrentWorld.GetRandomEnemy();
            GameObject instance = Instantiate(enemyToSpawn.Prefab);
            instance.transform.position = GetRandomInDonut(_spawnerSettings.MinSpawnRadius, _spawnerSettings.MaxSpawnRadius);

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
}