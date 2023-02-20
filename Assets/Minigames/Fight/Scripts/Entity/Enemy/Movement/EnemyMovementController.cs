using System.Collections;
using TMPro;
using UnityEngine;
using Utils;
using DG.Tweening;

namespace Minigames.Fight
{
    public class EnemyMovementController : MovementController
    {
        private EnemyEntity _overriddenEntity;
    
        [SerializeField] private float idealDistanceFromPlayer;

        
        void Start()
        {
            _overriddenEntity = MyEntity as EnemyEntity;
            SetStartingMoveSpeed(_overriddenEntity.enemyStats.moveSpeed);
        }

        protected virtual void Update()
        {
            TryMove();
        }

        protected virtual void TryMove()
        {
            Vector2 velocity = MyRigidbody2D.velocity;
            Vector2 toPlayer = _overriddenEntity.Target.position - transform.position;

            Vector2 targetVelocity;

            if (toPlayer.magnitude > idealDistanceFromPlayer)
            {
                targetVelocity = toPlayer.normalized * CurrentMoveSpeed;
            }
            else
            {
                targetVelocity = Vector2.zero;
            }

            velocity = Vector2.MoveTowards(velocity, targetVelocity, _overriddenEntity.enemyStats.acceleration * Time.deltaTime);
        
            MyRigidbody2D.velocity = velocity;
        }
        
        public void Knockback(Vector2 force)
        {
            MyRigidbody2D.velocity = force;
        }
    }
}
