using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public GameObject enemyPrefab;
    
    public static int PlayerLayer = 6;
    public static int ProjectileLayer = 7;
    public static int GroundLayer = 8;
    public static int EnemyLayer = 9;

    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
