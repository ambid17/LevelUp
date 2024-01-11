using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyObjectPool : MonoBehaviour
    {
        public List<EntityBehaviorData> AllEnemies = new();
        public List<EntityBehaviorData> ActiveEnemies = new();
        public List<EntityBehaviorData> EnemiesToReEnable = new();

        public void DisableAllEnemies()
        {
            foreach(var enemy in ActiveEnemies)
            {
                enemy.gameObject.SetActive(false);
                EnemiesToReEnable.Add(enemy);
            }

            ActiveEnemies.Clear();
        }

        public void ReEnableAllEnemies()
        {
            foreach(var enemy in EnemiesToReEnable)
            {
                enemy.gameObject.SetActive(true);
                ActiveEnemies.Add(enemy);
            }

            EnemiesToReEnable.Clear();
        }
    }
}