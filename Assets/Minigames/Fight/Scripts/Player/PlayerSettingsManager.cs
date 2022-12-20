using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsManager : MonoBehaviour
{
    public PlayerSettings PlayerSettings;
    
    void Start()
    {
        UpgradeItem.upgradePurchased.AddListener(ApplyUpgrade);
    }

    void Update()
    {
        
    }

    private void ApplyUpgrade(Upgrade upgrade)
    {
        
    }
}
