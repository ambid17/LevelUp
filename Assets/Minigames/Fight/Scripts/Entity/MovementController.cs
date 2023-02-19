using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Minigames.Fight
{
    public class MovementController : MonoBehaviour
    {
        protected float CurrentMoveSpeed;
        [NonSerialized]
        public Rigidbody2D MyRigidbody2D;
        protected Entity MyEntity;

        private void Awake()
        {
            MyRigidbody2D = GetComponent<Rigidbody2D>();
            MyEntity = GetComponent<Entity>();
        }

        protected virtual void SetStartingMoveSpeed(float moveSpeed)
        {
            CurrentMoveSpeed = moveSpeed;
        }

        public virtual void ApplyMoveEffect(float speedRatio)
        {
            CurrentMoveSpeed *= speedRatio;
        }

        public virtual void RemoveMoveEffect(float speedRatio)
        {
            CurrentMoveSpeed /= speedRatio;
        }
    }
}