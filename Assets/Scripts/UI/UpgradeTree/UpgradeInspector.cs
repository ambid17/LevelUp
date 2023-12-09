using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private Dictionary<int, float> tierCostMapping = new Dictionary<int, float>()
        {
            {1, 100},
            {2, 200},
            {3, 500},
        };

        private void Start()
        {
            _eventService = Platform.EventService;
            upgradeButton.onClick.AddListener(BuyUpgrade);
        }

        private void OnEnable()
        {
            // Start the UI with nothing selected
            OnUpgradeSelected(null);
        }

        public void OnUpgradeTierSelected(UpgradeCategory upgradeCategory, EffectCategory effectCategory, TierCategory tierCategory)
        {
            // Dont show anything if no upgrade selected
            container.SetActive(true);

            icon.gameObject.SetActive(false);
            nameText.text = $"Unlock random {upgradeCategory}-{effectCategory} upgrade from {tierCategory}";

            // convert from the tier (1,2,3...) to the cost
            int tierValue = (int)tierCategory;
            upgradeButtonText.text = tierCostMapping[tierValue].ToCurrencyString();

            var upgradesInCategory = GameManager.SettingsManager.effectSettings.GetUpgradesForCategories(upgradeCategory, effectCategory, tierCategory);
            var unlockedInCategory = upgradesInCategory.Where(u => u.IsUnlocked).ToList();
            descriptionText.text = $"You have unlocked {unlockedInCategory.Count} / {upgradesInCategory.Count} upgrades in this category";
            bonusText.gameObject.SetActive(false);

            if (!_currentUpgrade.IsUnlocked)
            {
                upgradeButtonText.text = "LOCKED";
                upgradeButton.interactable = false;
            }
            else
            {
                bool hasPurchasesLeft = _currentUpgrade.AmountOwned < _currentUpgrade.MaxAmountOwned ||
                                        _currentUpgrade.MaxAmountOwned == 0;
                bool canAfford = GameManager.CurrencyManager.BankedDna > _currentUpgrade.GetCost(1);
                upgradeButton.interactable = canAfford && hasPurchasesLeft;
                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }
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
            // Dont show anything if no upgrade selected
            container.SetActive(_currentUpgrade != null);
            if (_currentUpgrade == null)
            {
                return;
            }

            icon.gameObject.SetActive(_currentUpgrade.Icon != null);
            icon.sprite = _currentUpgrade.Icon;
            nameText.text = $"{_currentUpgrade.Name}\n{_currentUpgrade.GetUpgradeCountText()}";
            upgradeButtonText.text = _currentUpgrade.GetCost(1).ToCurrencyString();
            descriptionText.text = _currentUpgrade.positive.GetDescription();
            bonusText.gameObject.SetActive(true);
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
                bool canAfford = GameManager.CurrencyManager.BankedDna > _currentUpgrade.GetCost(1);
                upgradeButton.interactable = canAfford && hasPurchasesLeft;
                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }
        }

    }
}