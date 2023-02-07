using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class WeaponSpawner : MonoBehaviour
    {
        void Start()
        {
            foreach (var weapon in GameManager.SettingsManager.weaponSettings.equippedWeapons)
            {
                var weaponGO = Instantiate(weapon.Prefab, transform);
                var weaponController = weaponGO.GetComponent<WeaponController>();
                weaponController.Setup(weapon);
            }
        }
    }
}