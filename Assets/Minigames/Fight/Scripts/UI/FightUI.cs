using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Minigames.Fight
{
    public class FightUI : MonoBehaviour
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
    
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _pausePanel;
        
        private EventService _eventService;

        private bool isUpdatingAbility;
        private float abilityCooldownTimer;
        private float currentAbilityCooldown;
        void Start()
        {
            _eventService = GameManager.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<CpmUpdatedEvent>(SetGoldPerMinuteText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _eventService.Add<PlayerUsedAmmoEvent>(SetAmmo);
            _eventService.Add<PlayerUsedAbilityEvent>(StartUseAbility);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            CloseUpgrades();
        
            SetGoldText();
            SetGoldPerMinuteText();
            SetHpSlider(new PlayerHpUpdatedEvent(1));
            SetupAmmoAndAbility();
        }

        private void SetupAmmoAndAbility()
        {
            Weapon currentWeapon = GameManager.SettingsManager.weaponSettings.equippedWeapon;
            if (currentWeapon is ProjectileWeapon projectileWeapon)
            {
                _ammoContainer.SetActive(true);
                _bulletTypeImage.sprite = projectileWeapon.ammoIcon;
                _remainingAmmoText.text = $"{projectileWeapon.magazineSize} / {projectileWeapon.magazineSize}";
            }
            else
            {
                _ammoContainer.SetActive(false);
            }
            
            _abilityCooldownImage.sprite = GameManager.SettingsManager.weaponSettings.equippedWeapon.abilityIcon;
            _abilityCooldownImageMask.sprite = GameManager.SettingsManager.weaponSettings.equippedWeapon.abilityIcon;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenUpgrades();
            }
        
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_upgradePanel.activeInHierarchy)
                {
                    CloseUpgrades();
                }
                else
                {
                    _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
                }
            }

            if (isUpdatingAbility)
            {
                abilityCooldownTimer += Time.deltaTime;
                _abilityCooldownImageMask.fillAmount = abilityCooldownTimer / currentAbilityCooldown;
                if (abilityCooldownTimer > currentAbilityCooldown)
                {
                    isUpdatingAbility = false;
                }
            }
        }

        private void SetGoldText()
        {
            _goldText.text = GameManager.CurrencyManager.Currency.ToCurrencyString();
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

        private void SetAmmo(PlayerUsedAmmoEvent e)
        {
            _remainingAmmoText.text = $"{e.CurrentAmmo} / {e.MaxAmmo}";
        }

        private void StartUseAbility()
        {
            isUpdatingAbility = true;
            abilityCooldownTimer = 0;
            currentAbilityCooldown = GameManager.SettingsManager.weaponSettings.equippedWeapon
                .abilityCooldown;
        }

        private void OpenUpgrades()
        {
            _upgradePanel.SetActive(true);
        }

        private void CloseUpgrades()
        {
            _upgradePanel.SetActive(false);
        }
    }
}
