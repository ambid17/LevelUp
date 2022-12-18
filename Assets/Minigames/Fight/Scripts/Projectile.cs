using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum OwnerType
    {
        Player,
        Enemy
    }
    
    [SerializeField] private float timeToLive = 5;
    [SerializeField] private float moveSpeed = 5;
    private OwnerType _owner;
    public OwnerType Owner => _owner;

    private float _damage;
    public float Damage => _damage;
    
    
    private float timer = 0;
    private Vector2 shootDirection;
    void Start()
    {
        GameManager.Instance.playerDidDie.AddListener(Die);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeToLive)
        {
            Destroy(gameObject);
        }

        Move();
    }

    private void Move()
    {
        Vector2 delta = shootDirection * moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta.x, delta.y, 0);
    }

    #region Setup
    public void SetOwner(OwnerType owner)
    {
        _owner = owner;
    }

    public void SetDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }
    #endregion
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == GameManager.GroundLayer)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
