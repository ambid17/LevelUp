using System;
using UnityEngine;

namespace Minigames.Fight
{
    public class MeleeEnemyWeaponController : WeaponController
    {
        private bool _isTouchingPlayer = false;
        private EnemyEntity _overridenEntity;

        protected override void Start()
        {
            base.Start();
            _overridenEntity = MyEntity as EnemyEntity;
            if (!_overridenEntity.enemyStats.isPassive)
            {
                _overridenEntity.enemyStats.canShootTarget = true;
            }
        }

        protected override void Update()
        {
            base.Update();
            if (CanShoot())
            {
                _overridenEntity.OverriddenAnimationController.PlayMeleeAttackAnimation();
            }
        }

        protected override bool CanShoot()
        {
            return ShotTimer > weapon.fireRate && _isTouchingPlayer && _overridenEntity.enemyStats.canShootTarget;
        }

        public override void Shoot()
        {
            GameManager.PlayerEntity.TakeHit(Hit);
        }
        
        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                _isTouchingPlayer = true;
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.layer == PhysicsUtils.PlayerLayer)
                _isTouchingPlayer = false;
        }
    }
}
