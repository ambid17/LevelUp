using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ApplyUpgrade(Upgrade upgrade)
    {
        if (upgrade.name == "X")
        {
            GameManager.Instance.PlayerSettings.SetShotSpeed(upgrade);
        }
    }
}
