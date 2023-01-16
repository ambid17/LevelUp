using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fish
{
    public class FishUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Slider _weightSlider;
        [SerializeField] private Slider _depthSlider;
        
        private EventService _eventService;
        
        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(UpdateGoldText);
            _eventService.Add<FishOnLureUpdatedEvent>(UpdateWeightSlider);
            
            UpdateGoldText();
            UpdateWeightSlider();
        }

        private void Update()
        {
            UpdateDepthSlider();
        }

        private void UpdateGoldText()
        {
            _goldText.text = GameManager.Currency.ToCurrencyString();
        }

        private void UpdateWeightSlider()
        {
            _weightSlider.value = GameManager.CurrentWeightPercentage;
        }

        private void UpdateDepthSlider()
        {
            _depthSlider.value = GameManager.CurrentDepthPercentage;
        }
    }
}

