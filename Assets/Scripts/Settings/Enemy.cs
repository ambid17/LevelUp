using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Fight/Enemy", order = 1)]
    [Serializable]
    public class Enemy : ScriptableObject
    {
        public GameObject Prefab;
        public int SpawnWeight;
    }
}
