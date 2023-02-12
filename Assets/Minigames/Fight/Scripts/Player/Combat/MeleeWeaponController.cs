using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeWeaponController : WeaponController
    {
        [SerializeField] private MeleeWeapon overridenWeapon;

        private void Start()
        {
            overridenWeapon = _weapon as MeleeWeapon;
        }
        
        void Update()
        {
            if (GameManager.PlayerStatusController.IsDead)
            {
                return;
            }
        
            _shotTimer += Time.deltaTime;

            if (Input.GetMouseButton(0) && _shotTimer > _weapon.Stats.FireRate)
            {
                _shotTimer = 0;
                Shoot();
            }
        }
    
        private void Shoot()
        {
            Vector2 position = transform.position.AsVector2();
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, 4);

            float damage = CalculateDamage();
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.layer == PhysicsUtils.EnemyLayer)
                {
                    EnemyController enemy = hit.transform.gameObject.GetComponent<EnemyController>();
                    enemy.TakeDamage(damage);

                    Vector2 knockback = enemy.transform.position.AsVector2() - transform.position.AsVector2();
                    knockback = knockback.normalized * overridenWeapon.knockbackDistance;
                    enemy.Knockback(knockback);
                    
                    if (GameManager.SettingsManager.playerSettings.LifeSteal > 0)
                    {
                        _eventService.Dispatch(new OnLifestealEvent(damage));
                    }
                }
            }
        
        }
    }
}