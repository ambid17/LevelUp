using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "EnemyList", menuName = "ScriptableObjects/Fight/EnemyList", order = 1)]
    [Serializable]
    public class EnemyList : ScriptableObject
    {
        public List<EntityBehaviorData> enemies;

        [ContextMenu("Generate IDs")]
        public void GenerateIDs()
        {
            foreach (var enemy in enemies.OfType<IHiveMind>())
            {
                enemy.Id = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }
        }
    }

    
}
