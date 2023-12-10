using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class Entity : MonoBehaviour
    {
        public bool Stunned { get; set; }
        public MovementController MovementController;
        public VisualController VisualController;
        public AnimationManager animationController;
        public EntityStats Stats;
        
        public bool IsDead => Stats.currentHp <= 0;

        protected EventService eventService;

        [SerializeField]
        private bool tickDamageDownPerSecond;

        protected virtual void Awake()
        {
            eventService = Platform.EventService;
            MovementController = GetComponent<MovementController>();
            VisualController = GetComponent<VisualController>();
            animationController = GetComponent<AnimationManager>();
            Setup();
        }

        protected virtual void Setup()
        {
            Stats = new EntityStats();
        }
        
        protected virtual void Update()
        {
            if (Stats.StatusEffects.Count > 0)
            {
                TickStatuses();
            }
        }

        protected void TickStatuses()
        {
            foreach (var statusEffect in Stats.StatusEffects.ToList())
            {
                statusEffect.OnTick(Time.deltaTime);
            }
        }
        public virtual void TakeHit(float damage, Entity hitter)
        {
            Stats.TakeDamage(damage);

            VisualController.StartDamageFx(damage);

            if (IsDead)
            {
                Die(hitter);
            }
        }
        public void DealDamage(Entity target)
        {
            foreach (var effect in Stats.combatStats.OnHitEffects)
            {
                effect.Execute(this, target);
            }
            float damage = Stats.combatStats.baseDamage.Calculated + Stats.combatStats.onHitDamage.Calculated;
            target.TakeHit(damage, this);
            Stats.combatStats.onHitDamage.Clear();
        }

        protected virtual void OnKill()
        {
            // Execute OnKillEffects.
        }

        protected virtual void Die(Entity killer)
        {
            killer.OnKill();
        }
    }
}