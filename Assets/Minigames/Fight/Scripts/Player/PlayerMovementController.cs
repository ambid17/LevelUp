using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private Vector2 _movementToApply;
    private Vector2 _currentInput;
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        
        GameManager.GameStateManager.playerDidDie.AddListener(Die);
        GameManager.GameStateManager.playerDidRevive.AddListener(Revive);
    }

    void Update()
    {
        if (GameManager.GameStateManager.IsDead)
        {
            _rigidbody2D.velocity = Vector2.zero;
            return;
        }
        
        GetMovementInput();
        Move();
    }

    private void GetMovementInput()
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

        _currentInput = input.normalized * GameManager.UpgradeManager.playerSettings.MoveSpeed;
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
        float maxAcceleration = GameManager.UpgradeManager.playerSettings.Acceleration * Time.deltaTime;
        _movementToApply.x = Mathf.MoveTowards(_movementToApply.x, _currentInput.x, maxAcceleration);
        _movementToApply.y = Mathf.MoveTowards(_movementToApply.y, _currentInput.y, maxAcceleration);
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == PhysicsUtils.ProjectileLayer)
        {
            Projectile projectile = col.gameObject.GetComponent<Projectile>();

            if (projectile.Owner == Projectile.OwnerType.Enemy)
            {
                Destroy(projectile.gameObject);
                GameManager.GameStateManager.TakeDamage(projectile.Damage);
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
