using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [Serializable]
    public class RoomConnection
    {
        public string Label;
        public Vector2 Location;
        public Vector2 Direction;
        public List<Vector3Int> TilePositions = new();
        public bool HasConnection;
    }
}