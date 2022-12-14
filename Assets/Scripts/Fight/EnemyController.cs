using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private GameObject projectilePrefab;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private float shotSpeed;
    private float shotTimer;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float maxHp = 100;
    private float currentHp;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameManager.Instance.player.gameObject;
        currentHp = maxHp;
    }

    void Update()
    {
        TryShoot();
        TryMove();
    }

    void TryShoot()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > shotSpeed)
        {
            shotTimer = 0;
            
            GameObject projectileGO = Instantiate(projectilePrefab);
            projectileGO.transform.position = transform.position;
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.SetOwner(Projectile.OwnerType.Enemy);
            
            Vector2 direction = player.transform.position - transform.position;
            projectile.SetDirection(direction);
        }
    }

    void TryMove()
    {
        Vector2 velocity = Vector2.zero;
        Vector2 offset = player.transform.position - transform.position;

        if (offset.magnitude > 2)
        {
            velocity = offset.normalized * moveSpeed;
        }
        else
        {
            velocity = Vector2.zero;
        }
        
        _rigidbody2D.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == GameManager.ProjectileLayer)
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();

            if (projectile.Owner == Projectile.OwnerType.Player)
            {
                TakeDamage(projectile.Damage);
            }
        }
    }

    private void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
}
