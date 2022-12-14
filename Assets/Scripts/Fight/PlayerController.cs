using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float moveSpeed = 5;
    private Rigidbody2D _rigidbody2D;
    private Vector2 _movementToApply;
    private Camera _camera;

    [SerializeField] private float shotSpeed;
    private float shotTimer;
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    void Update()
    {
        GetMovement();
        TryShoot();
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

        _movementToApply = input;
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

            
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectile.SetDirection(direction);
        }
    }

    private void FixedUpdate()
    {
        if (_movementToApply.magnitude > 0)
        {
            _rigidbody2D.AddForce(_movementToApply * moveSpeed);
        }
        else
        {
            _rigidbody2D.AddForce(-_rigidbody2D.velocity);
        }
    }
}
