using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Minigames.Fight
{
    public class CurrencyManager : FightBehavior
    {
        [SerializeField] private NotificationPopup notificationPopup;
        private ProgressSettings _progressSettings => GameManager.SettingsManager.progressSettings;
        public float Currency
        {
            get => _progressSettings.Currency;
            set
            {
                _progressSettings.Currency = value;
                eventService.Dispatch<CurrencyUpdatedEvent>();
            }
        }
        public float CurrencyPerMinute
        {
            get => _progressSettings.CurrentWorld.CurrencyPerMinute;
            set
            {
                _progressSettings.CurrentWorld.CurrencyPerMinute = value;
                eventService.Dispatch<CpmUpdatedEvent>();
            }
        }
        public Dictionary<ResourceType, float> PhysicalResources
        {
            get => _progressSettings.PhysicalResources;
            set
            {
                _progressSettings.PhysicalResources = value;
            }
        }
        private float _gpmTimer; // GPM: gold per minute
        private readonly float _gpmInterval = 5;

        protected override void Awake()
        { 
            base.Awake();
            //AwardAwayCurrency();
        }

        void Start()
        {
        }

        void Update()
        {
            //UpdateGPM();
        }

        private void AwardAwayCurrency()
        {
            if (GameManager.SettingsManager.progressSettings.CurrentWorld.LastTimeVisited == DateTime.MinValue)
            {
                return;
            }
        
            DateTime currentTime = DateTime.Now;
            TimeSpan awayTime = currentTime - GameManager.SettingsManager.progressSettings.CurrentWorld.LastTimeVisited;

            // Cap the away time based on upgrades
            int clampedMinutesAway = (int) Mathf.Clamp((float)awayTime.TotalMinutes, 0,
                GameManager.SettingsManager.incomeSettings.IdleTime);
            float currencyPerMinuteScaled =
                CurrencyPerMinute * GameManager.SettingsManager.incomeSettings.IdleGoldRatio;
            float award = clampedMinutesAway * currencyPerMinuteScaled;

            Currency += award;
            eventService.Dispatch(new CurrencyRewardEvent(clampedMinutesAway, award));
            // This can't be an event as this happens in the Awake() of gameStateManager
            notificationPopup.AwardCurrency(clampedMinutesAway, award);
        }

        //private void UpdateGPM()
        //{
        //    _gpmTimer += Time.deltaTime;

        //    if (_gpmTimer > _gpmInterval)
        //    {
        //        _gpmTimer = 0;
        //        float currencyPerSecond = _currencyAcquiredThisInterval / _gpmInterval;
        //        float newCurrencyPerMinute = currencyPerSecond * 60;
        //        newCurrencyPerMinute += GameManager.SettingsManager.incomeSettings.GoldPerMinute;
        //        // If the upgrade is unlocked, only save new GPM if it's higher than before
        //        if (GameManager.SettingsManager.incomeSettings.SaveHighestGold)
        //        {
        //            if (newCurrencyPerMinute > CurrencyPerMinute)
        //            {
        //                CurrencyPerMinute = newCurrencyPerMinute;
        //            }
        //        }
        //        else
        //        {
        //            CurrencyPerMinute = newCurrencyPerMinute;
        //        }
                
        //        _currencyAcquiredThisInterval = 0;
        //    }
        //}

        public void EnemyKilled(float baseGoldValue)
        {
            float gold = baseGoldValue;
            Currency += gold;

            //CompletionType completionType = _progressSettings.AddKill();

            //if (completionType == CompletionType.Country)
            //{
            //    eventService.Dispatch<CountryCompletedEvent>();
            //}
            //else if (completionType == CompletionType.World)
            //{
            //    eventService.Dispatch<WorldCompletedEvent>();
            //}
            
            //eventService.Dispatch<EnemyKilledEvent>();
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

        public void AddResource(ResourceType type, float amount)
        {
            PhysicalResources[type] += amount;
            eventService.Dispatch(new PlayerResourceUpdateEvent(type, amount));
        }
    }
}
