using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class WeaponSpawner : MonoBehaviour
    {
        void Start()
        {
            Weapon equippedWeapon = GameManager.SettingsManager.weaponSettings.equippedWeapon;
            var weaponGO = Instantiate(equippedWeapon.Prefab, transform);
            var weaponController = weaponGO.GetComponent<WeaponController>();
            weaponController.Setup(equippedWeapon);
        }
    }
}