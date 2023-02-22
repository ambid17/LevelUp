using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ShotgunWeaponController : ProjectileWeaponController
    {
        float startAngle = 45;
        float endAngle = 135;

        protected override void Shoot()
        {
            int projectileCount = overridenWeapon.stats.ProjectileCount;
            float angleStep = (endAngle - startAngle) / projectileCount;
            float angle = startAngle;
            
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.ProjectilePrefab);
                projectile.transform.position = MyTransform.position.AsVector2();

                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                direction.x *= Mathf.Cos(angle * Mathf.Deg2Rad);
                direction.y *= Mathf.Sin(angle * Mathf.Deg2Rad);
                

                projectile.Setup(MyEntity, direction, overridenWeapon.stats.ProjectilePenetration);

                angle += angleStep;
            }

            overridenWeapon.BulletsInMagazine--;

            if (overridenWeapon.BulletsInMagazine <= 0)
            {
                ReloadTimer = 0;
                IsReloading = true;
            }
        }
        
        protected override void UseWeaponAbility()
        {
            PlayerProjectile projectile = Instantiate(overridenWeapon.ProjectilePrefab);
            projectile.transform.position = MyTransform.position.AsVector2();

            Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            projectile.Setup(MyEntity, direction, overridenWeapon.stats.ProjectilePenetration);
        }
    }
}