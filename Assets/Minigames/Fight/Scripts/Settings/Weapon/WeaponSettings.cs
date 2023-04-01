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

        public void Init()
        {
            if (equippedWeapon == null)
            {
                equippedWeapon = allWeapons.FirstOrDefault(wep => wep.weaponType == defaultWeaponType);
            }
            
            equippedWeapon.Init();
        }

        public WeaponModel ToModel()
        {
            WeaponModel model = new WeaponModel();
            model.equippedWeaponType = equippedWeapon.weaponType;
            return model;
        }

        public void FromModel(WeaponModel model)
        {
            equippedWeapon = allWeapons.FirstOrDefault(w => w.weaponType == model.equippedWeaponType);
        }
    }
}