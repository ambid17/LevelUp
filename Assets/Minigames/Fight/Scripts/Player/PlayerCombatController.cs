using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    private List<Weapon> weapons;

    void Start()
    {
        // TODO: reach into the save and init the weapons
    }

    void Update()
    {
        if (GameManager.Instance.IsDead)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            TryShootWeapons();
        }
    }
    
    private void TryShootWeapons()
    {
        foreach (var weapon in weapons)
        {
            weapon.TryShoot();
        }
    }
}
