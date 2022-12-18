using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public UpgradeData UpgradeData;
}

public class UpgradeData
{
    private List<UpgradeSaveItem> upgrades;
}

public class UpgradeSaveItem
{
    public string name;
    public int numberPurchased;
}
