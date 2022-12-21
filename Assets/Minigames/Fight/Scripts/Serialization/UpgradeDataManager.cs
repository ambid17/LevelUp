using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UpgradeDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Upgrades.dat";

    public static void LoadAndApplyData()
    {
        UpgradeData data = FileUtils.LoadFile<UpgradeData>(FileLocation);
        // update settings
        GameManager.UpgradeManager.LoadSerializedUpgrades(data);
    }

    public static void Save()
    {
        // grab values from settings
        List<Upgrade> upgrades = GameManager.UpgradeManager.GetAllUpgrades();
        UpgradeData data = new UpgradeData(upgrades);
        
        // write to file
        FileUtils.SaveFile(FileLocation, data);
    }
}