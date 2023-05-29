using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerProjectileWeaponController : ProjectileWeaponController
{
        protected float ReloadTimer;
        protected bool IsReloading;

        private float projectileSpreadOffset;

        protected override void Start()
        {
            base.Start();
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
        }

        protected override void Update()
        {
            base.Update();

            TryReload();

            if (CanUseWeaponAbility())
            {
                WeaponAbilityTimer = 0;
                EventService.Dispatch<PlayerUsedAbilityEvent>();
                UseWeaponAbility();
            }

        }
        public override void Setup(Weapon weapon)
        {
            base.Setup(weapon);
            // Calculate effect stuff.
        }
        protected override bool CanShoot()
        {
            return Input.GetMouseButton(0) && ShotTimer > weapon.fireRate && !IsReloading;
        }

        protected virtual void TryReload()
        {
            if (ReloadTimer < overridenWeapon.reloadTime || overridenWeapon.bulletsInMagazine == overridenWeapon.magazineSize)
            {
                ReloadTimer += Time.deltaTime;
                return;
            }
            overridenWeapon.bulletsInMagazine++;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
            ReloadTimer = 0;
        }

        protected override void Shoot()
        {
            // TODO: look at effects for this
            int projectileCount = 1;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;

                Vector2 direction = GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i / 2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * projectileSpreadOffset;

                projectile.transform.position = MyTransform.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction, this);
            }

            CheckReload();
        }

        protected virtual void CheckReload()
        {
            overridenWeapon.bulletsInMagazine--;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
            ReloadTimer = 0;
        }
    }
}