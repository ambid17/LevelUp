using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class FightUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldText;
        [SerializeField] private TMP_Text _goldPerMinuteText;
        [SerializeField] private Slider _hpSlider;
        [SerializeField] private Button _upgradeButton;
    
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _pausePanel;
    
        void Start()
        {
            GameManager.GameStateManager.currencyDidUpdate.AddListener(SetGoldText);
            GameManager.GameStateManager.currencyPerMinuteDidUpdate.AddListener(SetGoldPerMinuteText);
            GameManager.GameStateManager.hpDidUpdate.AddListener(SetHpSlider);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            CloseUpgrades();
        
            SetGoldText(GameManager.SettingsManager.progressSettings.Currency);
            SetGoldPerMinuteText(GameManager.SettingsManager.progressSettings.CurrentWorld.CurrencyPerMinute);
            SetHpSlider(1);
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
        }

        private void SetGoldText(float newValue)
        {
            _goldText.text = newValue.ToCurrencyString();
        }
    
        private void SetGoldPerMinuteText(float newValue)
        {
            string value = newValue.ToCurrencyString();
            _goldPerMinuteText.text = $"({value}/min)";
        }

        private void SetHpSlider(float hpPercentage)
        {
            _hpSlider.value = hpPercentage;
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
