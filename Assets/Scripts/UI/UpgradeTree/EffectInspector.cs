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
        [SerializeField] private GameObject container;
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text bonusText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeButtonText;

        private EffectItem _currentEffectItem;
        private Effect _currentEffect;

        private EventService _eventService;

        private int _purchaseCount = 1;

        private void Awake()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<EffectItemSelectedEvent>(OnEffectSelected);
            _eventService.Add<PurchaseCountChangedEvent>(OnPurchaseCountChanged);
            _eventService.Add<UnlockItemSelectedEvent>(OnUnlockSelected);
            upgradeButton.onClick.AddListener(BuyUpgrade);
        }

        private void OnEnable()
        {
            // Update the UI in case the currently inspected effect has been changed (i.e.: due to rewardUI)
            OnEffectSelected(new EffectItemSelectedEvent(_currentEffectItem));
        }

        private void OnDestroy()
        {
            _eventService.Remove<EffectItemSelectedEvent>(OnEffectSelected);
            _eventService.Remove<PurchaseCountChangedEvent>(OnPurchaseCountChanged);
            _eventService.Remove<UnlockItemSelectedEvent>(OnUnlockSelected);
        }

        public void OnEffectSelected(EffectItemSelectedEvent e)
        {
            _currentEffectItem = e.EffectItem;

            if (_currentEffectItem == null)
            {
                return;
            }
            _currentEffect = _currentEffectItem.effectNode.Effect;

            container.SetActive(_currentEffect != null);
            if (_currentEffect == null)
            {
                return;
            }

            OnUpgradeUpdated();
        }

        public void OnUnlockSelected(UnlockItemSelectedEvent e)
        {
            container.SetActive(false);
        }

        private void OnPurchaseCountChanged(PurchaseCountChangedEvent e)
        {
            _purchaseCount = e.PurchaseCount;
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
            if (_currentEffect == null)
            {
                return;
            }
            
            icon.sprite = _currentEffect.Icon;
            nameText.text = $"{_currentEffect.Name}\n{_currentEffect.GetUpgradeCountText()}";
            upgradeButtonText.text = _currentEffect.GetCost(GetAvailablePurchaseCount()).ToCurrencyString();
            descriptionText.text = _currentEffect.GetDescription();
            bonusText.text = _currentEffect.GetNextUpgradeDescription(GetAvailablePurchaseCount());
            
            if (!_currentEffect.IsUnlocked)
            {
                upgradeButtonText.text = "LOCKED";
                upgradeButton.interactable = false;
            }
            else
            {
                bool hasPurchasesLeft = _currentEffect.AmountOwned < _currentEffect.MaxAmountOwned ||
                                        _currentEffect.MaxAmountOwned == 0;
                bool canAfford = GameManager.CurrencyManager.Currency > _currentEffect.GetCost(GetAvailablePurchaseCount());
                upgradeButton.interactable = canAfford && hasPurchasesLeft;

                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }
        }

        /// <summary>
        /// Used to figure out how many of an effect you can purchase.
        /// If you want to purchase 100, but the effect maxes out after 5, you should only be able to purchase 5
        /// </summary>
        private int GetAvailablePurchaseCount()
        {
            int purchaseCount = _purchaseCount;

            if (_currentEffect.MaxAmountOwned > 0)
            {
                int purchasesToMax = _currentEffect.MaxAmountOwned - _currentEffect.AmountOwned;
                purchaseCount = Mathf.Min(purchaseCount, purchasesToMax);
            }

            return purchaseCount;
        }
    }
}