using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerTest : Singleton<PlayerManagerTest>
{
    [SerializeField]
    private GameObject _PlayerObj;

    public GameObject playerObj => _PlayerObj;
    public Vector2 playerPos => transform.position;
}
