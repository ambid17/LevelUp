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
            foreach (var statusEffect in Stats.StatusEffects)
            {
                statusEffect.OnTick(Time.deltaTime);
            }
        }

        // Called when this entity hits another
        public virtual void OnHitEnemy(Entity enemyEntity)
        {
            HitData hitData = new HitData(this, enemyEntity);
            hitData.BaseDamage = WeaponController.Weapon.Stats.Damage;
            hitData.Effects = Stats.OnHitEffects;
            enemyEntity.TakeHit(hitData);
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