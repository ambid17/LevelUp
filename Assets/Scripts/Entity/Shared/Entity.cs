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
        public AnimationManager AnimationController;
        public WeaponController WeaponController;
        public EntityStats Stats;
        
        public bool IsDead => Stats.combatStats.currentHp <= 0;

        protected EventService eventService;

        [SerializeField]
        private bool tickDamageDownPerSecond;

        protected virtual void Awake()
        {
            eventService = Platform.EventService;
            MovementController = GetComponent<MovementController>();
            VisualController = GetComponent<VisualController>();
            AnimationController = GetComponent<AnimationManager>();
            Setup();
        }

        protected virtual void Setup()
        {
            Stats = new EntityStats();
        }
        
        protected virtual void Update()
        {
            Stats.TickStatuses();
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
            // Execute all of the onHit effects to populate the onHitDamage
            foreach (var effect in Stats.combatStats.OnHitEffects)
            {
                effect.Execute(this, target);
            }

            float damage = WeaponController.CurrentWeapon.baseDamage.Calculated 
                + WeaponController.CurrentWeapon.onHitDamage.Calculated;
            target.TakeHit(damage, this);
            
            // Clear the onHitDamage because it is only used once per hit as many of the effects
            // are dependent on target stats
            WeaponController.CurrentWeapon.onHitDamage.Clear();
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