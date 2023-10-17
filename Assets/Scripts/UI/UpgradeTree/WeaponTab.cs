using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Fight
{
    public class WeaponTab : MonoBehaviour
    {
        public Weapon Weapon;

        [SerializeField] private Image iconImage;

        public void Setup(Weapon weapon)
        {
            Weapon = weapon;
            iconImage.sprite = weapon.icon;
        }
    }
}