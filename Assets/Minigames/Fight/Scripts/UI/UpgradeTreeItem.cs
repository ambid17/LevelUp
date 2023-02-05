using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class UpgradeTreeItem : MonoBehaviour
    {
        public Image image;
        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text bonusText;
        public Button upgradeButton;
        public TMP_Text upgradeButtonText;

        private Upgrade _upgrade;
    
        private EventService _eventService;
        
        private void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(OnCurrencyUpdated);
        }

        public void Setup(Upgrade upgrade)
        {
            _upgrade = upgrade;
            nameText.text = $"{upgrade.name}\n{_upgrade.GetUpgradeCountText()}";
            descriptionText.text = upgrade.description;
            upgradeButton.onClick.AddListener(() => BuyUpgrade());
            OnUpgradeUpdated();
        }
    
        public void BuyUpgrade()
        {
            if (GameManager.GameStateManager.TrySpendCurrency(_upgrade.GetCost()))
            {
                _upgrade.numberPurchased++;
                _eventService.Dispatch(new UpgradePurchasedEvent(_upgrade));
                OnUpgradeUpdated();
            }
        }
    
        private void OnUpgradeUpdated()
        {
            SetInteractability();
            upgradeButtonText.text = _upgrade.GetCost().ToCurrencyString();
        }

        private void OnCurrencyUpdated()
        {
            SetInteractability();
        }

        private void SetInteractability()
        {
            bool hasMoney = GameManager.GameStateManager.Currency > _upgrade.GetCost();
            bool canUpgrade = _upgrade.numberPurchased < _upgrade.maxPurchases || _upgrade.maxPurchases == 0; 
            upgradeButton.interactable = hasMoney && canUpgrade;
        }
    }
}
