using System.Collections;
using TMPro;
using UnityEngine;
using Utils;
using DG.Tweening;

namespace Minigames.Fight
{
    public class EnemyMovementController : MovementController
    {
        public Transform target;
        public Rigidbody2D myRigidbody;
        public EnemyEntity myEntity;
    
        [SerializeField] private float idealDistanceFromPlayer;

        
        void Start()
        {
            myRigidbody = GetComponent<Rigidbody2D>();
            SetStartingMoveSpeed(myEntity.EnemyStats.moveSpeed);
            target = GameManager.PlayerEntity.transform;
        }

        protected virtual void Update()
        {
            TryMove();
        }

        protected virtual void TryMove()
        {
            Vector2 velocity = myRigidbody.velocity;
            Vector2 offset = target.position - transform.position;

            Vector2 targetVelocity;

            if (offset.magnitude > idealDistanceFromPlayer)
            {
                targetVelocity = offset.normalized * CurrentMoveSpeed;
            }
            else
            {
                targetVelocity = Vector2.zero;
            }

            velocity = Vector2.MoveTowards(velocity, targetVelocity, myEntity.EnemyStats.acceleration * Time.deltaTime);
        
            myRigidbody.velocity = velocity;
        }
        
        public void Knockback(Vector2 force)
        {
            myRigidbody.velocity = force;
        }
    }
}
