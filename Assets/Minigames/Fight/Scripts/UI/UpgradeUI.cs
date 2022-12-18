using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    public UpgradeSettings settings;
    public GameObject upgradeItemPrefab;
    public Transform itemParent;
    
    void Start()
    {
    }

    void OnEnable()
    {
        foreach (var upgrade in settings.upgrades)
        {
            var itemInstance = Instantiate(upgradeItemPrefab, itemParent);
            UpgradeItem item = itemInstance.GetComponent<UpgradeItem>();
            item.Setup(upgrade);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
