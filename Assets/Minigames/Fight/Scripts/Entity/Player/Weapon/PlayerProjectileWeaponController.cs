using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerProjectileWeaponController : ProjectileWeaponController
{
        protected Camera Camera;
        protected float ReloadTimer;
        protected bool IsReloading;


        protected override void Start()
        {
            base.Start();
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
            Camera = Camera.main;
        }

        protected override void Update()
        {
            base.Update();

            if (IsReloading)
            {
                ReloadTimer += Time.deltaTime;
                TryReload();
            }

            if (CanUseWeaponAbility())
            {
                WeaponAbilityTimer = 0;
                EventService.Dispatch<PlayerUsedAbilityEvent>();
                UseWeaponAbility();
            }

        }

        protected override bool CanShoot()
        {
            return Input.GetMouseButton(0) && ShotTimer > weapon.fireRate && !IsReloading;
        }

        protected virtual void TryReload()
        {
            if (ReloadTimer < overridenWeapon.reloadTime)
            {
                return;
            }
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
            IsReloading = false;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));
        }

        protected override void Shoot()
        {
            PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;

            Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            projectile.transform.position = MyTransform.position.AsVector2();

            projectile.Setup(MyEntity, direction, this);

            CheckReload();
        }

        protected virtual void CheckReload()
        {
            overridenWeapon.bulletsInMagazine--;
            EventService.Dispatch(new PlayerAmmoUpdatedEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));

            if (overridenWeapon.bulletsInMagazine <= 0)
            {
                ReloadTimer = 0;
                IsReloading = true;
            }
        }
    }
}