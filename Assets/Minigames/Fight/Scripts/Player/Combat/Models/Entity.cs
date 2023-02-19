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
        public VisualController VisualController;
        public EntityStats Stats;
        
        public bool IsDead => Stats.currentHp <= 0;

        protected EventService eventService;

        protected virtual void Awake()
        {
            eventService = GameManager.EventService;
            WeaponController = GetComponent<WeaponController>();
            MovementController = GetComponent<MovementController>();
            VisualController = GetComponent<VisualController>();
        }

        public void AssignStats(EntityStats stats)
        {
            Stats = stats;
        }

        protected virtual void Update()
        {
            TickStatuses();
        }

        protected void TickStatuses()
        {
            foreach (var statusEffect in Stats.StatusEffects)
            {
                statusEffect.OnTick(Time.deltaTime);
            }
        }

        public virtual void TakeDamage(float damage)
        {
            Stats.currentHp -= damage;
        }
    }
}