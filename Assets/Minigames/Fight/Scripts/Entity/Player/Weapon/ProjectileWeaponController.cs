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
        private Camera _camera;
        private Transform _myTransform;
        
        private void Start()
        {
            overridenWeapon = weapon as ProjectileWeapon;
            _camera = Camera.main;
            _myTransform = transform;
        }
        
        protected override void Shoot()
        {
            int projectileCount = overridenWeapon.Stats.ProjectileCount;
            for (int i = 0; i < projectileCount; i++)
            {
                PlayerProjectile projectile = Instantiate(overridenWeapon.ProjectilePrefab);
            
                Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i/2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * overridenWeapon.ProjectileSpread;
            
                projectile.transform.position = _myTransform.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction, overridenWeapon.Stats.ProjectilePenetration);
            }
        }
    }
}