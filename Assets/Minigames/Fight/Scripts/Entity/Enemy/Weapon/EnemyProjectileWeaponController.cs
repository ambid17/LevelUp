using System;
using System.Collections;
using System.Collections.Generic;
using Minigames.Fight;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Minigames.Fight
{
    public class EnemyProjectileWeaponController : WeaponController
    {
        [SerializeField] private EnemyProjectile projectilePrefab;
        private Transform _myTransform;
        private EnemyEntity _overridenEntity;

        private void Start()
        {
            _overridenEntity = MyEntity as EnemyEntity;
            _myTransform = transform; // cache our transform for performance
        }
        
        protected override bool ShouldPreventUpdate()
        {
            return false;
        }
        
        protected override bool CanShoot()
        {
            return ShotTimer > weapon.fireRate && _overridenEntity.enemyStats.canShootTarget;
        }
        
        protected override void Shoot()
        {
            EnemyProjectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = transform.position;
            
            Vector2 direction = _overridenEntity.Target.position - _myTransform.position;

            if (_overridenEntity.enemyStats.predictTargetPosition)
            {
                direction = PredictProjectileDirection();
            }

            projectile.Setup(_overridenEntity, direction);
        }
        private Vector2 PredictProjectileDirection()
        {
            Vector2 targetVelocity = GameManager.PlayerEntity.MovementController.MyRigidbody2D.velocity;

            Vector2 relativePosition = transform.position - _overridenEntity.Target.position;
            float theta = Vector2.Angle(relativePosition, targetVelocity);

            float a = (targetVelocity.magnitude * targetVelocity.magnitude) - (_overridenEntity.enemyStats.ProjectileSpeed * _overridenEntity.enemyStats.ProjectileSpeed) ;
            float b = -2 * Mathf.Cos(theta * Mathf.Deg2Rad) * relativePosition.magnitude * targetVelocity.magnitude;
            float c = relativePosition.magnitude * relativePosition.magnitude;
            float delta = Mathf.Sqrt((b * b) - (4 * a * c));
            float t = -(b + delta) / (2 * a);

            Vector2 prediction = (Vector2)_overridenEntity.Target.position + (targetVelocity * t);
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