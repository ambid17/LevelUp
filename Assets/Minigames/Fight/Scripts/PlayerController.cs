using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementToApply;
    private Vector2 _currentInput;
    private Camera _camera;
    private SpriteRenderer _spriteRenderer;
    
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float Acceleration = 20;
    [SerializeField] private float shotSpeed = 0.1f;
    
    private float shotTimer = 0;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
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
            projectile.SetDamage(10);

            
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectile.SetDirection(direction);
        }
    }

    private void Move()
    {
        _rigidbody2D.velocity = _movementToApply;

        //Flip the sprite based on velocity
        if(_rigidbody2D.velocity.x < 0) 
            _spriteRenderer.flipX = true;
        else 
            _spriteRenderer.flipX = false;
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
}
