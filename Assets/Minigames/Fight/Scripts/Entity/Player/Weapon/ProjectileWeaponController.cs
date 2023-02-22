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


        private void Start()
        {
            overridenWeapon = weapon as ProjectileWeapon;
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
                Reload();
            }
            else if(CanShoot())
            {
                ShotTimer = 0;
                Shoot();
            }

            if (CanUseWeaponAbility())
            {
                WeaponAbilityTimer = 0;
                UseWeaponAbility();
            }
        }
        
        protected virtual void Reload()
        {
            overridenWeapon.BulletsInMagazine = overridenWeapon.MagazineSize;
            IsReloading = false;
        }
        
        protected override void Shoot()
        {
            int projectileCount = overridenWeapon.stats.ProjectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.ProjectilePrefab);
            
                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * overridenWeapon.ProjectileSpread;
            
                projectile.transform.position = MyTransform.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction, overridenWeapon.stats.ProjectilePenetration);
            }

            overridenWeapon.BulletsInMagazine--;

            if (overridenWeapon.BulletsInMagazine <= 0)
            {
                ReloadTimer = 0;
                IsReloading = true;
            }
        }
    }
}