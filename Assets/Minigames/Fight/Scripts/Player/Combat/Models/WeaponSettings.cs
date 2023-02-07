using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Minigames.Fight
{
    

    [CreateAssetMenu(fileName = "WeaponSettings", menuName = "ScriptableObjects/Fight/WeaponSettings", order = 1)]
    [Serializable]
    public class WeaponSettings : ScriptableObject
    {        
        [Header("Set in Editor")]
        public WeaponType defaultWeaponType = WeaponType.Pistol;
        public List<Weapon> allWeapons;

        [Header("Set at Runtime")]
        public List<Weapon> equippedWeapons;

        public void EquipWeapon(Weapon weapon)
        {
            if (!equippedWeapons.Contains(weapon))
            {
                equippedWeapons.Add(weapon);
            }
            else
            {
                Debug.LogError("weapon already equipped");
            }
        }

        public void Init()
        {
            if (equippedWeapons == null)
            {
                equippedWeapons = new List<Weapon>();
            }
            
            if (equippedWeapons.Count == 0)
            {
                equippedWeapons.Add(allWeapons.FirstOrDefault(wep => wep.WeaponType == defaultWeaponType));
            }
            
            foreach (var equippedWeapon in equippedWeapons)
            {
                equippedWeapon.Stats.Init();
            }
        }

        public void ApplyUpgrade(WeaponUpgrade upgrade)
        {
            var weapon = equippedWeapons.FirstOrDefault(w => w == upgrade.weapon);
            weapon.Stats.ApplyUpgrade(upgrade);
        }
    }
}