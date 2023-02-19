using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyProjectileWeaponController : WeaponController
    {
        [SerializeField] protected EnemyProjectile projectilePrefab;
        [SerializeField] protected Transform myTransform;
        [SerializeField] protected EnemyEntity overridenEntity;

        private void Start()
        {
            overridenEntity = myEntity as EnemyEntity;
            myTransform = transform; // cache our transform for performance
        }
        
        protected override bool ShouldPreventUpdate()
        {
            return false;
        }
        
        protected override bool CanShoot()
        {
            return _shotTimer > _weapon.Stats.FireRate;
        }
        
        protected override void Shoot()
        {
            EnemyProjectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            
            Vector2 direction = overridenEntity.target.position - myTransform.position;

            projectile.Setup(overridenEntity.enemyStats, direction);
        }
    }
}