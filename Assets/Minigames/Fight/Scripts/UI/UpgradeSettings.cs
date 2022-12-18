using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSettings", menuName = "ScriptableObjects/UpgradeSettings", order = 1)]
[Serializable]
public class UpgradeSettings : ScriptableObject
{
    public List<Upgrade> upgrades;
}

[Serializable]
public class Upgrade
{
    public string name;
    public string description;
    public int numberPurchased;
    public int maxPurchases;
    public int baseCost;
    public float costScalar;

    public string GetUpgradeCountText()
    {
        string upgradeCountText = $"{numberPurchased}";

        if (maxPurchases > 0)
        {
            upgradeCountText += $" / {maxPurchases}";
        }

        upgradeCountText += " Purchased";

        return upgradeCountText;
    }

    public float GetCost()
    {
        return Mathf.Pow(baseCost, costScalar * numberPurchased);
    }
}
