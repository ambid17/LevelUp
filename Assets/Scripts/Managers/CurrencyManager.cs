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
        private ProgressSettings _progressSettings => GameManager.ProgressSettings;
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


        public static readonly float BASE_COST = 10;
        public static readonly float COST_SCALAR = 1.5f;

        public static readonly Dictionary<int, float> TierCostMapping = new Dictionary<int, float>()
        {
            {1, 100},
            {2, 200},
            {3, 500},
        };

        public static readonly SerializableDictionary<ResourceType, float> baseResourceCosts = new SerializableDictionary<ResourceType, float>()
        {
            { ResourceType.Dirt, 1 },
            { ResourceType.Grass, 2 }
        };


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

        public void ResetCurrency()
        {
            Dna = 0;
        }

        public bool CanAffordCraft(Upgrade upgrade)
        {
            foreach(var resource in GetUpgradeResourceCosts(upgrade))
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
                foreach (var resource in GetUpgradeResourceCosts(upgrade))
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

        // example:
        // base cost = 100, scalar = 1.5;
        // 100, 150, 225
        public static float GetUpgradeCost(Upgrade upgrade)
        {
            return BASE_COST * Mathf.Pow(COST_SCALAR, upgrade.AmountOwned + 1);
        }

        public static SerializableDictionary<ResourceType, float> GetUpgradeResourceCosts(Upgrade upgrade)
        {
            var resourceCosts = new SerializableDictionary<ResourceType, float>();

            foreach (var cost in baseResourceCosts)
            {
                ResourceType resourceType = cost.Key;
                float resourceCost = cost.Value;

                float finalCost = resourceCost * Mathf.Pow(COST_SCALAR, upgrade.AmountOwned + 1);
                finalCost = Mathf.Floor(finalCost);
                resourceCosts.Add(cost.Key, finalCost);
            }

            return resourceCosts;
        }
    }
}
