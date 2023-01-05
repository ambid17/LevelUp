using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "MapSettings", menuName = "ScriptableObjects/Fish/MapSettings", order = 1)]
    [Serializable]
    public class MapSettings : ScriptableObject
    {
        public int MapWidth;
        public int MapHeight;

        public List<MapTile> BackgroundTiles;
        public List<MapTile> ObstacleTiles;

        
        [NonSerialized] private int _backgroundWeightTotal;
        [NonSerialized] private int _obstacleWeightTotal;
        
        public MapTile GetRandomObstacleTile()
        {
            if (_obstacleWeightTotal == 0)
            {
                _obstacleWeightTotal = ObstacleTiles.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _obstacleWeightTotal);
            foreach (var tile in ObstacleTiles)
            {
                randomWeight -= tile.SpawnWeight;
                if (randomWeight < 0)
                {
                    return tile;
                }
            }

            return ObstacleTiles[0];
        }
        

        public MapTile GetRandomBackgroundTile()
        {
            if (_backgroundWeightTotal == 0)
            {
                _backgroundWeightTotal = BackgroundTiles.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _backgroundWeightTotal);
            foreach (var tile in BackgroundTiles)
            {
                randomWeight -= tile.SpawnWeight;
                if (randomWeight < 0)
                {
                    return tile;
                }
            }

            return BackgroundTiles[0];
        }
    }

    [Serializable]
    public class MapTile
    {
        public Tile Tile;
        public int SpawnWeight;
    }
}