using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected WeaponSetting setting;
    
    private float _shotTimer = 0;
    
    void Start()
    {
    }

    void Update()
    {
        _shotTimer += Time.deltaTime;
    }

    public void TryShoot()
    {
        if (_shotTimer > setting.FireRate)
        {
            _shotTimer = 0;
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
    }
}
