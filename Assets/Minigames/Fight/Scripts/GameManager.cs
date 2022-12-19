using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    public PlayerMovementController playerMovement;
    public EnemySpawner enemySpawner;
    public PlayerSettings PlayerSettings;
    public UpgradeSettings UpgradeSettings;
    
    public static int PlayerLayer = 6;
    public static int ProjectileLayer = 7;
    public static int GroundLayer = 8;
    public static int EnemyLayer = 9;

    [SerializeField] private float _currency;
    public float Currency => _currency;
    public UnityEvent<float> currencyDidUpdate;

    [SerializeField] private float _currentPlayerHp;

    public float CurrentPlayerHP
    {
        get => _currentPlayerHp;
        set
        {
            _currentPlayerHp = value;
            float hpPercent = _currentPlayerHp / _maxPlayerHp;
            hpDidUpdate.Invoke(hpPercent);
        }
    }
    [SerializeField] private float _maxPlayerHp;
    
    public UnityEvent playerDidDie;
    public UnityEvent playerDidRevive;
    public UnityEvent<float> hpDidUpdate;
    
    private float _deathTime = 5;
    private float _deathTimer = 0;
    private bool _isDead;
    public bool IsDead => _isDead;
    
    void Start()
    {
        CurrentPlayerHP = _maxPlayerHp;
    }

    void Update()
    {
        if (_isDead)
        {
            WaitForRevive();
        }
    }

    public void AddCurrency(float currency)
    {
        _currency += currency;
        currencyDidUpdate.Invoke(_currency);
    }

    public void TrySpendCurrency(float currencyToSpend)
    {
        if (currencyToSpend > _currency)
        {
            return;
        }
        else
        {
            _currency -= currencyToSpend;
        }
    }

    public void TakeDamage(float damage)
    {
        CurrentPlayerHP -= damage;

        if (CurrentPlayerHP <= 0)
        {
            _deathTimer = 0;
            _isDead = true;
            playerDidDie.Invoke();
        }
    }
    
    private void WaitForRevive()
    {
        _deathTimer += Time.deltaTime;

        if (_deathTimer > _deathTime)
        {
            _isDead = false;
            CurrentPlayerHP = _maxPlayerHp;
            playerDidRevive.Invoke();
        }
    }
}
