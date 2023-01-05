using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "FishSpawnSettings", menuName = "ScriptableObjects/Fish/FishSpawnSettings", order = 1)]
    [Serializable]
    public class FishSpawnSettings : ScriptableObject
    {
        public float DepthInterval;
        public int FishPerWave;
        public float SpawnAreaWidth;
    }

}
