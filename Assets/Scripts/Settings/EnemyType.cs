using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/Room/EnemyType", order = 1)]
    [Serializable]
    public class EnemyType : ScriptableObject
    {
        public EntityBehaviorData MyEnemyPrefab;
    }
}