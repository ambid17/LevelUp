using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fish
{
    public class MarketUI : MonoBehaviour
    {
        [SerializeField] private MarketItem _marketItemPrefab;
        [SerializeField] private Transform _marketItemParent;
        
        [SerializeField] private MarketItem _upgradeItemPrefab;
        [SerializeField] private Transform _upgradeItemParent;

        [SerializeField] private GameObject _marketScrollView;
        [SerializeField] private GameObject _upgradeScrollView;

        [SerializeField] private Button _marketButton;
        [SerializeField] private Button _upgradeButton;
        
        [SerializeField] private GameObject _container;
    
        void Start()
        {
            SetupMarketItems();
            SetupUpgradeItems();
            TogglePanel(false);
            
            _marketButton.onClick.AddListener(() => ShowTab(true));
            _upgradeButton.onClick.AddListener(() => ShowTab(false));
            ShowTab(true);
        }

        private void ShowTab(bool isMarket)
        {
            _marketScrollView.SetActive(isMarket);
            _upgradeScrollView.SetActive(!isMarket);
        }

        private void SetupMarketItems()
        {
            foreach (var fish in GameManager.FishSettings.Fish)
            {
                MarketItem item = Instantiate(_marketItemPrefab, _marketItemParent);
                item.Setup(fish);
            }
        }
        
        private void SetupUpgradeItems()
        {
            // foreach (var fish in GameManager.FishSettings.Fish)
            // {
            //     MarketItem item = Instantiate(_upgradeItemPrefab, _upgradeItemParent);
            //     item.Setup(fish);
            // }
        }
    
        public void TogglePanel(bool shouldBeActive)
        {
            _container.SetActive(shouldBeActive);
        }

        public bool IsActive()
        {
            return _container.activeInHierarchy;
        }
    }
}

