using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PistolWeaponController : ProjectileWeaponController
    {
        private float projectileSpreadOffset = 0.15f;
        
        protected override void Shoot()
        {
            // TODO: look at effects for this
            int projectileCount = 1;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab);
            
                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * projectileSpreadOffset;
            
                projectile.transform.position = MyTransform.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction);
            }

            CheckReload();
        }
        
        
        float startAngle = -10;
        float endAngle = 10;
        
        protected override void UseWeaponAbility()
        {
            for (int i = overridenWeapon.bulletsInMagazine; i >= 0; i--)
            {
                // TODO: apply status effects twice as effective
                int projectileCount = 1;
                float angle = Random.Range(startAngle, endAngle);
            
                for (int j = 0; j < projectileCount; j++)
                {
                    PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab);
                    projectile.transform.position = MyTransform.position.AsVector2();

                    Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                    direction = direction.Rotate(angle);
                    
                    float indexOffset = j - (float)j/2;
                    Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * projectileSpreadOffset;
                    projectile.transform.position = MyTransform.position.AsVector2() + offset;

                    projectile.Setup(MyEntity, direction);
                }
            }
            
            overridenWeapon.bulletsInMagazine = 0;
            ReloadTimer = 0;
            IsReloading = true;
        }
    }
}