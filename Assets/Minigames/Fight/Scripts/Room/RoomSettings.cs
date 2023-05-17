using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "RoomSettings", menuName = "ScriptableObjects/Fight/RoomSettings", order = 1)]
    [Serializable]
    public class RoomSettings : ScriptableObject
    {
        public int minRooms;
        public int maxRooms;
        public RoomController startRoom;
        public List<RoomController> rooms;

        public RoomController GetRandomRoom()
        {
            int i = Random.Range(0, rooms.Count);
            return rooms[i];
        }
    }
}