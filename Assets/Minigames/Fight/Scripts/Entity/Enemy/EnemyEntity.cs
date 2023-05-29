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

        public EntityAnimationController OverriddenAnimationController => _animationControllerOverride;

        private bool _isMarkedForDeath;
        private EntityAnimationController _animationControllerOverride;

        protected override void Setup()
        {
            base.Setup();
            Target = GameManager.PlayerEntity.transform;
            _animationControllerOverride = animationController as EntityAnimationController;
            Stats.SetupFromEnemy(enemyStats);
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            _animationControllerOverride.PlayTakeHitAnim();
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

            _animationControllerOverride.PlayDieAnim();
        
            Destroy(gameObject);
        }
    }
}