using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EnemyProjectileWeaponController : ProjectileWeaponController
    {
        [SerializeField]
        Transform leftShootOffset;
        [SerializeField]
        Transform rightShootOffset;

        private EnemyEntity _overridenEntity;
        private float timeToReachTarget;

        protected override void Start()
        {
            base.Start();
            _overridenEntity = MyEntity as EnemyEntity;
        }
        
        protected override bool ShouldPreventUpdate()
        {
            return false;
        }
        protected override void Update()
        {
            base.Update();
            _overridenEntity.enemyStats.canShootTarget = CanShoot();
        }
        protected override bool CanShoot()
        {
            return ShotTimer > weapon.fireRate;
        }
        
        public override void Shoot()
        {
            ShotTimer = 0;
            EnemyProjectile projectile = Instantiate(overridenWeapon.projectilePrefab) as EnemyProjectile;
            Transform offset = !_overridenEntity.VisualController.SpriteRenderer.flipX ? leftShootOffset : rightShootOffset;
            projectile.transform.position = offset.position;
            
            if (overridenWeapon.bulletSpriteOverride != null)
            {
                projectile.MySpriteRenderer.sprite = overridenWeapon.bulletSpriteOverride;
            }

            Vector2 direction = _overridenEntity.Target.position - projectile.transform.position;

            if (_overridenEntity.enemyStats.predictTargetPosition)
            {
                direction = PredictProjectileDirection(projectile.transform);
            }

            if (destroyOnReachTarget)
            {
                _overridenEntity.enemyStats.ProjectileLifeTime = timeToReachTarget;
            }

            projectile.Setup(_overridenEntity, direction, this);

        }
        private Vector2 PredictProjectileDirection(Transform origin)
        {
            Vector2 targetVelocity = GameManager.PlayerEntity.MovementController.MyRigidbody2D.velocity;

            Vector2 relativePosition = origin.position - _overridenEntity.Target.position;
            float theta = Vector2.Angle(relativePosition, targetVelocity);

            float a = (targetVelocity.magnitude * targetVelocity.magnitude) - (_overridenEntity.enemyStats.ProjectileSpeed * _overridenEntity.enemyStats.ProjectileSpeed) ;
            float b = -2 * Mathf.Cos(theta * Mathf.Deg2Rad) * relativePosition.magnitude * targetVelocity.magnitude;
            float c = relativePosition.magnitude * relativePosition.magnitude;
            float delta = Mathf.Sqrt((b * b) - (4 * a * c));
            timeToReachTarget = -(b + delta) / (2 * a);

            Vector2 prediction = (Vector2)_overridenEntity.Target.position + (targetVelocity * timeToReachTarget);
            Vector2 difference = RandomOffset(prediction) - (Vector2)transform.position;

            return difference.normalized;
        }
        private Vector2 RandomOffset(Vector2 prediction)
        {
            float distance = (prediction - (Vector2)transform.position).magnitude;
            float randomFactor = distance * _overridenEntity.enemyStats.randomProjectileOffset;

            float x = Random.Range(prediction.x - randomFactor, prediction.x + randomFactor);
            float y = Random.Range(prediction.y - randomFactor, prediction.y + randomFactor);

            return new Vector2(x, y);
        }
    }
}