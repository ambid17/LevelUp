using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public PlayerController player;
    public GameObject enemyPrefab;
    
    public static int PlayerLayer = 6;
    public static int ProjectileLayer = 7;
    public static int GroundLayer = 8;
    public static int EnemyLayer = 9;

    [SerializeField] private float _currency;
    public UnityEvent<float> currencyDidUpdate;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddCurrency(float currency)
    {
        _currency += currency;
        currencyDidUpdate.Invoke(_currency);
    }
}
