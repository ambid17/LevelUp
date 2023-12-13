using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEditor.Animations;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EnemyEntity : Entity
    {
        public EnemyStats enemyStats;
        [NonSerialized]
        public Transform Target;

        [SerializeField]
        private AnimatorController deathAnimation;
        [SerializeField]
        private EnemyDeathAnimationPlayer deathAnimPlayerPrafab;

        private bool _isMarkedForDeath;

        protected override void Setup()
        {
            base.Setup();
            Target = GameManager.PlayerEntity.transform;

            // Magic float for now because I'm not sure flexibility will ever be necessary.
            // Offsets animation speed and movement speed by the same amount to make enemies feel more natural.
            float randomOffset = Random.Range(.9f, 1.1f);
            AnimationController.Anim.speed *= randomOffset;
        }


        protected override void Die(Entity killer)
        {
            base.Die(killer);
            // Since we are shooting so many projectiles...
            // OnTriggerEnter() gets called in Projectile multiple times before we get destroyed
            // This prevents duplicate deaths
            if (_isMarkedForDeath)
            {
                return;
            }
        
            _isMarkedForDeath = true;
        
            GameManager.CurrencyManager.EnemyKilled(enemyStats.goldValue);

            EnemyDeathAnimationPlayer deathAnimPlayer = Instantiate(deathAnimPlayerPrafab, transform.position, transform.rotation);
            deathAnimPlayer.Setup(VisualController.SpriteRenderer, deathAnimation);

            Destroy(gameObject);
        }
    }
}