using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using Utils;

namespace Minigames.Mining
{
    public class RepairShopUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _fromAmountText;
        [SerializeField] TMP_Text _toAmountText;
        [SerializeField] Button _purchaseButton;
        [SerializeField] Slider _amountSlider;
        [SerializeField] TMP_Text _buttonText;
        [SerializeField] string _textSizePrefix;
        private EventService _eventService;
        // Start is called before the first frame update
        void Start()
        {
            _eventService = Services.Instance.EventService;
            _amountSlider.onValueChanged.AddListener(OnSliderValueChanged);
            _purchaseButton.onClick.AddListener(OnBuyButtonPress);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                gameObject.SetActive(false);
        }

        // Update is called once per frame
        void OnEnable()
        {
            if (GameManager.Instance == null) return;
            _amountSlider.maxValue = GameManager.MiningProgressSettings.MaxHealth;
            _amountSlider.value = GameManager.MiningProgressSettings.HullHealth;
            UpdateShop();
        }

        void SetSaleText()
        {
            float saleValue = GetHealthCost();
            _buttonText.text = "Buy for " + saleValue.ToCurrencyString();
        }

        void OnSliderValueChanged(float newValue)
        {
            if (newValue < GameManager.MiningProgressSettings.HullHealth)
                _amountSlider.value = GameManager.MiningProgressSettings.HullHealth;

            if (newValue <= 0 || GetHealthCost() > GameManager.Currency)
                _purchaseButton.interactable = false;

            else
                _purchaseButton.interactable = true;

            UpdateShop();

        }

        private void SetAmountTexts()
        {
            _fromAmountText.text = GameManager.MiningProgressSettings.HullHealth.ToCurrencyString() + _textSizePrefix + "HP";
            _toAmountText.text = _amountSlider.value.ToCurrencyString() + _textSizePrefix + "HP";
        }

        void OnBuyButtonPress()
        {
            GameManager.MiningProgressSettings.HullHealth = _amountSlider.value;
            _eventService.Dispatch<OnHealthUpdatedEvent>();
            GameManager.Currency -= GetHealthCost();
            UpdateShop();
        }

        float GetHealthCost()
        {
            return (_amountSlider.value - GameManager.MiningProgressSettings.HullHealth) * GameManager.MiningProgressSettings.HealthCost;
        }

        private void UpdateShop()
        {
            SetAmountTexts();
            SetSaleText();
        }
    }
}
