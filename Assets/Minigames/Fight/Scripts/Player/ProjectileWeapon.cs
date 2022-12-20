using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    [SerializeField] private GameObject projectilePrefab;
    private Camera _camera;
    
    void Start()
    {
        _camera = Camera.main;
    }

    protected override void Shoot()
    {
        GameObject projectileGO = Instantiate(projectilePrefab);
        projectileGO.transform.position = transform.position;
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.SetOwner(Projectile.OwnerType.Player);
        projectile.SetDamage(setting.Damage);
            
        Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        projectile.SetDirection(direction);
    }
}
