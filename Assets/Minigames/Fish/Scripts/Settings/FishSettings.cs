using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fish
{
    [CreateAssetMenu(fileName = "FishSettings", menuName = "ScriptableObjects/Fish/FishSettings", order = 1)]
    [Serializable]
    public class FishSettings : ScriptableObject
    {
        public List<Fish> Fish;
        
        // See this for more info:
        // https://limboh27.medium.com/implementing-weighted-rng-in-unity-ed7186e3ff3b
        [NonSerialized] private int _weightTotal;

        public Fish GetRandomFish()
        {
            if (_weightTotal == 0)
            {
                _weightTotal = Fish.Sum(e => e.SpawnWeight);
            }

            int randomWeight = UnityEngine.Random.Range(0, _weightTotal);
            foreach (var fish in Fish)
            {
                randomWeight -= fish.SpawnWeight;
                if (randomWeight < 0)
                {
                    return fish;
                }
            }

            return Fish[0];
        }
    }

    [Serializable]
    public class Fish
    {
        public GameObject Prefab;
        public int SpawnWeight;
        public FishInstanceSettings InstanceSettings;
    }

    [Serializable]
    public class FishInstanceSettings
    {
        public float goldValue;
        public float moveSpeed;
    }
}