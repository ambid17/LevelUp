using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public enum UpgradeUiState
    {
        Upgrade,
        Craft
    }

    public class UpgradeUI : UIPanel
    {
        [SerializeField]
        private UpgradeCategorySelector upgradeCategorySelector;
        [SerializeField]
        private EffectCategorySelector effectCategorySelector;
        [SerializeField]
        private TierCategorySelector tierCategorySelector;
        [SerializeField]
        private UpgradeSelector upgradeSelector;
        [SerializeField]
        private UpgradeInspector upgradeInspector;

        [SerializeField]
        private Button backButton;

        [SerializeField]
        private Button closeButton;

        [Header("State")]
        public UpgradeCategory upgradeCategory = UpgradeCategory.None;
        public EffectCategory effectCategory = EffectCategory.None;
        public TierCategory tierCategory = TierCategory.None;
        public UpgradeUiState upgradeUiState = UpgradeUiState.Upgrade;

        void Start()
        {
            backButton.onClick.AddListener(Back);
            closeButton.onClick.AddListener(Close);
            Platform.EventService.Add<PlayerInteractedEvent>(OnPlayerInteract);
        }

        private void OnPlayerInteract(PlayerInteractedEvent e)
        {
            if(e.InteractionType == InteractionType.Craft)
            {
                upgradeUiState = UpgradeUiState.Craft;
                GameManager.UIManager.ToggleUiPanel(UIPanelType.Effect, true);
            }

            if(e.InteractionType == InteractionType.Upgrade)
            {
                upgradeUiState = UpgradeUiState.Upgrade;
                GameManager.UIManager.ToggleUiPanel(UIPanelType.Effect, true);
            }
        }

        private void Back()
        {
            // clear the inspector
            upgradeInspector.OnUpgradeSelected(null);

            if (tierCategory != TierCategory.None)
            {
                tierCategory = TierCategory.None;
            }
            else if (effectCategory != EffectCategory.None)
            {
                effectCategory = EffectCategory.None;
            }
            else if (upgradeCategory != UpgradeCategory.None)
            {
                upgradeCategory = UpgradeCategory.None;
            }

            SetUI();
        }

        private void Close()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Effect, false);
        }

        public override void Toggle(bool shouldBeActive)
        {
            base.Toggle(shouldBeActive);
            ResetUI();
        }

        private void ResetUI()
        {
            upgradeCategory = UpgradeCategory.None;
            effectCategory = EffectCategory.None;
            tierCategory = TierCategory.None;

            SetUI();
        }

        private void SetUI()
        {
            upgradeCategorySelector.gameObject.SetActive(false);
            effectCategorySelector.gameObject.SetActive(false);
            tierCategorySelector.gameObject.SetActive(false);
            upgradeSelector.gameObject.SetActive(false);

            if (upgradeCategory == UpgradeCategory.None)
            {
                upgradeCategorySelector.gameObject.SetActive(true);
            }
            else if (effectCategory == EffectCategory.None)
            {
                effectCategorySelector.gameObject.SetActive(true);
            }
            else if (tierCategory == TierCategory.None)
            {
                tierCategorySelector.gameObject.SetActive(true);
            }
            else
            {
                upgradeSelector.gameObject.SetActive(true);
            }
        }

        public void OnUpgradeCategorySelected(UpgradeCategory upgradeCategory)
        {
            this.upgradeCategory = upgradeCategory;
            SetUI();
        }

        public void OnEffectCategorySelected(EffectCategory effectCategory)
        {
            this.effectCategory = effectCategory;
            SetUI();
        }

        public void OnTierCategorySelected(TierCategory tierCategory)
        {
            this.tierCategory = tierCategory;
            upgradeInspector.OnUpgradeTierSelected(upgradeCategory, effectCategory, tierCategory);
            SetUI();
        }

        public void OnUpgradeSelected(Upgrade upgrade)
        {
            upgradeInspector.OnUpgradeSelected(upgrade);
        }
    }
}