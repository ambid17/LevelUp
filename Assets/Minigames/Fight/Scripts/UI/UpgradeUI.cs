using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class UpgradeUI : MonoBehaviour
    {
        public GameObject upgradeItemPrefab;
        public Transform itemParent;

        public Button playerTabButton; 
        public Button weaponTabButton; 
        public Button closeButton; 

        private List<UpgradeItem> _playerUpgradeItems;
        private List<UpgradeItem> _weaponUpgradeItems;

        public enum UpgradeType
        {
            Player,
            Weapon
        }
    
        void Start()
        {
            closeButton.onClick.AddListener(Close);
            InitPlayerUpgrades();
            InitWeaponUpgrades();
            InitTabButtons();
            ToggleUpgradeItems(UpgradeType.Player);
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void InitPlayerUpgrades()
        {
            _playerUpgradeItems = new List<UpgradeItem>();
        
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.PlayerUpgrades)
            {
                var itemInstance = Instantiate(upgradeItemPrefab, itemParent);
                UpgradeItem item = itemInstance.GetComponent<UpgradeItem>();
                item.Setup(upgrade);
                _playerUpgradeItems.Add(item);
            }
        }
    
        private void InitWeaponUpgrades()
        {
            _weaponUpgradeItems = new List<UpgradeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.WeaponUpgrades)
            {
                var itemInstance = Instantiate(upgradeItemPrefab, itemParent);
                UpgradeItem item = itemInstance.GetComponent<UpgradeItem>();
                item.Setup(upgrade);
                _weaponUpgradeItems.Add(item);
            }
        }

        private void InitTabButtons()
        {
            playerTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Player));
            weaponTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Weapon));
        }

        private void ToggleUpgradeItems(UpgradeType upgradeType)
        {
            foreach (var item in _playerUpgradeItems)
            {
                item.gameObject.SetActive(upgradeType == UpgradeType.Player);
            }
        
            foreach (var item in _weaponUpgradeItems)
            {
                item.gameObject.SetActive(upgradeType == UpgradeType.Weapon);
            }
        }
    }
}
