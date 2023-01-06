using System;
using UnityEngine;
using UnityEngine.Events;

namespace Minigames.Fight
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private NotificationPanel _notificationPanel;

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
    
        public float CurrencyPerMinute
        {
            get => _progressSettings.CurrentWorld.CurrencyPerMinute;
            set
            {
                _progressSettings.CurrentWorld.CurrencyPerMinute = value;
                currencyPerMinuteDidUpdate.Invoke(value);
            }
        }

        private float _currentPlayerHp;
        public float CurrentPlayerHP
        {
            get => _currentPlayerHp;
            set
            {
                float newHp = value;
                // if adding to player hp, clamp it to max
                newHp = Mathf.Clamp(newHp, 0, GameManager.SettingsManager.playerSettings.MaxHp);
                _currentPlayerHp = newHp;
                
                float hpPercent = _currentPlayerHp / GameManager.SettingsManager.playerSettings.MaxHp;
                hpDidUpdate.Invoke(hpPercent);
            }
        }
    
        public UnityEvent<float> currencyDidUpdate;
        public UnityEvent<float> currencyPerMinuteDidUpdate;
        public UnityEvent playerDidDie;
        public UnityEvent playerDidRevive;
        public UnityEvent<float> hpDidUpdate;
        public UnityEvent enemyKilled;
    
        private float _deathTime = 5;
        private float _deathTimer;
        private bool _isDead;
        public bool IsDead => _isDead;

    
        private float _gpmTimer; // GPM: gold per minute
        private readonly float _gpmInterval = 5;
        private float _currencyAcquiredThisInterval;


        private void Awake()
        {
            AwardAwayCurrency();
        }

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

            UpdateGPM();
        }

        private void AwardAwayCurrency()
        {
            if (GameManager.SettingsManager.progressSettings.CurrentWorld.LastTimeVisited == DateTime.MinValue)
            {
                return;
            }
        
            DateTime currentTime = DateTime.Now;
            TimeSpan awayTime = currentTime - GameManager.SettingsManager.progressSettings.CurrentWorld.LastTimeVisited;

            int minutesAway = (int) awayTime.TotalMinutes;
            float award = minutesAway * CurrencyPerMinute;

            Currency += award;
            _notificationPanel.Notify(minutesAway, award);
        }

        private void UpdateGPM()
        {
            _gpmTimer += Time.deltaTime;

            if (_gpmTimer > _gpmInterval)
            {
                _gpmTimer = 0;
                float currencyPerSecond = _currencyAcquiredThisInterval / _gpmInterval;
                CurrencyPerMinute = currencyPerSecond * 60;
                _currencyAcquiredThisInterval = 0;
            }
        }

        public void EnemyKilled(EnemyInstanceSettings enemy)
        {
            float gold = enemy.GoldValue;
            Currency += gold;
            _currencyAcquiredThisInterval += gold;
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
}
