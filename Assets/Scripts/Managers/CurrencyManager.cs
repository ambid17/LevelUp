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
        public ResourceTypeFloatDictionary PhysicalResources
        {
            get => _progressSettings.PhysicalResources;
            set
            {
                _progressSettings.PhysicalResources = value;
            }
        }

        // TODO set up effects for this.
        public float ResourceValue { get => _progressSettings.BaseResourceValue; set => _progressSettings.BaseResourceValue = value; }

        private float _gpmTimer; // GPM: gold per minute
        private readonly float _gpmInterval = 5;

        protected override void Awake()
        { 
            base.Awake();
            
        }

        void Start()
        {
            eventService.Add<PlayerDiedEvent>(ResetCurrency);
        }

        public void EnemyKilled(float baseGoldValue)
        {
            float gold = baseGoldValue;
            Currency += gold;
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
            if (PhysicalResources.ContainsKey(type))
            {
                PhysicalResources[type] += amount;
            }
            else
            {
                PhysicalResources.Add(type, amount);
            }
            eventService.Dispatch(new PlayerResourceUpdateEvent(type, PhysicalResources[type]));
        }

        public void ResetResource(ResourceType type)
        {
            PhysicalResources[type] = 0;
        }

        public void ResetCurrency()
        {
            Currency = 0;
        }
    }
}
