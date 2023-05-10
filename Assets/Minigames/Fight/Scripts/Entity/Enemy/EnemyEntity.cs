using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyEntity : Entity
    {
        public EnemyStats enemyStats;
        [NonSerialized]
        public Transform Target;
        private bool _isMarkedForDeath;

        protected override void Setup()
        {
            base.Setup();
            Target = GameManager.PlayerEntity.transform;
            Stats.SetupFromEnemy(enemyStats);
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
        
            GameManager.CurrencyManager.EnemyKilled(enemyStats.GoldValue);
        
            Destroy(gameObject);
        }
    }
}