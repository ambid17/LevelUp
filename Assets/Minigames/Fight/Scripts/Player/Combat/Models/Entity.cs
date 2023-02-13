using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class Entity : MonoBehaviour
    {
        public WeaponController WeaponController;
        public MovementController MovementController;

        public OrderedList<StatusEffectTracker> StatusEffects = new();

        private float _currentHp;
        private float _maxHp;

        protected EventService eventService;

        protected virtual void Awake()
        {
            eventService = GameManager.EventService;
            WeaponController = GetComponent<WeaponController>();
            MovementController = GetComponent<MovementController>();
        }

        private void Update()
        {
            foreach (var statusEffect in StatusEffects)
            {
                statusEffect.OnTick(Time.deltaTime);
            }
        }

        public void TakeDamage(float damage)
        {
        }
    }
}