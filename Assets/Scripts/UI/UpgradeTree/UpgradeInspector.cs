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

        private Effect _currentEffect;
        private EventService _eventService;

        private void Start()
        {
            _eventService = Platform.EventService;
            upgradeButton.onClick.AddListener(BuyUpgrade);
        }

        public void OnEffectSelected(Effect effect)
        {
            _currentEffect = effect;
            OnUpgradeUpdated();
        }

        public void BuyUpgrade()
        {
            if (GameManager.CurrencyManager.TrySpendCurrency(_currentEffect.GetCost(1)))
            {
                _currentEffect.AmountOwned += 1;
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
            upgradeButtonText.text = _currentEffect.GetCost(1).ToCurrencyString();
            descriptionText.text = _currentEffect.GetDescription();
            bonusText.text = _currentEffect.GetNextUpgradeDescription(1);

            if (!_currentEffect.IsUnlocked)
            {
                upgradeButtonText.text = "LOCKED";
                upgradeButton.interactable = false;
            }
            else
            {
                bool hasPurchasesLeft = _currentEffect.AmountOwned < _currentEffect.MaxAmountOwned ||
                                        _currentEffect.MaxAmountOwned == 0;
                bool canAfford = GameManager.CurrencyManager.Currency > _currentEffect.GetCost(1);
                upgradeButton.interactable = canAfford && hasPurchasesLeft;
                if (!hasPurchasesLeft)
                {
                    upgradeButtonText.text = "MAXED";
                }
            }
        }

    }
}