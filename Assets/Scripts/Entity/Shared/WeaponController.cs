using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Minigames.Fight
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponMode CurrentWeaponMode = WeaponMode.Projectile;
        public WeaponStats CurrentWeapon => CurrentWeaponMode == WeaponMode.Projectile ? _combatStats.projectileWeaponStats : _combatStats.meleeWeaponStats;

        protected CombatStats _combatStats => MyEntity.Stats.combatStats;

        protected float ShootTimer;
        protected float MeleeTimer;

        protected Entity MyEntity;

        void Awake()
        {
            MyEntity = GetComponent<Entity>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            ShootTimer += Time.deltaTime;
            MeleeTimer += Time.deltaTime;
        }
        
        public virtual bool CanShoot()
        {
            return true;
        }

        public virtual bool CanMelee()
        {
            return true;
        }

        public virtual void Shoot()
        {

        }

        public virtual void Melee()
        {

        }
    }
}
