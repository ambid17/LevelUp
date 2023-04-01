using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerTest : Singleton<PlayerManagerTest>
{
    [SerializeField]
    private Transform _PlayerTransform;

    public Transform playerTransform => Instance._PlayerTransform;
    public Vector2 playerPos => Instance._PlayerTransform.position;
}
