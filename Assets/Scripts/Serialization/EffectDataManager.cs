using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EffectDataManager
    {
        private static string FileLocation = $"{Application.persistentDataPath}/Effects.dat";

        public static UpgradeContainer Load()
        {
            UpgradeContainer data = FileUtils.LoadFile<UpgradeContainer>(FileLocation);
            return data;
        }

        public static void Save()
        {
            // grab values from settings
            List<Upgrade> upgrades = GameManager.EffectSettings.AllUpgrades;
            UpgradeContainer data = new UpgradeContainer(upgrades);
        
            // write to file
            FileUtils.SaveFile(FileLocation, data);
        }
    }
}
