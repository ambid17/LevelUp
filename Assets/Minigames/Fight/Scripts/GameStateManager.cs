using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private float _currency;

    public float Currency
    {
        get => _currency;
        set
        {
            _currency = value;
            currencyDidUpdate.Invoke(_currency);
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
            float hpPercent = _currentPlayerHp / GameManager.UpgradeManager.playerSettings.MaxHp;
            hpDidUpdate.Invoke(hpPercent);
        }
    }
    
    public UnityEvent playerDidDie;
    public UnityEvent playerDidRevive;
    public UnityEvent<float> hpDidUpdate;
    
    private float _deathTime = 5;
    private float _deathTimer = 0;
    private bool _isDead;
    public bool IsDead => _isDead;
    
    void Start()
    {
        CurrentPlayerHP = GameManager.UpgradeManager.playerSettings.MaxHp;
    }

    void Update()
    {
        if (_isDead)
        {
            WaitForRevive();
        }
    }

    public void LoadCurrency(float currency)
    {
        Currency = currency;
    }

    public void AddCurrency(float currency)
    {
        Currency += currency;
    }

    public bool TrySpendCurrency(float currencyToSpend)
    {
        if (currencyToSpend > _currency)
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
            CurrentPlayerHP = GameManager.UpgradeManager.playerSettings.MaxHp;
            playerDidRevive.Invoke();
        }
    }
}
