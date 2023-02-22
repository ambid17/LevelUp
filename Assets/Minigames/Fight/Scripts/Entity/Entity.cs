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
        public EntityStats Stats;
        
        public bool IsDead => Stats.currentHp <= 0;

        protected EventService eventService;

        protected virtual void Awake()
        {
            eventService = GameManager.EventService;
            WeaponController = GetComponent<WeaponController>();
            MovementController = GetComponent<MovementController>();
            VisualController = GetComponent<VisualController>();
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

            if (Stats.StatusEffectsToRemove.Count > 0)
            {
                RemoveStatuses();
            }
        }

        protected void TickStatuses()
        {
            foreach (var statusEffect in Stats.StatusEffects)
            {
                if (statusEffect.OnTick(Time.deltaTime))
                {
                    // Add this effect to our list to remove
                    // we can't remove during the for loop as that breaks the list enumeration
                    Stats.StatusEffectsToRemove.Add(statusEffect);
                }
            }
        }

        private void RemoveStatuses()
        {
            foreach (var statusEffect in Stats.StatusEffectsToRemove)
            {
                statusEffect.effect.RemoveEffect(this);
                Stats.StatusEffects.Remove(statusEffect);
            }
            Stats.StatusEffectsToRemove.Clear();
        }

        // Called when this entity hits another
        public virtual void OnHitOther(Entity otherEntity)
        {
            HitData hitData = new HitData(this, otherEntity);
            hitData.BaseDamage = WeaponController.Weapon.damage;
            hitData.Effects = Stats.OnHitEffects;
            otherEntity.TakeHit(hitData);
        }

        public virtual void TakeHit(HitData hit)
        {
            float damage = hit.CalculateDamage();
            Stats.currentHp -= damage;
            VisualController.StartDamageFx(damage);

            if (IsDead)
            {
                Die();
            }
        }

        protected virtual void Die()
        {
            
        }
    }
}