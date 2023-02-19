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
        public Weapon equippedWeapon;

        public void SetDefaults()
        {
            equippedWeapon = null;
        }

        public void EquipWeapon(Weapon weapon)
        {
            equippedWeapon = weapon;
        }

        public void Init()
        {
            if (equippedWeapon == null)
            {
                equippedWeapon = allWeapons.FirstOrDefault(wep => wep.WeaponType == defaultWeaponType);
                
            }
            
            equippedWeapon.Stats.Init();
        }

        public void ApplyUpgrade(WeaponUpgrade upgrade)
        {
            equippedWeapon.Stats.ApplyUpgrade(upgrade);
        }
    }
}