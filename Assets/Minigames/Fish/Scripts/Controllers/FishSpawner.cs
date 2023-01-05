using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Minigames.Fish
{
    public class FishSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _fishParent;
        
        private FishSpawnSettings _fishSpawnSettings;
        private FishSettings _fishSettings;
        private EventService _eventService;
        private int _lastSpawnDepth;
        public int LastSpawnDepth => _lastSpawnDepth;
        
        void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<ReeledInEvent>(OnReeledIn);
            _fishSettings = GameManager.FishSettings;
            _fishSpawnSettings = GameManager.FishSpawnSettings;
        }

        void Update()
        {
            if (GameManager.GameState != GameState.Fling || GameManager.CurrentLure == null)
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
                _eventService.Dispatch<LureNextDepthEvent>();
            }
        }

        private void SpawnWave()
        {
            for (int i = 0; i < _fishSpawnSettings.FishPerWave; i++)
            {
                SpawnFish();
            }
        }

        private void SpawnFish()
        {
            Fish fishToSpawn = _fishSettings.GetRandomFish();
            
            GameObject instance = Instantiate(fishToSpawn.Prefab, _fishParent);

            Bounds spawnBounds = GetFishSpawnBounds();
            instance.transform.position = spawnBounds.RandomPointInBounds();

            FishController controller = instance.GetComponent<FishController>();
            controller.Setup(fishToSpawn.InstanceSettings, spawnBounds);
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

        private void OnReeledIn()
        {
            _lastSpawnDepth = 0;
        }
    }
}

