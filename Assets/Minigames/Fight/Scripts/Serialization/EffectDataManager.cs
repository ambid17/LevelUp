using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EffectDataManager
    {
        private static string FileLocation = $"{Application.persistentDataPath}/Effects.dat";

        public static EffectContainer Load()
        {
            EffectContainer data = FileUtils.LoadFile<EffectContainer>(FileLocation);
            return data;
        }

        public static void Save()
        {
            // grab values from settings
            List<Effect> upgrades = GameManager.SettingsManager.GetUnlockedEffects();
            EffectContainer data = new EffectContainer(upgrades);
        
            // write to file
            FileUtils.SaveFile(FileLocation, data);
        }
    }
}
