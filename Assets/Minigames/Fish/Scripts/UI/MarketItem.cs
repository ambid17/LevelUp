using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fish
{
    public class MarketItem : MonoBehaviour
    {
        [SerializeField] Image _image;
        [SerializeField] TMP_Text _fishName;
        [SerializeField] TMP_Text _fishCount;
        [SerializeField] TMP_Text _sellText;
        [SerializeField] Button _sellButton;
        [SerializeField] Slider _sellSlider;

        private Fish _fish;
        private EventService _eventService;
        
        void OnEnable()
        {
            UpdateItem();
        }
        
        private void Start()
        {
            _sellSlider.onValueChanged.AddListener(OnSliderValueChanged);
            _sellButton.onClick.AddListener(OnSellButtonPress);
        }

        public void Setup(Fish fish)
        {
            _fish = fish;
            _image.sprite = fish.Sprite;
            UpdateItem();
        }
        
        void SetSaleText()
        {
            _sellText.text = $"Sell for {GetSaleValue().ToCurrencyString()}";
        }

        private void OnSellButtonPress()
        {
            _fish.Count -= (int)_sellSlider.value;
            GameManager.Currency += _sellSlider.value * _fish.GoldValue;
            UpdateItem();
        }


        void OnSliderValueChanged(float newValue)
        {
            SetSellButtonInteractable();
            SetSaleText();
        }

        float GetSaleValue()
        {
            return _sellSlider.value * _fish.GoldValue;
        }

        void SetSellButtonInteractable()
        {
            if (_sellSlider.value == 0 || GetSaleValue() > GameManager.Currency) _sellButton.interactable = false;
            else _sellButton.interactable = true;
        }

        void UpdateItem()
        {
            if (GameManager.Instance == null || _fish == null) return;
            SetSaleText();
            SetSellButtonInteractable();
            _sellSlider.maxValue = _fish.Count;
            _sellSlider.value = 0;
            _fishName.text = _fish.Name;
            _fishCount.text = _fish.Count.ToCurrencyString();
        }
    }
}