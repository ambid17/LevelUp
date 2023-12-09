using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class UpgradeInspector : MonoBehaviour
    {
        [SerializeField] private GameObject container;
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
            _eventService = Platform.EventService;
            upgradeButton.onClick.AddListener(BuyUpgrade);
        }

        public void OnUpgradeSelected(Upgrade upgrade)
        {
            _currentUpgrade = upgrade;
            OnUpgradeUpdated();
        }

        public void BuyUpgrade()
        {
            if (GameManager.CurrencyManager.TrySpendCurrency(_currentUpgrade.GetCost(1)))
            {
                _currentUpgrade.AmountOwned += 1;
                _eventService.Dispatch(new UpgradePurchasedEvent(_currentUpgrade));
                OnUpgradeUpdated();
            }
        }

        private void OnUpgradeUpdated()
        {
            if (_currentUpgrade == null)
            {
                return;
            }

            icon.sprite = _currentUpgrade.Icon;
            nameText.text = $"{_currentUpgrade.Name}\n{_currentUpgrade.GetUpgradeCountText()}";
            upgradeButtonText.text = _currentUpgrade.GetCost(1).ToCurrencyString();
            descriptionText.text = _currentUpgrade.positive.GetDescription();
            bonusText.text = _currentUpgrade.negative.GetDescription();

            if (!_currentUpgrade.IsUnlocked)
            {
                upgradeButtonText.text = "LOCKED";
                upgradeButton.interactable = false;
            }
            else
            {
                bool hasPurchasesLeft = _currentUpgrade.AmountOwned < _currentUpgrade.MaxAmountOwned ||
                                        _currentUpgrade.MaxAmountOwned == 0;
                bool canAfford = GameManager.CurrencyManager.Currency > _currentUpgrade.GetCost(1);
                upgradeButton.interactable = canAfford && hasPurchasesLeft;
                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }
        }

    }
}