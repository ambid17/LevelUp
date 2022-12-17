using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemyController : EnemyController
{
    bool touchingPlayer = false;

    override protected void TryMove()
    {
        Vector2 velocity = Vector2.zero;
        Vector2 offset = player.transform.position - transform.position;

        if (offset.magnitude > 0f)
        {
            velocity = offset.normalized * moveSpeed;
        }
        else
        {
            velocity = Vector2.zero;
        }
        
        _rigidbody2D.velocity = velocity;
        FlipSpriteOnDirection();
    }
    override protected void TryShoot() 
    { 
        if(touchingPlayer)
        {
            shotTimer += Time.deltaTime;
            if (shotTimer > shotSpeed)
            {
                shotTimer = 0;   
                Debug.Log("Melee attack!");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == GameManager.PlayerLayer)
            touchingPlayer = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject.layer == GameManager.PlayerLayer)
            touchingPlayer = false;
    }
}
