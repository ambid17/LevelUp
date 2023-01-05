using System.Collections;
using System.Collections.Generic;
using Minigames.Fish;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryItem _inventoryItemPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _container;
    
    void Start()
    {
        SetupItems();
        TogglePanel(false);
    }

    private void SetupItems()
    {
        foreach (var fish in GameManager.FishSettings.Fish)
        {
            InventoryItem item = Instantiate(_inventoryItemPrefab, _parent);
            item.Setup(fish);
        }
    }

    public void TogglePanel(bool shouldBeActive)
    {
        _container.SetActive(shouldBeActive);
    }
}
