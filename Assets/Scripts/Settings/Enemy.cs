using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    [CreateAssetMenu(fileName = "Enemy", menuName = "ScriptableObjects/Fight/Enemy", order = 1)]
    [Serializable]
    public class Enemy : ScriptableObject
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
