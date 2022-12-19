using UnityEngine;

public class VehicleBody : MonoBehaviour
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
}
