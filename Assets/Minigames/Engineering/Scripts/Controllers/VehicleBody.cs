using UnityEngine;
using Interfaces;

public class VehicleBody : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;


    [SerializeField]
    private float speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
    public void ApplyDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Destroyed()
    {
        throw new System.NotImplementedException();
    }

}
