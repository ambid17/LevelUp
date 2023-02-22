using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class ProjectileWeaponController : WeaponController
    {
        [SerializeField] protected ProjectileWeapon overridenWeapon;
        protected Camera Camera;
        protected Transform MyTransform;
        protected float ReloadTimer;
        protected bool IsReloading;


        protected virtual void Start()
        {
            overridenWeapon = weapon as ProjectileWeapon;
            overridenWeapon.bulletsInMagazine = overridenWeapon.magazineSize;
            Camera = Camera.main;
            MyTransform = transform;
        }
        
        protected override void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;
            WeaponAbilityTimer += Time.deltaTime;

            if (IsReloading)
            {
                ReloadTimer += Time.deltaTime;
                TryReload();
            }
            
            if(CanShoot())
            {
                ShotTimer = 0;
                Shoot();
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
        }
        
        protected override void Shoot()
        {
            PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab);
        
            Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            projectile.transform.position = MyTransform.position.AsVector2();

            projectile.Setup(MyEntity, direction);

            CheckReload();
        }

        protected virtual void CheckReload()
        {
            overridenWeapon.bulletsInMagazine--;
            EventService.Dispatch(new PlayerUsedAmmoEvent(overridenWeapon.bulletsInMagazine, overridenWeapon.magazineSize));

            if (overridenWeapon.bulletsInMagazine <= 0)
            {
                ReloadTimer = 0;
                IsReloading = true;
            }
        }
    }
}