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
        [SerializeField] protected EnemyInstanceSettings settings;
        
        protected void Setup(EnemyInstanceSettings settings, Transform targetTransform)
        {
            this.settings = settings;
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

            projectile.Setup(settings, direction);
        }
    }
}