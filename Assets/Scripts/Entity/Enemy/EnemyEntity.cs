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

        [SerializeField]
        private AnimationName deathAnimation;
        [SerializeField]
        private EnemyDeathAnimationPlayer deathAnimPlayerPrafab;

        private bool _isMarkedForDeath;

        protected override void Setup()
        {
            base.Setup();
            Target = GameManager.PlayerEntity.transform;
            Stats.SetupFromEnemy(enemyStats);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
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

            EnemyDeathAnimationPlayer deathAnimPlayer = Instantiate(deathAnimPlayerPrafab, transform.position, transform.rotation);
            deathAnimPlayer.SpriteRenderer.flipX = VisualController.SpriteRenderer.flipX;
            deathAnimPlayer.SpriteRenderer.sortingLayerName = VisualController.SpriteRenderer.sortingLayerName;
            deathAnimPlayer.SpriteRenderer.sortingLayerID = VisualController.SpriteRenderer.sortingLayerID;

            Destroy(gameObject);
        }
    }
}