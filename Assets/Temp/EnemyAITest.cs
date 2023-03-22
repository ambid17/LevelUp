using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAITest : MonoBehaviour
{
    [SerializeField]
    private float m_Health;
    public float health => m_Health;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Health -= 10;
            Debug.Log(health);
        }
    }
}
