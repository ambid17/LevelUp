using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private float _shotTimer = 0;
    private Camera _camera;

    void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        if (GameManager.Instance.IsDead)
        {
            return;
        }

        TryShoot();
    }
    
    private void TryShoot()
    {
        _shotTimer += Time.deltaTime;
        if (Input.GetMouseButton(0) && _shotTimer > GameManager.Instance.PlayerSettings.shotSpeed)
        {
            _shotTimer = 0;
            
            GameObject projectileGO = Instantiate(projectilePrefab);
            projectileGO.transform.position = transform.position;
            Projectile projectile = projectileGO.GetComponent<Projectile>();
            projectile.SetOwner(Projectile.OwnerType.Player);
            projectile.SetDamage(GameManager.Instance.PlayerSettings.shotDamage);

            
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectile.SetDirection(direction);
        }
    }
}
