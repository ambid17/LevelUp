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
    public class UpgradeInspector : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeButtonText;

        private Upgrade _currentUpgrade;

        private EventService _eventService;

        private void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(OnCurrencyUpdated);
            _eventService.Add<UpgradeSelectedEvent>(OnUpgradeSelected);
        }

        public void OnUpgradeSelected(UpgradeSelectedEvent eventType)
        {
            Upgrade upgrade = eventType.Upgrade;
            
            _currentUpgrade = upgrade;
            icon.sprite = upgrade.icon;
            nameText.text = $"{upgrade.name}\n{_currentUpgrade.GetUpgradeCountText()}";
            descriptionText.text = upgrade.GetDescription();
            upgradeButton.onClick.AddListener(() => BuyUpgrade());
            OnUpgradeUpdated();
        }

        public void BuyUpgrade()
        {
            if (GameManager.GameStateManager.TrySpendCurrency(_currentUpgrade.GetCost()))
            {
                _currentUpgrade.numberPurchased++;
                _eventService.Dispatch(new UpgradePurchasedEvent(_currentUpgrade));
                OnUpgradeUpdated();
            }
        }

        private void OnUpgradeUpdated()
        {
            SetInteractability();
            upgradeButtonText.text = _currentUpgrade.GetCost().ToCurrencyString();
            bonusText.text = _currentUpgrade.GetBonusDescription();
        }

        private void OnCurrencyUpdated()
        {
            SetInteractability();
        }

        private void SetInteractability()
        {
            bool hasMoney = GameManager.GameStateManager.Currency > _currentUpgrade.GetCost();
            bool hasPurchasesLeft = _currentUpgrade.numberPurchased < _currentUpgrade.maxPurchases || _currentUpgrade.maxPurchases == 0;
            upgradeButton.interactable = hasMoney && hasPurchasesLeft;
        }
    }
}