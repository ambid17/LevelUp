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
        [SerializeField] private TMP_Text _goldPerMinuteText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private GameObject _ammoContainer;
        [SerializeField] private Image _bulletTypeImage;
        [SerializeField] private TMP_Text _remainingAmmoText;


        [SerializeField] private Image _abilityCooldownImage;
        [SerializeField] private Image _abilityCooldownImageMask;

        private bool isUpdatingAbility;

        private EventService _eventService;

        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<CpmUpdatedEvent>(SetGoldPerMinuteText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _eventService.Add<PlayerAmmoUpdatedEvent>(SetAmmo);
            _eventService.Add<PlayerUsedAbilityEvent>(StartUseAbility);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
        
            SetGoldText();
            SetGoldPerMinuteText();
            SetHpSlider(new PlayerHpUpdatedEvent(1));
            SetupAmmoAndAbility();
        }

        private void Update()
        {
            if (isUpdatingAbility)
            {
                //_abilityCooldownImageMask.fillAmount = GameManager.PlayerEntity.WeaponController.AbilityTimer / GameManager.PlayerEntity.WeaponController.Weapon.abilityCooldown;
                if (_abilityCooldownImageMask.fillAmount >= 1)
                {
                    isUpdatingAbility = false;
                }
            }
        }

        private void OpenUpgrades()
        {
            GameManager.UIManager.ToggleUiPanel(UIPanelType.EffectUpgrade, true);
        }

        private void SetupAmmoAndAbility()
        {
            // TODO rework this with current weapon system and set up an event for weapon switch
            _bulletTypeImage.sprite = GameManager.PlayerEntity.WeaponArmController.CurrentArm.EquippedWeapon.Weapon.ammoIcon;
        }
        
        private void SetGoldText()
        {
            float goldEarned = GameManager.CurrencyManager.Currency;
            float multipliedGold = Mathf.Pow(goldEarned, 1.1f);
            float difference = multipliedGold - goldEarned;
            _goldText.text = goldEarned.ToCurrencyString() + " + " + difference.ToCurrencyString();
        }
    
        private void SetGoldPerMinuteText()
        {
            string value = GameManager.CurrencyManager.CurrencyPerMinute.ToCurrencyString();
            _goldPerMinuteText.text = $"({value}/min)";
        }

        private void SetHpSlider(PlayerHpUpdatedEvent eventType)
        {
            _hpSlider.value = eventType.PercentHp;
        }

        private void SetAmmo(PlayerAmmoUpdatedEvent e)
        {
            _remainingAmmoText.text = $"{e.CurrentAmmo} / {e.MaxAmmo}";
        }

        private void StartUseAbility()
        {
            isUpdatingAbility = true;
        }
    }
}