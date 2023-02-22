using System;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        private bool _isTouchingPlayer = false;


        protected override bool CanShoot()
        {
            return ShotTimer > weapon.fireRate && _isTouchingPlayer;
        }

        protected override void Shoot()
        {
            MyEntity.OnHitOther(GameManager.PlayerEntity);
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                _isTouchingPlayer = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                _isTouchingPlayer = false;
        }
    }
}
