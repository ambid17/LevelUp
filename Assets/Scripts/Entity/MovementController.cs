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
        private float BaseMoveSpeed;
        private Dictionary<string,float> moveEffects;
        [NonSerialized]
        public Rigidbody2D MyRigidbody2D;
        protected Entity MyEntity;

        private void Awake()
        {
            MyRigidbody2D = GetComponent<Rigidbody2D>();
            MyEntity = GetComponent<Entity>();
            moveEffects = new();
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
                    moveEffects.Add(slow.Name, slow.slowAmount);
                    break;
            }

            RecalculateMoveSpeed();
        }

        public virtual void RemoveMoveEffect(Effect effect)
        {
            switch (effect)
            {
                case SlowEffect slow:
                    moveEffects.Remove(slow.Name);
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