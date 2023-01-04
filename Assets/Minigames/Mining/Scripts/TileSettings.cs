using Minigames.Fight;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Minigames.Mining
{
    [CreateAssetMenu(fileName = "TileSettings", menuName = "ScriptableObjects/Mining/TileSettings", order = 1)]
    [Serializable]
    public class TileSettings : ScriptableObject
    {
        public List<TileDescriptor> Tiles;


        // See this for more info:
        // https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
        [NonSerialized] private int _weightTotal;
        public TileDescriptor GetRandomTile()
        {
            if (_weightTotal == 0)
            {
                foreach(TileDescriptor tile in Tiles)
                {
                    _weightTotal += tile.SpawnWeight;
                }
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var tile in Tiles)
            {
                randomWeight -= tile.SpawnWeight;
                if (randomWeight < 0)
                {
                    return tile;
                }
            }

            return Tiles[0];
        }

        public void AddToInventory(TileType tileType)
        {
            Tiles.First(t => t.TileType == tileType).Count++;
        }
    }
    [Serializable]
    public class TileDescriptor
    {
        [Header("Set In Editor")]
        public MiningTile Tile;
        public bool Sellable;
        public float Value;
        public int SpawnWeight;
        public TileType TileType;
        [Header("Runtime Values")]
        public int Count;
    }

    public enum TileType
    {
        Air, Copper, Iron, Gold, Adamantium, Lava
    }
}

