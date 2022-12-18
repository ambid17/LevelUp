using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    public TMP_Text goldText;
    public Slider hpSlider;
    public Button upgradeButton;
    public Button closeUpgradesButton;
    public GameObject upgradePanel;
    
    void Start()
    {
        GameManager.Instance.currencyDidUpdate.AddListener(SetGoldText);
        GameManager.Instance.hpDidUpdate.AddListener(SetHpSlider);
        upgradeButton.onClick.AddListener(OpenUpgrades);
        closeUpgradesButton.onClick.AddListener(CloseUpgrades);
        CloseUpgrades();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && upgradePanel.activeInHierarchy)
        {
            CloseUpgrades();
        }
    }

    private void SetGoldText(float newValue)
    {
        goldText.text = newValue.ToCurrencyString();
    }

    private void SetHpSlider(float hpPercentage)
    {
        hpSlider.value = hpPercentage;
    }

    private void OpenUpgrades()
    {
        upgradePanel.SetActive(true);
    }

    private void CloseUpgrades()
    {
        upgradePanel.SetActive(false);
    }
}
