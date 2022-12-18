using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text upgradeCountText;
    public Button upgradeButton;
    public TMP_Text upgradeButtonText;

    private void Start()
    {
        upgradeButtonText = upgradeButton.GetComponentInChildren<TMP_Text>();
    }

    public void Setup(Upgrade upgrade)
    {
        nameText.text = upgrade.name;
        descriptionText.text = upgrade.description;
        upgradeCountText.text = upgrade.GetUpgradeCountText();
        upgradeButton.interactable = GameManager.Instance.Currency > upgrade.GetCost();
        upgradeButton.onClick.AddListener(() => BuyUpgrade(upgrade));
        upgradeButtonText.text = upgrade.GetCost().ToCurrencyString();
    }

    public void BuyUpgrade(Upgrade upgrade)
    {
        
    }
}
