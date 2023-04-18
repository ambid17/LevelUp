using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class RocketLauncherWeaponController : ProjectileWeaponController
    {
        private float projectileSpreadOffset = 0.3f;
        
        protected override void Shoot()
        {
            int projectileCount = 1;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;
            
                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * projectileSpreadOffset;
            
                projectile.transform.position = MyTransform.position.AsVector2() + offset;
                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                projectile.Setup(MyEntity, direction);
            }

            CheckReload();
        }
        
        protected override void UseWeaponAbility()
        {
            Transform closestEnemy = GetClosestEnemy();
            int projectileCount = 1;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;

                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                if (closestEnemy != null)
                {
                    direction = closestEnemy.position - transform.position;
                }
                

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * projectileSpreadOffset;
            
                projectile.transform.position = MyTransform.position.AsVector2() + offset;
                
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                projectile.Setup(MyEntity, direction);
            }
        }
        
        // TODO: handle this in the projectile using physics2d.overlapCirlce()
        private Transform GetClosestEnemy ()
        {
            EnemyEntity[] enemies = FindObjectsOfType<EnemyEntity>();
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            for(int i = 0 ; i < enemies.Length; i++)
            {
                Transform potentialTarget = enemies[i].transform;
                Vector3 directionToTarget = potentialTarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if(dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }
     
            return bestTarget;
        }
    }
}