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
        public WeaponController WeaponController;
        public MovementController MovementController;
        public VisualController VisualController;
        public EntityAnimationController animationController;
        public EntityStats Stats;
        
        public bool IsDead => Stats.currentHp <= 0;

        protected EventService eventService;

        [SerializeField]
        private bool tickDamageDownPerSecond;

        protected virtual void Awake()
        {
            eventService = GameManager.EventService;
            WeaponController = GetComponent<WeaponController>();
            MovementController = GetComponent<MovementController>();
            VisualController = GetComponent<VisualController>();
            animationController = GetComponent<EntityAnimationController>();
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

            UpdateDps();
        }

        private void UpdateDps()
        {
            Stats.dpsTimer += Time.deltaTime;

            if (Stats.dpsTimer > 1)
            {
                Stats.dpsTimer = 0;

                // Option to reduce damage per second over time instead of resetting it entirely.
                if (tickDamageDownPerSecond && Stats.damageTakenThisSecond > 0)
                {
                    Stats.damageTakenThisSecond -= 1;
                    return;
                }
                Stats.damageTakenThisSecond = 0;
            }
        }

        protected void TickStatuses()
        {
            foreach (var statusEffect in Stats.StatusEffects.ToList())
            {
                statusEffect.OnTick(Time.deltaTime);
            }
        }

        // Called when this entity hits another
        public virtual void OnHitOther(Entity otherEntity)
        {
            // TODO: performance optimization is to create this once and update it only when the stats are updated
            HitData hitData = new HitData(this, otherEntity);
            otherEntity.TakeHit(hitData);
        }

        public virtual void TakeHit(HitData hit)
        {
            animationController.PlayTakeHitAnim();

            float damage = hit.CalculateDamage();
            TakeDamage(damage);
        }

        public virtual void TakeDamage(float damage)
        {
            Stats.TakeDamage(damage);

            // Damage FX are confusing if a hit only applies status effects.
            if (damage > 0)
            {
                VisualController.StartDamageFx(damage);
            }

            if (IsDead)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            animationController.PlayDieAnim();
        }
    }
}