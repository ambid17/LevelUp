using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fish
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _inventoryButton;
        [SerializeField] private Button _marketButton;
        [SerializeField] private InventoryUI _inventoryUI;
        [SerializeField] private MarketUI _marketUI;
        
        void Start()
        {
            _inventoryButton.onClick.AddListener(OpenInventory);
            _marketButton.onClick.AddListener(OpenMarket);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseAllPanels();
            }
        }

        private void CloseAllPanels()
        {
            _inventoryUI.TogglePanel(false);
            _marketUI.TogglePanel(false);
            _pausePanel.SetActive(false);
        }

        private void OpenInventory()
        {
            _inventoryUI.TogglePanel(true);
        }
        
        private void OpenMarket()
        {
            _marketUI.TogglePanel(true);
        }
    }
}

