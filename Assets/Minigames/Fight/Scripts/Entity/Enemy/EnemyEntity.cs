using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyEntity : Entity
    {
        public EnemyStats EnemyStats;
        protected bool isMarkedForDeath;
        void Start()
        {

        }

        void Update()
        {

        }
        
        protected override void Die()
        {
            // Since we are shooting so many projectiles...
            // OnTriggerEnter() gets called in Projectile multiple times before we get destroyed
            // This prevents duplicate deaths
            if (isMarkedForDeath)
            {
                return;
            }
        
            isMarkedForDeath = true;
        
            GameManager.EnemySpawnManager.EnemyCount--;
            GameManager.CurrencyManager.EnemyKilled(EnemyStats.GoldValue);
        
            Destroy(gameObject);
        }
    }
}