using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class UpgradeItem : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text descriptionText;
        public TMP_Text upgradeCountText;
        public Button upgradeButton;
        public TMP_Text upgradeButtonText;

        private Upgrade _upgrade;
    
        private EventService _eventService;
        
        private void Start()
        {
            upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(OnCurrencyUpdated);
        }

        public void Setup(Upgrade upgrade)
        {
            _upgrade = upgrade;
            nameText.text = upgrade.name;
            descriptionText.text = upgrade.description;
            upgradeButton.onClick.AddListener(() => BuyUpgrade());
            OnUpgradeUpdated();
        }
    
        public void BuyUpgrade()
        {
            int purchaseCount = GetAvailablePurchaseCount();
            
            if (GameManager.CurrencyManager.TrySpendCurrency(_upgrade.GetCost(purchaseCount)))
            {
                _upgrade.numberPurchased += purchaseCount;
                _eventService.Dispatch(new UpgradePurchasedEvent(_upgrade));
                OnUpgradeUpdated();
            }
        }
    
        private void OnUpgradeUpdated()
        {
            upgradeCountText.text = _upgrade.GetUpgradeCountText();
            SetInteractability();
            upgradeButtonText.text = _upgrade.GetCost(GetAvailablePurchaseCount()).ToCurrencyString();
        }

        private void OnCurrencyUpdated()
        {
            SetInteractability();
        }

        private void SetInteractability()
        {
            bool hasMoney = GameManager.CurrencyManager.Currency > _upgrade.GetCost(GetAvailablePurchaseCount());
            bool canUpgrade = _upgrade.numberPurchased < _upgrade.maxPurchases || _upgrade.maxPurchases == 0; 
            upgradeButton.interactable = hasMoney && canUpgrade;
        }
        
        private int GetAvailablePurchaseCount()
        {
            int purchaseCount = GameManager.SettingsManager.UpgradePurchaseCount;

            int purchasesToMax = _upgrade.maxPurchases - _upgrade.numberPurchased;

            purchaseCount = Mathf.Min(purchaseCount, purchasesToMax);

            return purchaseCount;
        }
    }
}