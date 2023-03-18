using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class EffectInspector : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeButtonText;

        private Upgrade _currentUpgrade;

        private EventService _eventService;

        private void Awake()
        {
            _eventService = GameManager.EventService;
            
            upgradeButton.onClick.AddListener(() => BuyUpgrade());
        }

        private void OnEnable()
        {
            _eventService.Add<CurrencyUpdatedEvent>(OnCurrencyUpdated);
            _eventService.Add<UpgradeSelectedEvent>(OnUpgradeSelected);
            _eventService.Add<PurchaseCountChangedEvent>(OnUpgradeUpdated);
        }

        private void OnDisable()
        {
            _eventService.Remove<CurrencyUpdatedEvent>(OnCurrencyUpdated);
            _eventService.Remove<UpgradeSelectedEvent>(OnUpgradeSelected);
            _eventService.Remove<PurchaseCountChangedEvent>(OnUpgradeUpdated);
        }

        public void ManuallySelectUpgrade(Upgrade upgrade)
        {
            _currentUpgrade = upgrade;
            icon.sprite = upgrade.icon;
            descriptionText.text = upgrade.GetDescription();
            OnUpgradeUpdated();
        }

        public void OnUpgradeSelected(UpgradeSelectedEvent eventType)
        {
            Upgrade upgrade = eventType.Upgrade;
            
            _currentUpgrade = upgrade;
            icon.sprite = upgrade.icon;
            descriptionText.text = upgrade.GetDescription();
            OnUpgradeUpdated();
        }

        public void BuyUpgrade()
        {
            int purchaseCount = GetAvailablePurchaseCount();

            if (GameManager.CurrencyManager.TrySpendCurrency(_currentUpgrade.GetCost(purchaseCount)))
            {
                _currentUpgrade.numberPurchased += purchaseCount;
                _eventService.Dispatch(new UpgradePurchasedEvent(_currentUpgrade));
                OnUpgradeUpdated();
            }
        }

        private void OnUpgradeUpdated()
        {
            SetInteractability();
            nameText.text = $"{_currentUpgrade.name}\n{_currentUpgrade.GetUpgradeCountText()}";
            upgradeButtonText.text = _currentUpgrade.GetCost(GetAvailablePurchaseCount()).ToCurrencyString();
            bonusText.text = _currentUpgrade.GetBonusDescription();
        }

        private void OnCurrencyUpdated()
        {
            SetInteractability();
        }

        private void SetInteractability()
        {
            if (_currentUpgrade == null)
            {
                return;
            }
            bool hasMoney = GameManager.CurrencyManager.Currency > _currentUpgrade.GetCost(GetAvailablePurchaseCount());
            upgradeButton.interactable = hasMoney;
            bool hasPurchasesLeft = _currentUpgrade.numberPurchased < _currentUpgrade.maxPurchases || _currentUpgrade.maxPurchases == 0;
            upgradeButton.gameObject.SetActive(hasPurchasesLeft);
        }

        private int GetAvailablePurchaseCount()
        {
            int purchaseCount = GameManager.SettingsManager.UpgradePurchaseCount;

            int purchasesToMax = 1;
            if (_currentUpgrade.maxPurchases > 0)
            {
                purchasesToMax = _currentUpgrade.maxPurchases - _currentUpgrade.numberPurchased;
                purchaseCount = Mathf.Min(purchaseCount, purchasesToMax);
            }


            return purchaseCount;
        }
    }
}