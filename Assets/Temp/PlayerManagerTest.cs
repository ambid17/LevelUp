using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerTest : Singleton<PlayerManagerTest>
{
    [SerializeField]
    private GameObject _PlayerPos;

    public GameObject playerPos => _PlayerPos;
}
