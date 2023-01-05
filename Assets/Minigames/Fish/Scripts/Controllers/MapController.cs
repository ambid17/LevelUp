using System.Collections;
using System.Collections.Generic;
using Minigames.Fish;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{
    [SerializeField] private MapSettings _mapSettings;
    [SerializeField] private Tilemap _backgroundTilemap;
    [SerializeField] private Tilemap _obstacleTilemap;
    void Start()
    {
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
            for (int y = -_mapSettings.MapHeight; y < 0; y++)
            {
                _backgroundTilemap.SetTile(new Vector3Int(x, y, 0), _mapSettings.GetRandomBackgroundTile().Tile);
                _obstacleTilemap.SetTile(new Vector3Int(x, y, 0), _mapSettings.GetRandomObstacleTile().Tile);
            }
        }
    }
}
