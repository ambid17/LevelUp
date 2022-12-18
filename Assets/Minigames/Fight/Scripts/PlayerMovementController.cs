using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementToApply;
    private Vector2 _currentInput;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite movingSprite;
    
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float Acceleration = 20;
    [SerializeField] private float shotSpeed = 0.1f;
    [SerializeField] private float shotDamage = 10;
    
    private float shotTimer = 0;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        GameManager.Instance.playerDidDie.AddListener(Die);
        GameManager.Instance.playerDidDie.AddListener(Revive);
    }

    void Update()
    {
        if (GameManager.Instance.IsDead)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }
        GetMovement();
        TryShoot();
        Move();
    }

    private void GetMovement()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            input.x += 1;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            input.x -= 1;
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            input.y += 1;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            input.y -= 1;
        }

        _currentInput = input * moveSpeed;
    }

    private void TryShoot()
    {
        shotTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && shotTimer > shotSpeed)
        {
            shotTimer = 0;
            
            GameObject projectileGO = Instantiate(projectilePrefab);
            projectileGO.transform.position = transform.position;
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.SetOwner(Projectile.OwnerType.Player);
            projectile.SetDamage(shotDamage);

            
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectile.SetDirection(direction);
        }
    }

    private void Move()
    {
        _rigidbody2D.velocity = _movementToApply;

        if (_rigidbody2D.velocity.magnitude == 0)
        {
            _animator.SetBool("IsMoving", false);
        }
        else
        {
            _animator.SetBool("IsMoving", true);
        }
        
        //Flip the sprite based on velocity
        if(_rigidbody2D.velocity.x < 0) 
            _spriteRenderer.flipX = false;
        else 
            _spriteRenderer.flipX = true;
    }
    
    private void FixedUpdate()
    {
        ApplyGroundAcceleration();
    }
    
    private void ApplyGroundAcceleration()
    {
        _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, Acceleration * Time.deltaTime);
        _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, Acceleration * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == GameManager.ProjectileLayer)
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();

            if (projectile.Owner == Projectile.OwnerType.Enemy)
            {
                GameManager.Instance.TakeDamage(projectile.Damage);
            }
        }
    }

    private void Revive()
    {
        _spriteRenderer.color = Color.white;
    }
    private void Die()
    {
        _spriteRenderer.color = Color.black;
    }
}
