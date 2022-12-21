using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected Transform playerTransform;
    [SerializeField] protected GameObject projectilePrefab;
    protected Rigidbody2D _rigidbody2D;
    protected SpriteRenderer _spriteRenderer;
    
    protected float shotTimer;
    protected float currentHp;
    protected EnemyInstanceSettings settings;

    [SerializeField] private float idealDistanceFromPlayer;

    private const float MaxDistanceFromPlayer = 100; 

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerTransform = GameManager.Player.transform;
        GameManager.GameStateManager.playerDidDie.AddListener(() => Die(false));
    }

    public void Setup(EnemyInstanceSettings settings)
    {
        this.settings = settings;
        currentHp = settings.maxHp;
    }

    protected virtual void Update()
    {
        TryShoot();
        TryMove();
        TryCull();
    }

    protected virtual void TryShoot()
    {
        shotTimer += Time.deltaTime;
        if (shotTimer > settings.shotSpeed)
        {
            shotTimer = 0;
            
            GameObject projectileGO = Instantiate(projectilePrefab);
            projectileGO.transform.position = transform.position;
            
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            Vector2 direction = playerTransform.position - transform.position;

            projectile.SetupForEnemy(settings.weaponDamage, direction);
        }
    }

    protected virtual void TryMove()
    {
        Vector2 velocity = Vector2.zero;
        Vector2 offset = playerTransform.position - transform.position;

        if (offset.magnitude > idealDistanceFromPlayer)
        {
            velocity = offset.normalized * settings.moveSpeed;
        }
        else
        {
            velocity = Vector2.zero;
        }
        
        _rigidbody2D.velocity = velocity;

        FlipSpriteOnDirection();
    }

    // If the player runs too far from the enemy, kill it off
    private void TryCull()
    {
        Vector2 offsetFromPlayer = playerTransform.position - transform.position;
        if (offsetFromPlayer.magnitude > MaxDistanceFromPlayer)
        {
            Die(false);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die(true);
        }
    }

    protected void  FlipSpriteOnDirection()
    {
        //Flip the sprite based on velocity
        if(_rigidbody2D.velocity.x < 0) 
            _spriteRenderer.flipX = true;
        else 
            _spriteRenderer.flipX = false;
    }

    private void Die(bool shouldAwardCurrency)
    {
        GameManager.EnemySpawner.EnemyCount--;

        if (shouldAwardCurrency)
        {
            GameManager.GameStateManager.AddCurrency(settings.goldValue);
        }
        Destroy(gameObject);
    }
}
