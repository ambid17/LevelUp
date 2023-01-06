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
    
        [SerializeField] private GameObject _upgradePanel;
        [SerializeField] private GameObject _pausePanel;
        
        private EventService _eventService;
    
        void Start()
        {
            _eventService = Services.Instance.EventService;
            _eventService.Add<CurrencyUpdatedEvent>(SetGoldText);
            _eventService.Add<CpmUpdatedEvent>(SetGoldPerMinuteText);
            _eventService.Add<PlayerHpUpdatedEvent>(SetHpSlider);
            _upgradeButton.onClick.AddListener(OpenUpgrades);
            CloseUpgrades();
        
            SetGoldText();
            SetGoldPerMinuteText();
            SetHpSlider(new PlayerHpUpdatedEvent(1));
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

        private void SetGoldText()
        {
            _goldText.text = GameManager.GameStateManager.Currency.ToCurrencyString();
        }
    
        private void SetGoldPerMinuteText()
        {
            string value = GameManager.GameStateManager.CurrencyPerMinute.ToCurrencyString();
            _goldPerMinuteText.text = $"({value}/min)";
        }

        private void SetHpSlider(PlayerHpUpdatedEvent eventType)
        {
            _hpSlider.value = eventType.PercentHp;
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
