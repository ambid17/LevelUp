using System.Collections;
using TMPro;
using UnityEngine;
using Utils;
using DG.Tweening;

namespace Minigames.Fight
{
    public class EnemyMovementController : MovementController
    {
        public Rigidbody2D myRigidbody;
        public EnemyEntity myEntity;
    
        [SerializeField] private float idealDistanceFromPlayer;

        
        void Start()
        {
            myEntity = GetComponent<EnemyEntity>();
            myRigidbody = GetComponent<Rigidbody2D>();
            SetStartingMoveSpeed(myEntity.enemyStats.moveSpeed);
        }

        protected virtual void Update()
        {
            TryMove();
        }

        protected virtual void TryMove()
        {
            Vector2 velocity = myRigidbody.velocity;
            Vector2 toPlayer = myEntity.target.position - transform.position;

            Vector2 targetVelocity;

            if (toPlayer.magnitude > idealDistanceFromPlayer)
            {
                targetVelocity = toPlayer.normalized * CurrentMoveSpeed;
            }
            else
            {
                targetVelocity = Vector2.zero;
            }

            velocity = Vector2.MoveTowards(velocity, targetVelocity, myEntity.enemyStats.acceleration * Time.deltaTime);
        
            myRigidbody.velocity = velocity;
        }
        
        public void Knockback(Vector2 force)
        {
            myRigidbody.velocity = force;
        }
    }
}
