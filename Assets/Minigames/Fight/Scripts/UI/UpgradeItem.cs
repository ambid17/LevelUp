using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text upgradeCountText;
    public Button upgradeButton;
    public TMP_Text upgradeButtonText;

    private Upgrade _upgrade;
    public static event Action<Upgrade> upgradePurchased;
    
    private void Start()
    {
        upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();
        GameManager.GameStateManager.currencyDidUpdate.AddListener(OnCurrencyUpdated);
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
        if (GameManager.GameStateManager.TrySpendCurrency(_upgrade.GetCost()))
        {
            _upgrade.numberPurchased++;
            upgradePurchased?.Invoke(_upgrade);
            OnUpgradeUpdated();
        }
    }
    
    private void OnUpgradeUpdated()
    {
        upgradeCountText.text = _upgrade.GetUpgradeCountText();
        upgradeButton.interactable = GameManager.GameStateManager.Currency > _upgrade.GetCost();
        upgradeButtonText.text = _upgrade.GetCost().ToCurrencyString();
    }

    private void OnCurrencyUpdated(float newValue)
    {
        upgradeButton.interactable = newValue > _upgrade.GetCost();
    }
}
