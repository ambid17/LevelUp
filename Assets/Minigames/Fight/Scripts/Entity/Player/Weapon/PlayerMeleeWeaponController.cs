using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerMeleeWeaponController : PlayerWeaponController
    {
        
        protected override bool CanShoot()
        {
            return Input.GetKey(KeyCode.Mouse0) && IsEquipped && ShotTimer > weapon.fireRate;
        }
        protected override void Update()
        {
            if (!isCurrentArm)
            {
                return;
            }
            base.Update();
            if (CanShoot())
            {
                TryShoot();
            }
        }
        public override void Shoot()
        {
            PlayerMeleeAoE melee = Instantiate(overridenWeapon.projectilePrefab, GameManager.PlayerEntity.transform) as PlayerMeleeAoE;
            melee.transform.position = _overridenEntity.WeaponArmController.CurrentArm.ProjectileOrigin.position.AsVector2();
            melee.Setup(MyEntity, Vector2.zero, this);
            ShotTimer = 0;
        }
    }
}