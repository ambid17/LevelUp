using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyProjectileWeaponController : WeaponController
    {
        [SerializeField] protected EnemyProjectile projectilePrefab;
        [SerializeField] protected Transform targetTransform;
        [SerializeField] protected EnemyStats stats;
        
        protected void Setup(EnemyStats stats, Transform targetTransform)
        {
            this.stats = stats;
            this.targetTransform = targetTransform;
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
            
            Vector2 direction = targetTransform.position - transform.position;

            projectile.Setup(stats, direction);
        }
    }
}