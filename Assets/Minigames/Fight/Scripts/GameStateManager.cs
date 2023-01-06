using System;
using UnityEngine;
using UnityEngine.Events;
using Utils;

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
                _eventService.Dispatch<CurrencyUpdatedEvent>();
            }
        }
    
        public float CurrencyPerMinute
        {
            get => _progressSettings.CurrentWorld.CurrencyPerMinute;
            set
            {
                _progressSettings.CurrentWorld.CurrencyPerMinute = value;
                _eventService.Dispatch<CpmUpdatedEvent>();
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
                _eventService.Dispatch(new PlayerHpUpdatedEvent(hpPercent));
            }
        }

        private EventService _eventService;
        private float _deathTime = 5;
        private float _deathTimer;
        private bool _isDead;
        public bool IsDead => _isDead;

    
        private float _gpmTimer; // GPM: gold per minute
        private readonly float _gpmInterval = 5;
        private float _currencyAcquiredThisInterval;


        private void Awake()
        {
            _eventService = GameManager.EventService;
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
            _eventService.Dispatch<EnemyKilledEvent>();
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
            _eventService.Dispatch<PlayerDamagedEvent>();

            if (CurrentPlayerHP <= 0)
            {
                _deathTimer = 0;
                _isDead = true;
                _eventService.Dispatch<PlayerDiedEvent>();
            }
        }
    
        private void WaitForRevive()
        {
            _deathTimer += Time.deltaTime;

            if (_deathTimer > _deathTime)
            {
                _isDead = false;
                CurrentPlayerHP = GameManager.SettingsManager.playerSettings.MaxHp;
                _eventService.Dispatch<PlayerRevivedEvent>();
            }
        }
    }
}
