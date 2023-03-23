using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField] private WeaponButton weaponButtonPrefab;
    [SerializeField] private TMP_Text weaponNameText;
    [SerializeField] private TMP_Text weaponDescriptionText;
    [SerializeField] private Button weaponEquipbutton;
    [SerializeField] private Transform weaponButtonParent;

    private Weapon selectedWeapon;
    void Start()
    {
        PopulateWeaponList();
        GameManager.EventService.Add<WeaponSelectedEvent>(OnWeaponSelected);
        weaponEquipbutton.onClick.AddListener(EquipWeapon);
    }

    private void PopulateWeaponList()
    {
        bool isFirstWeapon = true;
        foreach (var weapon in GameManager.WeaponSettings.allWeapons)
        {
            WeaponButton button = Instantiate(weaponButtonPrefab, weaponButtonParent);
            button.Setup(weapon);

            if (isFirstWeapon)
            {
                isFirstWeapon = false;
                OnWeaponSelected(new WeaponSelectedEvent(weapon));
            }
        }
    }

    private void OnWeaponSelected(WeaponSelectedEvent e)
    {
        selectedWeapon = e.Weapon;
        weaponNameText.text = selectedWeapon.readableName;
        weaponDescriptionText.text = selectedWeapon.description;
    }

    private void EquipWeapon()
    {
        GameManager.WeaponSettings.equippedWeapon = selectedWeapon;
    }
}
