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

    public static UnityEvent<Upgrade> upgradePurchased;
    private void Start()
    {
        upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();
    }

    public void Setup(Upgrade upgrade)
    {
        nameText.text = upgrade.name;
        descriptionText.text = upgrade.description;
        upgradeButton.onClick.AddListener(() => BuyUpgrade(upgrade));
        UpgradeChanged(upgrade);
    }
    
    public void BuyUpgrade(Upgrade upgrade)
    {
        upgrade.numberPurchased++;
        upgradePurchased.Invoke(upgrade);
        UpgradeChanged(upgrade);
    }
    
    private void UpgradeChanged(Upgrade upgrade)
    {
        upgradeCountText.text = upgrade.GetUpgradeCountText();
        upgradeButton.interactable = GameManager.Instance.Currency > upgrade.GetCost();
        upgradeButtonText.text = upgrade.GetCost().ToCurrencyString();
    }
}
