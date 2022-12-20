using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
            touchingPlayer = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
            touchingPlayer = false;
    }
}
