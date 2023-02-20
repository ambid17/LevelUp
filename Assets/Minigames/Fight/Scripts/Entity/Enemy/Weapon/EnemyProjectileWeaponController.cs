using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyProjectileWeaponController : WeaponController
    {
        [SerializeField] private EnemyProjectile projectilePrefab;
        private Transform _myTransform;
        private EnemyEntity _overridenEntity;

        private void Start()
        {
            _overridenEntity = MyEntity as EnemyEntity;
            _myTransform = transform; // cache our transform for performance
        }
        
        protected override bool ShouldPreventUpdate()
        {
            return false;
        }
        
        protected override bool CanShoot()
        {
            return ShotTimer > weapon.Stats.FireRate;
        }
        
        protected override void Shoot()
        {
            EnemyProjectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            
            Vector2 direction = _overridenEntity.Target.position - _myTransform.position;

            projectile.Setup(_overridenEntity.enemyStats, direction);
        }
    }
}