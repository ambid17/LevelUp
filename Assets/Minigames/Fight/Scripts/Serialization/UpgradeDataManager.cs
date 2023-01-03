using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class UpgradeDataManager
    {
        private static string FileLocation = $"{Application.persistentDataPath}/Upgrades.dat";

        public static UpgradeData Load()
        {
            UpgradeData data = FileUtils.LoadFile<UpgradeData>(FileLocation);
            return data;
        }

        public static void Save()
        {
            // grab values from settings
            List<Upgrade> upgrades = GameManager.SettingsManager.GetAllUpgrades();
            UpgradeData data = new UpgradeData(upgrades);
        
            // write to file
            FileUtils.SaveFile(FileLocation, data);
        }
    }
}