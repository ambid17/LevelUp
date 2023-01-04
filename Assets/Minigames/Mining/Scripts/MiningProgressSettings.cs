using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace Minigames.Mining
{
    [CreateAssetMenu(fileName = "ProgressSettings", menuName = "ScriptableObjects/Mining/ProgressSettings", order = 1)]
    [Serializable]
    public class MiningProgressSettings : ScriptableObject
    {
        [Header("Set In Editor")]
        public float MaxFuel;
        public float MaxHealth;
        public float FuelCost;
        public float HealthCost;
        public float MoveSpeed;

        [Header("Runtime Value")]
        public float FuelAmount;
        public float HullHealth;
    }
}

