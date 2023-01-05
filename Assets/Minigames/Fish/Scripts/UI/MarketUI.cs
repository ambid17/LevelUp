using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketUI : MonoBehaviour
{
    //[SerializeField] private MarketItem _inventoryItemPrefab;
    [SerializeField] private Transform _parent;
    [SerializeField] private GameObject _container;
    
    void Start()
    {
        TogglePanel(false);
    }

    void Update()
    {
        
    }
    
    public void TogglePanel(bool shouldBeActive)
    {
        _container.SetActive(shouldBeActive);
    }
}
