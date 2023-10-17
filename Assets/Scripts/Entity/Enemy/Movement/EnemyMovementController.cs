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
            SetStartingMoveSpeed(_overriddenEntity.enemyStats.MoveSpeed);
        }
        
        public void Knockback(Vector2 force)
        {
            MyRigidbody2D.velocity = force;
        }
    }
}
