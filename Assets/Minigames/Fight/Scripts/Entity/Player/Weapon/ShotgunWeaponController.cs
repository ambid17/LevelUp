using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class ShotgunWeaponController : PlayerProjectileWeaponController
    {
        // TODO: scale the angle with the number of projectiles
        float startAngle = -30;
        float endAngle = 30;

        protected override void Shoot()
        {
            int projectileCount = 4;
            float angleStep = (endAngle - startAngle) / (projectileCount - 1);
            float angle = startAngle;
            
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;
                projectile.transform.position = MyTransform.position.AsVector2();

                Vector2 direction = GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                direction = direction.Rotate(angle);
                

                projectile.Setup(MyEntity, direction, this);

                angle += angleStep;
            }

            CheckReload();
        }
        
        
        
        protected override void UseWeaponAbility()
        {
            PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;
            projectile.transform.position = MyTransform.position.AsVector2();

            Vector2 direction = GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            
            projectile.Setup(MyEntity, direction, this);
        }
    }
}