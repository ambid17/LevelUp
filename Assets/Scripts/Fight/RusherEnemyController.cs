using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RusherEnemyController : EnemyController
{
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
    }
    override protected void TryShoot() { }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Melee!");
    }
}
