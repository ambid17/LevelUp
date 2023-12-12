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
        private CombatStats _combatStats => GameManager.PlayerEntity.Stats.combatStats;
        void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _eventService.Add<PlayerAmmoUpdatedEvent>(SetAmmo);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
        
            SetGoldText();
           // SetHpSlider(new PlayerHpUpdatedEvent());
        }

        private void OpenUpgrades()
        {
            _eventService.Dispatch(new PlayerInteractedEvent(InteractionType.Upgrade));
        }
        
        private void SetGoldText()
        {
            _goldText.text = GameManager.CurrencyManager.Dna.ToCurrencyString();
        }

        private void SetHpSlider()
        {
            _hpSlider.value = _combatStats.currentHp / _combatStats.maxHp.Calculated;
        }

        private void SetAmmo(PlayerAmmoUpdatedEvent e)
        {
            _remainingAmmoText.text = $"{e.CurrentAmmo} / {e.MaxAmmo}";
        }
    }
}