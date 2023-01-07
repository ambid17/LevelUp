using System.Collections;
using System.Collections.Generic;
using Minigames.Fish;
using UnityEngine;
using UnityEngine.Tilemaps;
using Utils;

namespace Minigames.Fish
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private MapSettings _mapSettings;
        private FishSpawnSettings _fishSpawnSettings;
        private EventService _eventService;

        [SerializeField] private Tilemap _backgroundTilemap;
        [SerializeField] private Tilemap _obstacleTilemap;

        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<LureNextDepthEvent>(GenerateMap);
            _fishSpawnSettings = GameManager.FishSpawnSettings;
            GenerateMap();
        }

        void Update()
        {
            // TODO: generate as the player dives down
        }

        private void GenerateMap()
        {
            for (int x = -_mapSettings.MapWidth / 2; x < _mapSettings.MapWidth / 2; x++)
            {
                for (int y = GameManager.FishSpawner.LastSpawnDepth - _fishSpawnSettings.DepthInterval;
                     y < GameManager.FishSpawner.LastSpawnDepth;
                     y++)
                {
                    _backgroundTilemap.SetTile(new Vector3Int(x, y, 0), _mapSettings.GetRandomBackgroundTile().Tile);
                    // TODO: generate obstacles as a group of tiles put together using some sort of noise
                    //_obstacleTilemap.SetTile(new Vector3Int(x, y, 0), _mapSettings.GetRandomObstacleTile().Tile);
                }
            }
        }
    }
}