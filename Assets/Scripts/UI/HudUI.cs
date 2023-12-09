using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class HudUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private TMP_Text _remainingAmmoText;

        private EventService _eventService;

        void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _eventService.Add<PlayerAmmoUpdatedEvent>(SetAmmo);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
        
            SetGoldText();
            SetHpSlider(new PlayerHpUpdatedEvent(1));
        }

        private void OpenUpgrades()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.Effect, true);
        }
        
        private void SetGoldText()
        {
            _goldText.text = GameManager.CurrencyManager.Dna.ToCurrencyString();
        }

        private void SetHpSlider(PlayerHpUpdatedEvent eventType)
        {
            _hpSlider.value = eventType.PercentHp;
        }

        private void SetAmmo(PlayerAmmoUpdatedEvent e)
        {
            _remainingAmmoText.text = $"{e.CurrentAmmo} / {e.MaxAmmo}";
        }
    }
}