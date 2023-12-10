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
    }
}