using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private GameObject upgradeItemPrefab;
        [SerializeField] private Transform itemParent;

        [SerializeField] private Button playerTabButton; 
        [SerializeField] private Button weaponTabButton; 
        [SerializeField] private Button enemyTabButton; 
        [SerializeField] private Button incomeTabButton; 
        [SerializeField] private Button closeButton; 

        private List<UpgradeItem> _playerUpgradeItems;
        private List<UpgradeItem> _weaponUpgradeItems;
        private List<UpgradeItem> _enemyUpgradeItems;
        private List<UpgradeItem> _incomeUpgradeItems;

        public enum UpgradeType
        {
            Player,
            Weapon,
            Enemy,
            Income
        }
    
        void Start()
        {
            closeButton.onClick.AddListener(Close);
            InitPlayerUpgrades();
            InitWeaponUpgrades();
            InitEnemyUpgrades();
            InitIncomeUpgrades();
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
        
        private void InitEnemyUpgrades()
        {
            _enemyUpgradeItems = new List<UpgradeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.EnemyUpgrades)
            {
                var itemInstance = Instantiate(upgradeItemPrefab, itemParent);
                UpgradeItem item = itemInstance.GetComponent<UpgradeItem>();
                item.Setup(upgrade);
                _enemyUpgradeItems.Add(item);
            }
        }
        
        private void InitIncomeUpgrades()
        {
            _incomeUpgradeItems = new List<UpgradeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.IncomeUpgrades)
            {
                var itemInstance = Instantiate(upgradeItemPrefab, itemParent);
                UpgradeItem item = itemInstance.GetComponent<UpgradeItem>();
                item.Setup(upgrade);
                _incomeUpgradeItems.Add(item);
            }
        }

        private void InitTabButtons()
        {
            playerTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Player));
            weaponTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Weapon));
            enemyTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Enemy));
            incomeTabButton.onClick.AddListener(() =>ToggleUpgradeItems(UpgradeType.Income));
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
            
            foreach (var item in _enemyUpgradeItems)
            {
                item.gameObject.SetActive(upgradeType == UpgradeType.Enemy);
            }
            
            foreach (var item in _incomeUpgradeItems)
            {
                item.gameObject.SetActive(upgradeType == UpgradeType.Income);
            }
        }
    }
}
