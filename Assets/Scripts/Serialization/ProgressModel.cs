using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ProgressModel
    {
        public List<BiomeData> BiomeData;
        public float Dna;
        public float BankedDna;
        public ResourceTypeFloatDictionary PhysicalResources;
    }

    public class BiomeData
    {
        public string BiomeName;
        public bool IsCompleted;
        public bool IsUnlocked;
        public int FloorsCompleted;
    }
}