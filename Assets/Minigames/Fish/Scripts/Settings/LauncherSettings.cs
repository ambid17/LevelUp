using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "LauncherSettings", menuName = "ScriptableObjects/Fish/LauncherSettings", order = 1)]
    [Serializable]
    public class LauncherSettings : ScriptableObject
    {
        public float SlingshotStrength;
        public float SlingshotMaxDistance;

        public const int BlockLayer = 6;
        public const int ObstacleLayer = 7;
        public const int ProjectileProjectileLayer = 8;
        public const int HotProjectileLayer = 9;
        public const int WaterLayer = 10;
        public const int PowerupLayer = 11;
    }
}
