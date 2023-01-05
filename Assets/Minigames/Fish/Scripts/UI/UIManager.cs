using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fish
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _marketButton;
        [SerializeField] private MarketUI _marketUI;
        
        void Start()
        {
            _marketButton.onClick.AddListener(OpenMarket);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseAllPanels();

                if (!_marketUI.IsActive())
                {
                    _pausePanel.SetActive(!_pausePanel.activeInHierarchy);
                }
            }
        }

        private void CloseAllPanels()
        {
            _marketUI.TogglePanel(false);
            _pausePanel.SetActive(false);
        }
        
        private void OpenMarket()
        {
            _marketUI.TogglePanel(true);
        }
    }
}

