using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Mining
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Slider _fuelSlider;

        private EventService _eventService;
        void Start()
        {
            SetGoldText();
            SetHpSlider();
            SetFuelSlider();
            _eventService = Services.Instance.EventService;
            _eventService.Add<OnFuelUpdatedEvent>(SetFuelSlider);
            _eventService.Add<OnCurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<OnHealthUpdatedEvent>(SetHpSlider);
        }

        private void SetFuelSlider()
        {
            _fuelSlider.value = GameManager.MiningProgressSettings.FuelAmount / GameManager.MiningProgressSettings.MaxFuel;
        }

        private void SetGoldText()
        {
            _goldText.text = GameManager.Currency.ToCurrencyString();
        }

        private void SetHpSlider()
        {
            _hpSlider.value = GameManager.MiningProgressSettings.HullHealth / GameManager.MiningProgressSettings.MaxHealth;
        }
    }
}
