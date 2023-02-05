using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class UpgradeTree : MonoBehaviour
    {
        [SerializeField] private UpgradeTreeItem upgradeItemPrefab;
        [SerializeField] private UpgradeTier upgradeTierPrefab;
        [SerializeField] private Transform tierParent;
        [SerializeField] private UpgradeInspector inspector;

        [SerializeField] private Button playerTabButton;
        [SerializeField] private Button weaponTabButton;
        [SerializeField] private Button enemyTabButton;
        [SerializeField] private Button incomeTabButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private Button purchaseCountButton;

        private List<UpgradeTreeItem> _playerUpgradeItems;
        private List<UpgradeTreeItem> _weaponUpgradeItems;
        private List<UpgradeTreeItem> _enemyUpgradeItems;
        private List<UpgradeTreeItem> _incomeUpgradeItems;
        private List<UpgradeTier> _upgradeTiers;


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

            _upgradeTiers = new List<UpgradeTier>();
            InitPlayerUpgrades();
            InitWeaponUpgrades();
            InitEnemyUpgrades();
            InitIncomeUpgrades();
            InitTabButtons();
            ToggleUpgradeItems(UpgradeType.Player);
            inspector.OnUpgradeSelected(new UpgradeSelectedEvent(_playerUpgradeItems[0].upgrade));
        }

        private void Close()
        {
            gameObject.SetActive(false);
        }

        private void InitPlayerUpgrades()
        {
            _playerUpgradeItems = new List<UpgradeTreeItem>();
        
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.PlayerUpgrades)
            {
                var item = Instantiate(upgradeItemPrefab, GetTierForUpgrade(upgrade));
                item.Setup(upgrade);
                _playerUpgradeItems.Add(item);
            }
        }
    
        private void InitWeaponUpgrades()
        {
            _weaponUpgradeItems = new List<UpgradeTreeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.WeaponUpgrades)
            {
                var item = Instantiate(upgradeItemPrefab, GetTierForUpgrade(upgrade));
                item.Setup(upgrade);
                _weaponUpgradeItems.Add(item);
            }
        }
        
        private void InitEnemyUpgrades()
        {
            _enemyUpgradeItems = new List<UpgradeTreeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.EnemyUpgrades)
            {
                var item = Instantiate(upgradeItemPrefab, GetTierForUpgrade(upgrade));
                item.Setup(upgrade);
                _enemyUpgradeItems.Add(item);
            }
        }
        
        private void InitIncomeUpgrades()
        {
            _incomeUpgradeItems = new List<UpgradeTreeItem>();
            foreach (var upgrade in GameManager.SettingsManager.upgradeSettings.IncomeUpgrades)
            {
                var item = Instantiate(upgradeItemPrefab, GetTierForUpgrade(upgrade));
                item.Setup(upgrade);
                _incomeUpgradeItems.Add(item);
            }
        }

        private Transform GetTierForUpgrade(Upgrade upgrade)
        {
            var tier = _upgradeTiers.FirstOrDefault(t => t.tier == upgrade.tier);

            if (tier == null)
            {
                tier = Instantiate(upgradeTierPrefab, tierParent);
                tier.tier = upgrade.tier;
            }
            
            _upgradeTiers.Add(tier);

            return tier.transform;
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