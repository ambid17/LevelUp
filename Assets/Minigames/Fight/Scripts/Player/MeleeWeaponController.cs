using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeWeaponController : WeaponController
    {
        void Update()
        {
            if (GameManager.GameStateManager.IsDead)
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
                    
                    if (GameManager.SettingsManager.playerSettings.LifeSteal > 0)
                    {
                        GameManager.GameStateManager.CurrentPlayerHP += damage * GameManager.SettingsManager.playerSettings.LifeSteal;
                    }
                }
            }
        
        }
    }
}