using System;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        private EnemyEntity _overridenEntity;
        private MeleeWeapon _overridenWeapon;

        protected override void Start()
        {
            base.Start();
            _overridenEntity = MyEntity as EnemyEntity;
            _overridenWeapon = weapon as MeleeWeapon;
        }

        protected override void Update()
        {
            base.Update();
            _overridenEntity.enemyStats.canMeleeTarget = CanShoot();
        }

        protected override bool CanShoot()
        {
            return ShotTimer > _overridenWeapon.fireRate;
        }

        public override void Shoot()
        {
            ShotTimer = 0;
            if (Vector2.Distance(transform.position, GameManager.PlayerEntity.transform.position) > _overridenWeapon.attackRange)
            {
                return;
            }
            GameManager.PlayerEntity.TakeHit(Hit);
        }
        
    }
}
