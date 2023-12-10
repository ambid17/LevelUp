using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace Minigames.Fight
{
    public class MovementController : MonoBehaviour
    {
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
            BaseMoveSpeed = moveSpeed;
            CurrentMoveSpeed = moveSpeed;
        }

        public virtual void ApplyMoveEffect(Effect effect)
        {
            switch (effect)
            {
                case SlowEffect slow:
                    moveEffects.Add(effect.GetType().Name, slow.slowAmount);
                    break;
            }

            RecalculateMoveSpeed();
        }

        public virtual void RemoveMoveEffect(Effect effect)
        {
            switch (effect)
            {
                case SlowEffect slow:
                    moveEffects.Remove(effect.GetType().Name);
                    break;
            }

            RecalculateMoveSpeed();
        }

        private void RecalculateMoveSpeed()
        {
            CurrentMoveSpeed = BaseMoveSpeed;
            foreach (var kvp in moveEffects)
            {
                CurrentMoveSpeed *= kvp.Value;
            }
        }
    }
}