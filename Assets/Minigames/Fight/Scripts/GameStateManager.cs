using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    private ProgressSettings _progressSettings => GameManager.SettingsManager.progressSettings;
    public float Currency
    {
        get => _progressSettings.Currency;
        set
        {
            _progressSettings.Currency = value;
            currencyDidUpdate.Invoke(value);
        }
    }
    public UnityEvent<float> currencyDidUpdate;

    
    [SerializeField] private float _currentPlayerHp;
    public float CurrentPlayerHP
    {
        get => _currentPlayerHp;
        set
        {
            _currentPlayerHp = value;
            float hpPercent = _currentPlayerHp / GameManager.SettingsManager.playerSettings.MaxHp;
            hpDidUpdate.Invoke(hpPercent);
        }
    }
    
    public UnityEvent playerDidDie;
    public UnityEvent playerDidRevive;
    public UnityEvent<float> hpDidUpdate;
    
    private float _deathTime = 5;
    private float _deathTimer;
    private bool _isDead;
    public bool IsDead => _isDead;

    public UnityEvent enemyKilled;
    
    void Start()
    {
        CurrentPlayerHP = GameManager.SettingsManager.playerSettings.MaxHp;
    }

    void Update()
    {
        if (_isDead)
        {
            WaitForRevive();
        }
    }

    public void EnemyKilled(EnemyInstanceSettings enemy)
    {
        float gold = enemy.goldValue;
        Currency += gold;
        _progressSettings.AddKill();
        enemyKilled.Invoke();
    }

    public bool TrySpendCurrency(float currencyToSpend)
    {
        if (currencyToSpend > Currency)
        {
            return false;
        }
        
        Currency -= currencyToSpend;
        return true;
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
            CurrentPlayerHP = GameManager.SettingsManager.playerSettings.MaxHp;
            playerDidRevive.Invoke();
        }
    }
}
