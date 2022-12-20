using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public UpgradeData UpgradeData;
}

public class UpgradeData
{
    private List<UpgradeModel> upgrades;

    void Load()
    {
        // load from file
        // update settings
    }

    void Save()
    {
        // grab values from settings
        // write to file
    }
}

public class UpgradeModel
{
    public string name;
    public int numberPurchased;
}
