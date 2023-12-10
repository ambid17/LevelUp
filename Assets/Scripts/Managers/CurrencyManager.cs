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
        public float Dna
        {
            get => _progressSettings.Dna;
            set
            {
                _progressSettings.Dna = value;
                eventService.Dispatch<CurrencyUpdatedEvent>();
            }
        }

        public float BankedDna
        {
            get => _progressSettings.BankedDna;
            set
            {
                _progressSettings.BankedDna = value;
                eventService.Dispatch<CurrencyUpdatedEvent>();
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
            Dna += gold;
        }

        public bool TrySpendCurrency(float currencyToSpend)
        {
            if (currencyToSpend > BankedDna)
            {
                return false;
            }

            BankedDna -= currencyToSpend;
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
            Dna = 0;
        }

        public bool CanAffordCraft(Upgrade upgrade)
        {
            foreach(var resource in upgrade.resourceCosts)
            {
                if (PhysicalResources[resource.Key] < resource.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool TryCraftUpgrade(Upgrade upgrade)
        {
            if (upgrade.IsCrafted)
            {
                Debug.LogError($"Trying to craft an already crafted upgrade : {upgrade.Name}");
                return false;
            }

            if (CanAffordCraft(upgrade))
            {
                foreach (var resource in upgrade.resourceCosts)
                {
                    PhysicalResources[resource.Key] -= resource.Value;
                }
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
