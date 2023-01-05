using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] FishSpawnSettings _fishSpawnSettings;
        private FishSettings _fishSettings;
        private float _lastSpawnDepth;
        
        void Start()
        {
            _fishSettings = GameManager.FishSettings;
        }

        void Update()
        {
            if (GameManager.GameState != GameState.Fling)
            {
                return;
            }
        
            TrySpawnFish();
        }
        
        private void TrySpawnFish()
        {
            bool noFishSpawned = _lastSpawnDepth == 0;
            bool hasReachedNextDepth = GameManager.CurrentLure.transform.position.y < _lastSpawnDepth;
            if (noFishSpawned || hasReachedNextDepth)
            {
                _lastSpawnDepth -= _fishSpawnSettings.DepthInterval;
                SpawnWave();
            }
        }

        private void SpawnWave()
        {
            for (int i = 0; i < _fishSpawnSettings.FishPerWave; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            Fish fishToSpawn = _fishSettings.GetRandomFish();
            
            GameObject instance = Instantiate(fishToSpawn.Prefab);
            instance.transform.position = GetRandomPosition();

            FishController controller = instance.GetComponent<FishController>();
            controller.Setup(fishToSpawn.InstanceSettings, GetFishSpawnBounds());
        }

        private Bounds GetFishSpawnBounds()
        {
            Bounds spawnBounds = new Bounds();
            
            float halfWidth = _fishSpawnSettings.SpawnAreaWidth / 2;

            Vector3 min = new Vector3(-halfWidth, _lastSpawnDepth, 0);
            Vector3 max = new Vector3(halfWidth, _lastSpawnDepth + _fishSpawnSettings.DepthInterval, 0);
            
            spawnBounds.SetMinMax(min, max);

            return spawnBounds;
        }
    
        public Vector2 GetRandomPosition()
        {
            Vector2 point = new Vector2();

            point.y = Random.Range(_lastSpawnDepth, _lastSpawnDepth + _fishSpawnSettings.DepthInterval);
            float halfWidth = _fishSpawnSettings.SpawnAreaWidth / 2;
            point.x = Random.Range(-halfWidth, halfWidth);

            return point;
        }
    }
}

