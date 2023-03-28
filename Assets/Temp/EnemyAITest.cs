using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITest : MonoBehaviour
{
    [SerializeField]
    private float _Health;
    [SerializeField]
    private float _Speed = 5;
    public float speed => _Speed;
    public float health => _Health;
    public Vector2 enemyPos => transform.position;

    [SerializeField]
    float offset;
    [SerializeField]
    LayerMask layerMask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _Health -= 10;
            Debug.Log(health);
        }
    }
}
