using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

namespace Minigames.Mining
{
    public class FuelShopUI : MonoBehaviour
    {
        [SerializeField] TMP_Text _fromAmountText;
        [SerializeField] TMP_Text _toAmountText;
        [SerializeField] Button _purchaseButton;
        [SerializeField] Slider _amountSlider;
        [SerializeField] TMP_Text _buttonText;
        [SerializeField] string _textSizePrefix;
        // Start is called before the first frame update
        void Start()
        {
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
            _amountSlider.maxValue = GameManager.MiningProgressSettings.MaxFuel;
            _amountSlider.value = GameManager.MiningProgressSettings.FuelAmount;
            UpdateShop();
        }

        void SetSaleText()
        {
            float saleValue = GetFuelCost();
            _buttonText.text = "Buy for " + saleValue.ToCurrencyString();
        }

        void OnSliderValueChanged(float newValue)
        {
            if (newValue < GameManager.MiningProgressSettings.FuelAmount)
                _amountSlider.value = GameManager.MiningProgressSettings.FuelAmount;

            if (newValue <= 0 || GetFuelCost() > GameManager.Currency)
                _purchaseButton.interactable = false;

            else
                _purchaseButton.interactable = true;

            UpdateShop();

        }

        private void SetAmountTexts()
        {
            _fromAmountText.text = GameManager.MiningProgressSettings.FuelAmount.ToCurrencyString() + _textSizePrefix + "L";
            _toAmountText.text = _amountSlider.value.ToCurrencyString() + _textSizePrefix + "L";
        }

        void OnBuyButtonPress()
        {
            GameManager.MiningProgressSettings.FuelAmount = _amountSlider.value;
            GameManager.Currency -= GetFuelCost();
            UpdateShop();
        }

        float GetFuelCost()
        {
            return (_amountSlider.value - GameManager.MiningProgressSettings.FuelAmount) * GameManager.MiningProgressSettings.FuelCost;
        }
        private void UpdateShop()
        {
            SetAmountTexts();
            SetSaleText();
        }
    }
}
