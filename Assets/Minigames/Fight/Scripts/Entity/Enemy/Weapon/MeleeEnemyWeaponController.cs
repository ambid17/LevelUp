using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        private bool _isTouchingPlayer = false;
        
        protected override bool CanShoot()
        {
            return ShotTimer > weapon.Stats.FireRate && _isTouchingPlayer;
        }

        protected override void Shoot()
        {
            EventService.Dispatch(new OnPlayerDamageEvent(weapon.Stats.Damage));
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
