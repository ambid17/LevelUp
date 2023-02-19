using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        bool isTouchingPlayer = false;
        
        protected override bool ShouldPreventUpdate()
        {
            return false;
        }
        
        protected override bool CanShoot()
        {
            return _shotTimer > _weapon.Stats.FireRate && isTouchingPlayer;
        }

        protected override void Shoot()
        {
            _eventService.Dispatch(new OnPlayerDamageEvent(_weapon.Stats.Damage));
        }

        protected virtual float CalculateDamage()
        {
            float damage = _weapon.Stats.Damage;

            if (_weapon.Stats.CritChance > 0)
            {
                float randomValue = Random.Range(0f, 1f);
                bool shouldCrit = randomValue < _weapon.Stats.CritChance;

                if (shouldCrit)
                {
                    damage *= _weapon.Stats.CritDamage;
                }
            }

            return damage;
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
