using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeSettings settings;
    public GameObject upgradeItemPrefab;
    public Transform itemParent;

    public Button playerTabButton; 
    public Button weaponTabButton; 

    private List<UpgradeItem> _playerUpgradeItems;
    private List<UpgradeItem> _weaponUpgradeItems;

    public enum UpgradeType
    {
        Player,
        Weapon
    }
    
    void Start()
    {
        InitPlayerUpgrades();
        InitWeaponUpgrades();
        InitTabButtons();
    }

    private void InitPlayerUpgrades()
    {
        _playerUpgradeItems = new List<UpgradeItem>();
        
        foreach (var upgrade in settings.PlayerUpgrades)
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
        foreach (var upgrade in settings.WeaponUpgrades)
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
