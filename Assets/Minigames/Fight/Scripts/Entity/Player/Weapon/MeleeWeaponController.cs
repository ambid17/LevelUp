using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeWeaponController : WeaponController
    { 
        [SerializeField] protected MeleeWeapon overridenWeapon;
        [SerializeField] protected MeleeWeaponInstance meleeWeaponInstance;
        private Camera _camera;
        
        protected override void Start()
        {
            base.Start();
            overridenWeapon = weapon as MeleeWeapon;
            meleeWeaponInstance = Instantiate(overridenWeapon.weaponPrefab, transform);
            _camera = Camera.main;
        }
        
        protected override void Shoot()
        {
            Vector2 position = transform.position.AsVector2();
            Vector2 direction = _camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            RaycastHit2D[] hits = Physics2D.RaycastAll(position, direction, overridenWeapon.attackRange);
        
            foreach (var hit in hits)
            {
                if (hit.transform.gameObject.layer == PhysicsUtils.EnemyLayer)
                {
                    EnemyEntity enemyEntity = hit.transform.gameObject.GetComponent<EnemyEntity>();
                    enemyEntity.TakeHit(Hit);
                }
            }
        
        }
    }
}