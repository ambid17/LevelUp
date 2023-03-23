using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    private Button _button;
    private Weapon _weapon;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectWeapon);
    }

    public void Setup(Weapon weapon)
    {
        _weapon = weapon;
        weaponImage.sprite = weapon.icon;
    }

    private void SelectWeapon()
    {
        GameManager.EventService.Dispatch(new WeaponSelectedEvent(_weapon));
    }
}
