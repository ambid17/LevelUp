using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public class ProjectileWeaponController : WeaponController
    {
        [SerializeField] private ProjectileWeapon overridenWeapon;

        private void Start()
        {
            overridenWeapon = _weapon as ProjectileWeapon;
        }

        void Update()
        {
            if (GameManager.PlayerStatusController.IsDead)
            {
                return;
            }
        
            _shotTimer += Time.deltaTime;

            if (Input.GetMouseButton(0) && _shotTimer > _weapon.Stats.FireRate)
            {
                _shotTimer = 0;
                Shoot();
            }
        }
        
        private void Shoot()
        {
            int projectileCount = overridenWeapon.Stats.ProjectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.ProjectilePrefab);
            
                Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * overridenWeapon.ProjectileSpread;
            
                projectile.transform.position = transform.position.AsVector2() + offset;

                float damage = CalculateDamage();
            
                projectile.Setup(damage, direction, overridenWeapon.Stats.ProjectilePenetration);
            }
        }
    }
}