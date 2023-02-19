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
    
        protected override void Shoot()
        {
            Vector2 position = transform.position.AsVector2();
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, 4);

            float damage = CalculateDamage();
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.layer == PhysicsUtils.EnemyLayer)
                {
                    EnemyEntity enemyEntity = hit.transform.gameObject.GetComponent<EnemyEntity>();

                    HitData hitData = new HitData(myEntity, enemyEntity);

                    enemyEntity.TakeHit(hitData);

                    // Vector2 knockback = enemyEntity.transform.position.AsVector2() - transform.position.AsVector2();
                    // knockback = knockback.normalized * overridenWeapon.knockbackDistance;
                    // enemyEntity.Knockback(knockback);
                    //
                    // if (GameManager.SettingsManager.playerSettings.LifeSteal > 0)
                    // {
                    //     _eventService.Dispatch(new OnLifestealEvent(damage));
                    // }
                }
            }
        
        }
    }
}