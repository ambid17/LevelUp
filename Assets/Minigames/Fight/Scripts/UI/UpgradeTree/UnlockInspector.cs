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
    public class UnlockInspector : MonoBehaviour
    {
        [SerializeField] private GameObject container;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Button upgradeButton;
        [SerializeField] private TMP_Text upgradeButtonText;

        private EffectItem _currentEffectItem;
        private UpgradeCategory _upgradeCategory;
        private EffectCategory  _effectCategory;
        private TierCategory    _tierCategory;

        private EventService _eventService;

        private void Awake()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<EffectItemSelectedEvent>(OnEffectSelected);
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
            _eventService.Remove<UnlockItemSelectedEvent>(OnUnlockSelected);
        }

        public void OnUnlockSelected(UnlockItemSelectedEvent e)
        {
            _currentEffectItem = e.EffectItem;

            if (_currentEffectItem == null)
            {
                return;
            }
            _upgradeCategory = _currentEffectItem.effectNode.UpgradeCategory;
            _effectCategory = _currentEffectItem.effectNode.EffectCategory;
            _tierCategory = _currentEffectItem.effectNode.TierCategory;
            container.SetActive(true);

            OnUpgradeUpdated();
        }

        public void OnEffectSelected(EffectItemSelectedEvent e)
        {
            container.SetActive(false);
        }

        public void BuyUpgrade()
        {
            float tierCost = GameManager.SettingsManager.progressSettings.UnlockCostMaps[_tierCategory];

            if (GameManager.CurrencyManager.TrySpendCurrency(tierCost))
            {
                // TODO unlock random effect in correct tree
                OnUpgradeUpdated();
            }
        }

        private void OnUpgradeUpdated()
        {
            nameText.text = $"Unlock";
            float tierCost = GameManager.SettingsManager.progressSettings.UnlockCostMaps[_tierCategory];
            upgradeButtonText.text = tierCost.ToCurrencyString();
            descriptionText.text = $"Unlock a random {_upgradeCategory} {_effectCategory} from {_tierCategory}";

            bool canAfford = GameManager.CurrencyManager.Currency > tierCost;
            upgradeButton.interactable = canAfford; // TODO fail if all upgrades already owned.
        }
    }
}