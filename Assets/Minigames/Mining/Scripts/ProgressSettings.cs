using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Minigames.Mining
{
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/Mining/ProgressSettings", order = 1)]
    [Serializable]
    public class ProgressSettings : ScriptableObject
    {
        public List<InventoryItem> Inventory;
    }

    public class InventoryItem
    {
        public TileType TileType;
        public int Count;
    }
}

