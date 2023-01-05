using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fish
{
    public class FishUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Slider _weightSlider;
        
        private EventService _eventService;
        
        void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(UpdateGoldText);
            _eventService.Add<FishOnLureUpdatedEvent>(UpdateWeightSlider);
            
            UpdateGoldText();
            UpdateWeightSlider();
        }

        private void UpdateGoldText()
        {
            _goldText.text = GameManager.Currency.ToCurrencyString();
        }

        private void UpdateWeightSlider()
        {
            _weightSlider.value = GameManager.CurrentWeightPercentage;
        }
    }
}

