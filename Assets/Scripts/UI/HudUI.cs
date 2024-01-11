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
        [SerializeField] private Button _craftButton;
        [SerializeField] private TMP_Text _remainingAmmoText;
        [SerializeField] private Image _ammoImage;

        private EventService _eventService;
        private CombatStats _combatStats => GameManager.PlayerEntity.Stats.combatStats;
        void Start()
        {
            _eventService = Platform.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _eventService.Add<PlayerAmmoUpdatedEvent>(UpdateAmmo);
            _eventService.Add<PlayerChangedWeaponEvent>(SetWeaponUI);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            _craftButton.onClick.AddListener(OpenCrafting);

            SetupDevButtons();
            SetGoldText();
           // SetHpSlider(new PlayerHpUpdatedEvent());
        }

        private void SetupDevButtons()
        {
            _upgradeButton.gameObject.SetActive(false);
            _craftButton.gameObject.SetActive(false);

            #if UNITY_EDITOR
                _upgradeButton.gameObject.SetActive(true);
                _craftButton.gameObject.SetActive(true);
            #endif
        }

        private void OpenUpgrades()
        {
            _eventService.Dispatch(new PlayerInteractedEvent(InteractionType.Upgrade));
        }

        private void OpenCrafting()
        {
            _eventService.Dispatch(new PlayerInteractedEvent(InteractionType.Craft));
        }

        private void SetGoldText()
        {
            _goldText.text = GameManager.CurrencyManager.Dna.ToCurrencyString();
        }

        private void SetHpSlider()
        {
            _hpSlider.value = _combatStats.currentHp / _combatStats.maxHp.Calculated;
        }

        private void SetWeaponUI(PlayerChangedWeaponEvent e)
        {
            if (e.NewWeaponMode == WeaponMode.Melee)
            {
                _ammoImage.sprite = _combatStats.meleeWeaponStats.sprite;
            }
            else if (e.NewWeaponMode == WeaponMode.Projectile)
            {
                _ammoImage.sprite = _combatStats.projectileWeaponStats.sprite;
            }
        }

        private void UpdateAmmo(PlayerAmmoUpdatedEvent e)
        {
            _remainingAmmoText.text = $"{e.CurrentAmmo} / {e.MaxAmmo}";
        }
    }
}