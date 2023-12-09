using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerProjectileWeaponController : PlayerWeaponController
{
        protected float ReloadTimer;

        protected override void Start()
        {
            base.Start();
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
        }

        protected override void Update()
        {
            if (!isCurrentArm || MyEntity.IsDead)
            {
                return;
            }
            
            base.Update();
            
            TryRegenAmmo();

            if (CanShoot())
            {
                _overridenEntity.WeaponArmController.PlayShootAnimation();
            }
        }

        public override void Setup(Weapon weapon)
        {
            base.Setup(weapon);
            // Calculate effect stuff.
        }

        protected override bool CanShoot()
        {
            return Input.GetKey(KeyCode.Mouse0) && IsEquipped && ShotTimer > weapon.fireRate && overridenWeapon.bulletsInMagazine > 0;
        }

        protected virtual void TryRegenAmmo()
        {
            if (overridenWeapon.bulletsInMagazine == overridenWeapon.magazineSize)
            {
                return;
            }

            if (ReloadTimer < overridenWeapon.reloadTime)
            {
                ReloadTimer += Time.deltaTime;
                return;
            }

            // slowly add bullet to mag over time
            overridenWeapon.bulletsInMagazine++;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
            ReloadTimer = 0;
        }

        public override void Shoot()
        {
            for (int i = 0; i < overridenWeapon.projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;

                Vector2 direction = GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i / 2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * overridenWeapon.projectileSpread;

                projectile.transform.position = _overridenEntity.WeaponArmController.CurrentArm.ProjectileOrigin.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction, this);
            }

            CheckReload();
            ShotTimer = 0;
        }

        protected virtual void CheckReload()
        {
            overridenWeapon.bulletsInMagazine--;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
            ReloadTimer = 0;
        }
    }
}