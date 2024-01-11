using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "RoomSpriteSettings", menuName = "ScriptableObjects/Rooms/RoomSpriteSettings", order = 1)]
    [Serializable]
    public class RoomSpriteSettings : ScriptableObject
    {
        public float WallThickness;
        [SerializeField]
        public List<RoomSprite> RoomSprites;
    }

    [Serializable]
    public class RoomSprite
    {
        public PropType propType;
        public Sprite sprite;
    }
}