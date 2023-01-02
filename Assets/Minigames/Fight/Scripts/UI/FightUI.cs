using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text goldPerMinuteText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button closeUpgradesButton;
    [SerializeField] private GameObject upgradePanel;
    
    void Start()
    {
        GameManager.GameStateManager.currencyDidUpdate.AddListener(SetGoldText);
        GameManager.GameStateManager.currencyPerMinuteDidUpdate.AddListener(SetGoldPerMinuteText);
        GameManager.GameStateManager.hpDidUpdate.AddListener(SetHpSlider);
        upgradeButton.onClick.AddListener(OpenUpgrades);
        closeUpgradesButton.onClick.AddListener(CloseUpgrades);
        CloseUpgrades();
        
        SetGoldText(GameManager.SettingsManager.progressSettings.CurrentWorld.Currency);
        SetGoldPerMinuteText(GameManager.SettingsManager.progressSettings.CurrentWorld.CurrencyPerMinute);
        SetHpSlider(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenUpgrades();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && upgradePanel.activeInHierarchy)
        {
            CloseUpgrades();
        }
    }

    private void SetGoldText(float newValue)
    {
        goldText.text = newValue.ToCurrencyString();
    }
    
    private void SetGoldPerMinuteText(float newValue)
    {
        string value = newValue.ToCurrencyString();
        goldPerMinuteText.text = $"({value}/min)";
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
