using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyController : EnemyController
    {
        bool touchingPlayer = false;

        override protected void TryShoot() 
        { 
            if(touchingPlayer)
            {
                shotTimer += Time.deltaTime;
                if (shotTimer > settings.fireRate)
                {
                    shotTimer = 0;   
                    eventService.Dispatch(new OnPlayerDamageEvent(settings.weaponDamage));
                }
            }
        }
        

        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                touchingPlayer = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                touchingPlayer = false;
        }
    }
}
