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

        private Effect _currentEffect;

        private EventService _eventService;

        private void Awake()
        {
            _eventService = GameManager.EventService;

            upgradeButton.onClick.AddListener(() => BuyUpgrade());
        }

        private void OnEnable()
        {
            _eventService.Add<CurrencyUpdatedEvent>(OnCurrencyUpdated);
            _eventService.Add<EffectUpgradeItemSelectedEvent>(OnEffectSelected);
            _eventService.Add<PurchaseCountChangedEvent>(OnUpgradeUpdated);
        }

        private void OnDisable()
        {
            _eventService.Remove<CurrencyUpdatedEvent>(OnCurrencyUpdated);
            _eventService.Remove<EffectUpgradeItemSelectedEvent>(OnEffectSelected);
            _eventService.Remove<PurchaseCountChangedEvent>(OnUpgradeUpdated);
        }

        // public void ManuallySelectUpgrade(Upgrade upgrade)
        // {
        //     _currentEffect = upgrade;
        //     icon.sprite = upgrade.icon;
        //     descriptionText.text = upgrade.GetDescription();
        //     OnUpgradeUpdated();
        // }

        public void OnEffectSelected(EffectUpgradeItemSelectedEvent e)
        {
            EffectUpgradeItem layoutItem = e.LayoutItem;

            _currentEffect = layoutItem.effectNode.Effect;

            if (_currentEffect == null)
            {
                return;
            }

            icon.sprite = _currentEffect.Icon;
            descriptionText.text = _currentEffect.Description;
            OnUpgradeUpdated();
        }

        public void BuyUpgrade()
        {
            int purchaseCount = GetAvailablePurchaseCount();

            if (GameManager.CurrencyManager.TrySpendCurrency(_currentEffect.GetCost(purchaseCount)))
            {
                _currentEffect.AmountOwned += purchaseCount;
                _eventService.Dispatch(new EffectPurchasedEvent(_currentEffect));
                OnUpgradeUpdated();
            }
        }

        private void OnUpgradeUpdated()
        {
            SetInteractability();
            nameText.text = $"{_currentEffect.Name}\n{_currentEffect.GetUpgradeCountText()}";
            upgradeButtonText.text = _currentEffect.GetCost(GetAvailablePurchaseCount()).ToCurrencyString();
            bonusText.text = _currentEffect.NextUpgradeDescription;
        }

        private void OnCurrencyUpdated()
        {
            SetInteractability();
        }

        private void SetInteractability()
        {
            if (_currentEffect == null)
            {
                return;
            }

            bool hasMoney = GameManager.CurrencyManager.Currency > _currentEffect.GetCost(GetAvailablePurchaseCount());
            upgradeButton.interactable = hasMoney;
            bool hasPurchasesLeft = _currentEffect.AmountOwned < _currentEffect.MaxAmountOwned ||
                                    _currentEffect.MaxAmountOwned == 0;
            upgradeButton.gameObject.SetActive(hasPurchasesLeft);
        }

        /// <summary>
        /// Used to figure out how many of an effect you can purchase.
        /// If you want to purchase 100, but the effect maxes out after 5, you should only be able to purchase 5
        /// </summary>
        private int GetAvailablePurchaseCount()
        {
            int purchaseCount = GameManager.SettingsManager.UpgradePurchaseCount;

            if (_currentEffect.MaxAmountOwned > 0)
            {
                int purchasesToMax = _currentEffect.MaxAmountOwned - _currentEffect.AmountOwned;
                purchaseCount = Mathf.Min(purchaseCount, purchasesToMax);
            }

            return purchaseCount;
        }
    }
}