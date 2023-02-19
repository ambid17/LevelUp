using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        bool isTouchingPlayer = false;
        
        protected override bool CanShoot()
        {
            return _shotTimer > _weapon.Stats.FireRate && isTouchingPlayer;
        }

        protected override void Shoot()
        {
            _eventService.Dispatch(new OnPlayerDamageEvent(_weapon.Stats.Damage));
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                isTouchingPlayer = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                isTouchingPlayer = false;
        }
    }
}
