using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerProjectileWeaponController : PlayerWeaponController
{
        protected float ReloadTimer;

        private float projectileSpreadOffset;

        protected override void Start()
        {
            base.Start();
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
        }

        protected override void Update()
        {
            if (!isCurrentArm)
            {
                return;
            }
            base.Update();

            
            TryReload();

            if (CanShoot())
            {
                TryShoot();
            }

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
            return Input.GetKey(KeyCode.Mouse0) && IsEquipped && ShotTimer > weapon.fireRate && overridenWeapon.bulletsInMagazine > 0;
        }

        protected virtual void TryReload()
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
            overridenWeapon.bulletsInMagazine++;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
            ReloadTimer = 0;
        }

        public override void Shoot()
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