using System;
using System.Collections.Generic;
using System.Linq;
using Minigames.Fight;
using UnityEngine;

namespace Minigames.Fight
{
    public enum WeaponType
    {
        Pistol,
        RocketLauncher,
        Shotgun,
        MachineGun,
        Katana,
        Hammer,
        Shield
    }

    [Serializable]
    public class Weapon : ScriptableObject
    {
        public WeaponType weaponType;
        public Sprite icon;
        public Sprite ammoIcon;
        public Sprite abilityIcon;
        public float abilityCooldown;
        public float fireRate;
        public float damage;

        public void Init()
        {
            
        }
    }
}