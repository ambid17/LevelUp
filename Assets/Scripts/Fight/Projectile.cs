using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float timeToLive = 5;
    [SerializeField] private float moveSpeed = 5;
    
    
    private float timer = 0;
    private Vector2 shootDirection;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _rigidbody.AddForce(shootDirection * moveSpeed, ForceMode2D.Impulse);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToLive)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (layerMask == (layerMask | (1 << layerMask)))
        {
            Destroy(gameObject);
        }
    }
}
