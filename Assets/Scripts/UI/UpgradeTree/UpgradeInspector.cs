using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
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
        [SerializeField] private TMP_Text resourcesText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeButtonText;
        [SerializeField] private RectTransform upgradeButtonRect;
        [SerializeField] private UpgradeUI upgradeUI;

        private Upgrade _currentUpgrade;
        private EventService _eventService;

        private void Start()
        {
            _eventService = Platform.EventService;
        }

        private void OnEnable()
        {
            // Start the UI with nothing selected, this will hide the container
            OnUpgradeSelected(null);
        }

        public void OnUpgradeSelected(Upgrade upgrade)
        {
            _currentUpgrade = upgrade;

            if(upgradeUI.upgradeUiState == UpgradeUiState.Upgrade)
            {
                OnUpgradeSelectedForUpgrade();
            }else if(upgradeUI.upgradeUiState == UpgradeUiState.Craft)
            {
                OnUpgradeSelectedForCraft();
            }
        }

        public void OnUpgradeTierSelected()
        {
            // Dont show anything if no upgrade selected
            container.SetActive(true);

            icon.gameObject.SetActive(false);
            nameText.text = $"Unlock random {upgradeUI.upgradeCategory}-{upgradeUI.effectCategory} upgrade from {upgradeUI.tierCategory}";

            // convert from the tier (1,2,3...) to the cost
            int tierValue = (int)upgradeUI.tierCategory;
            float tierCost = CurrencyManager.TierCostMapping[tierValue];
            upgradeButtonText.text = tierCost.ToCurrencyString();

            var upgradesInCategory = GameManager.UpgradeSettings.GetAllUpgradesInCategory(upgradeUI.upgradeCategory, upgradeUI.effectCategory, upgradeUI.tierCategory);
            var unlockedInCategory = upgradesInCategory.Where(u => u.IsUnlocked).ToList();
            descriptionText.text = $"You have unlocked {unlockedInCategory.Count} / {upgradesInCategory.Count} upgrades in this category";

            bonusText.text = string.Empty;
            resourcesText.gameObject.SetActive(false);


            bool hasPurchasesLeft = unlockedInCategory.Count < upgradesInCategory.Count;
            bool canAfford = GameManager.CurrencyManager.BankedDna > tierCost;
            upgradeButton.interactable = canAfford && hasPurchasesLeft;
            if (!hasPurchasesLeft)
            {
                upgradeButtonText.text = "MAXED";
            }

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(() => UnlockRandomUpgrade());

            StartCoroutine(RebuildUpgradeButton());
        }

        public void UnlockRandomUpgrade()
        {
            int tierValue = (int)upgradeUI.tierCategory;
            float tierCost = CurrencyManager.TierCostMapping[tierValue];
            if (GameManager.CurrencyManager.TrySpendCurrency(tierCost))
            {
                var upgrade = GameManager.UpgradeSettings.GetUpgradeToUnlock(upgradeUI.upgradeCategory, upgradeUI.effectCategory, upgradeUI.tierCategory);
                upgrade.IsUnlocked = true;

                // Update the UI with the new state
                OnUpgradeTierSelected();
            }
        }

        private void OnUpgradeSelectedForUpgrade()
        {
            // Dont show anything if no upgrade selected
            container.SetActive(_currentUpgrade != null);
            if (_currentUpgrade == null)
            {
                return;
            }

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(BuyUpgrade);

            icon.gameObject.SetActive(_currentUpgrade.Icon != null);
            icon.sprite = _currentUpgrade.Icon;
            nameText.text = $"{_currentUpgrade.Name}\n{_currentUpgrade.GetUpgradeCountText()}";
            upgradeButtonText.text = CurrencyManager.GetUpgradeCost(_currentUpgrade).ToCurrencyString();
            descriptionText.text = _currentUpgrade.positive.effect.GetDescription();
            bonusText.text = _currentUpgrade.negative.effect.GetDescription();

            resourcesText.gameObject.SetActive(false);

            if (!_currentUpgrade.IsUnlocked)
            {
                upgradeButtonText.text = "LOCKED";
                upgradeButton.interactable = false;
            }
            else
            {
                bool hasPurchasesLeft = _currentUpgrade.AmountOwned < _currentUpgrade.MaxAmountOwned ||
                                        _currentUpgrade.MaxAmountOwned == 0;
                bool canAfford = GameManager.CurrencyManager.BankedDna > CurrencyManager.GetUpgradeCost(_currentUpgrade);
                upgradeButton.interactable = canAfford && hasPurchasesLeft;
                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }

            StartCoroutine(RebuildUpgradeButton());
        }

        public void BuyUpgrade()
        {
            if (GameManager.CurrencyManager.TrySpendCurrency(CurrencyManager.GetUpgradeCost(_currentUpgrade)))
            {
                _currentUpgrade.BuyUpgrade();
                OnUpgradeSelectedForUpgrade();
            }
        }

        private void OnUpgradeSelectedForCraft()
        {
            // Dont show anything if no upgrade selected
            container.SetActive(_currentUpgrade != null);
            if (_currentUpgrade == null)
            {
                return;
            }

            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(CraftUpgrade);

            icon.gameObject.SetActive(_currentUpgrade.Icon != null);
            icon.sprite = _currentUpgrade.Icon;
            nameText.text = $"{_currentUpgrade.Name}\n{_currentUpgrade.GetUpgradeCountText()}";
            upgradeButtonText.text = "CRAFT";
            descriptionText.text = _currentUpgrade.positive.effect.GetDescription();
            bonusText.text = _currentUpgrade.negative.effect.GetDescription();

            resourcesText.gameObject.SetActive(true);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("Resource costs:");
            foreach(var cost in CurrencyManager.GetUpgradeResourceCosts(_currentUpgrade))
            {
                stringBuilder.AppendLine($"{cost.Key} : {cost.Value}");
            }
            resourcesText.text = stringBuilder.ToString();

            
            bool canCraft = !_currentUpgrade.IsCrafted;
            bool canAfford = GameManager.CurrencyManager.CanAffordCraft(_currentUpgrade);
            upgradeButton.interactable = canAfford && canCraft;

            StartCoroutine(RebuildUpgradeButton());
        }

        public void CraftUpgrade()
        {
            if (GameManager.CurrencyManager.TryCraftUpgrade(_currentUpgrade))
            {
                _currentUpgrade.Craft();
                OnUpgradeSelectedForCraft();
                Platform.EventService.Dispatch<DidCraftUpgradeEvent>();
            }
        }

        // Forces the horizontal layout groups to regenerate, fixing any overlaps when the text changes
        private IEnumerator RebuildUpgradeButton()
        {
            yield return new WaitForEndOfFrame();
            LayoutRebuilder.ForceRebuildLayoutImmediate(upgradeButtonRect);
        }
    }
}