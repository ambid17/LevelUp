using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class WeaponDataManager
    {
        private static string FileLocation = $"{Application.persistentDataPath}/Weapon.dat";

        public static WeaponModel Load()
        {
            WeaponModel data = FileUtils.LoadFile<WeaponModel>(FileLocation);
            return data;
        }

        public static void Save(WeaponSettings weaponSettings)
        {
            WeaponModel data = weaponSettings.ToModel();
            FileUtils.SaveFile(FileLocation, data);
        }
    }
}