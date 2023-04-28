using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MachineGunWeaponController : PlayerProjectileWeaponController
    {
        private readonly float _startAngle = -15;
        private readonly float _endAngle = 15;
        private float _startingFireRate;
        private PlayerEntity _overridenEntity;
        private float _abilityDurationTimer;
        private readonly float _abilityDuration = 3;
        private bool _isUsingAbility;
        
        protected override void Start()
        {
            base.Start();
            _overridenEntity = MyEntity as PlayerEntity;
        }
        
        protected override void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;
            WeaponAbilityTimer += Time.deltaTime;

            if (_isUsingAbility)
            {
                _abilityDurationTimer += Time.deltaTime;
                TryStopUsingAbility();
            }

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
            return Input.GetMouseButton(0) && ShotTimer > weapon.fireRate && (!IsReloading || _isUsingAbility);
        }
        

        protected override void Shoot()
        {
            int projectileCount = 1;
            
            for (int i = 0; i < projectileCount; i++)
            {
                float angle = Random.Range(_startAngle, _endAngle);

                PlayerProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as PlayerProjectile;
                projectile.transform.position = MyTransform.position.AsVector2();

                Vector2 direction = Camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
                direction = direction.Rotate(angle);
                

                projectile.Setup(MyEntity, direction);

            }

            if (!_isUsingAbility)
            {
                CheckReload();
            }
        }
        
        // disable movement, double fire rate
        protected override void UseWeaponAbility()
        {
            _startingFireRate = weapon.fireRate;
            weapon.fireRate /= 2;
            _overridenEntity.CanMove = false;
            _isUsingAbility = true;
            _abilityDurationTimer = 0;
        }

        private void TryStopUsingAbility()
        {
            if (_abilityDurationTimer > _abilityDuration)
            {
                _overridenEntity.CanMove = true;
                weapon.fireRate = _startingFireRate;
                _isUsingAbility = false;
            }
        }
    }
}