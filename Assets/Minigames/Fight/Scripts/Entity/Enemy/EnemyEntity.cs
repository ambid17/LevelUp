using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyEntity : Entity
    {
        public EnemyStats enemyStats;
        public Transform target;
        private bool _isMarkedForDeath;

        protected override void Setup()
        {
            target = GameManager.PlayerEntity.transform;
        }
        
        protected override void Die()
        {
            // Since we are shooting so many projectiles...
            // OnTriggerEnter() gets called in Projectile multiple times before we get destroyed
            // This prevents duplicate deaths
            if (_isMarkedForDeath)
            {
                return;
            }
        
            _isMarkedForDeath = true;
        
            GameManager.EnemySpawnManager.EnemyCount--;
            GameManager.CurrencyManager.EnemyKilled(enemyStats.GoldValue);
        
            Destroy(gameObject);
        }
    }
}