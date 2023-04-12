using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BulletTest : MonoBehaviour
{
    public float bulletSpeed;

    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    private void FixedUpdate()
    {
        rb.velocity = transform.up * bulletSpeed;
    }
}
