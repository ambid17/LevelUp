using UnityEngine;

namespace Minigames.Fight
{
    public class RusherEnemyController : EnemyController
    {
        bool touchingPlayer = false;

        override protected void TryShoot() 
        { 
            if(touchingPlayer)
            {
                shotTimer += Time.deltaTime;
                if (shotTimer > settings.shotSpeed)
                {
                    shotTimer = 0;   
                    GameManager.GameStateManager.TakeDamage(settings.weaponDamage);
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
