using FunkyCode.LightingSettings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Minigames.Fight
{
    public class EnemyWeaponController : WeaponController
    {
        public Vector2 CurrentOffsetPosition => CurrentWeaponMode == WeaponMode.Projectile ? shootOffset.position : meleeOffset.position;
        protected Transform shootOffset => MyEntity.VisualController.SpriteRenderer.flipX ? flippedShootOffset : unflippedShootOffset;
        protected Transform meleeOffset => MyEntity.VisualController.SpriteRenderer.flipX ? flippedMeleeOffset : unflippedMeleeOffset;

        [SerializeField]
        private Transform unflippedShootOffset;
        [SerializeField]
        private Transform flippedShootOffset;
        [SerializeField]
        private Transform flippedMeleeOffset;
        [SerializeField]
        private Transform unflippedMeleeOffset;

        [SerializeField]
        private bool _destroyOnReachTarget;

        protected Vector2 _storedDirection;
        protected Vector2 _storedTarget;
        protected Vector2 _storedVelocity;
        protected Vector2 _storedPrediction;

        private float _timeToReachTarget;

        public override void Shoot()
        {
            ProjectileController projectile = Instantiate(_combatStats.projectileWeaponStats.projectilePrefab);

            Transform offset = shootOffset;
            projectile.transform.position = offset.position;

            Vector2 direction;

            EnemyProjectileSpawner projectileSpawner = projectile as EnemyProjectileSpawner;

            if (projectileSpawner != null)
            {
                // Run algorithm twice to approximate the correct position for the projectilespawner offset.
                projectileSpawner.FaceTarget(_storedTarget);
                direction = PredictProjectileDirection(projectileSpawner.Offset.position);
                projectileSpawner.FaceTarget(_storedPrediction);
                direction = PredictProjectileDirection(projectileSpawner.Offset.position);
            }
            else
            {
                direction = PredictProjectileDirection(projectile.transform.position);
            }
            float lifeTimeOverride = 0;

            if (_destroyOnReachTarget)
            {
                lifeTimeOverride = _timeToReachTarget;
            }
            // Set weapon mode here instead of anywhere else to ensure it's the same frame as projectile setting up.
            CurrentWeaponMode = WeaponMode.Projectile;
            projectile.Setup(MyEntity, MyEntity.Stats.combatStats.projectileWeaponStats, direction, lifeTimeOverride);
        }

        public override bool CanShoot()
        {
            return ShootTimer >= _combatStats.projectileWeaponStats.rateOfFire.Calculated;
        }

        public override bool CanMelee()
        {
            // Removed the requirement to be within range.
            // The pursue distance now checks the distance relative to the position the melee object instantiate instead of transform.position
            // this means the enemy will never be able to attack. Additionally the entire purpose for checking proximity was that damage was applied directly,
            // now the melee VFX actually have to touch the player making this check unecessary.
            return MeleeTimer >= _combatStats.meleeWeaponStats.rateOfFire.Calculated;
        }

        public override void Melee()
        {
            ProjectileController melee = Instantiate(_combatStats.meleeWeaponStats.projectilePrefab);

            melee.transform.position = meleeOffset.position;
            melee.transform.rotation = PhysicsUtils.LookAt(melee.transform, _storedTarget, 180);

            // Set weapon mode here instead of anywhere else to ensure it's the same frame as projectile setting up.
            CurrentWeaponMode = WeaponMode.Melee;

            float lifetimeOverride = Vector2.Distance(melee.transform.position, _storedTarget) / _combatStats.meleeWeaponStats.projectileMoveSpeed.Calculated;

            melee.Setup(MyEntity, MyEntity.Stats.combatStats.meleeWeaponStats , _storedDirection, lifetimeOverride);
        }

        // Called by animator to ensure less than perfect aim.
        // Resets melee timer to make animation cancelling more effective.
        public void SetMeleeDirection()
        {
            MeleeTimer = 0;
            _storedDirection = GameManager.PlayerEntity.transform.position - meleeOffset.position;
            _storedTarget = GameManager.PlayerEntity.transform.position;
            EnemyVisualController myVC = MyEntity.VisualController as EnemyVisualController;
            myVC.FaceTarget(_storedTarget);
        }

        // Called by animator to ensure less than perfect aim.
        // Resets shot timer to make animation cancelling more effective.
        public void SetProjectileDirection()
        { 
            ShootTimer = 0;
            _storedTarget = GameManager.PlayerEntity.transform.position;
            _storedVelocity = GameManager.PlayerEntity.Rigidbody2D.velocity;
            EnemyVisualController myVC = MyEntity.VisualController as EnemyVisualController;
            myVC.FaceTarget(_storedTarget);
        }

        private Vector2 PredictProjectileDirection(Vector2 origin)
        {
            // TODO: equation only works if projectile is faster than target, need a check with a secondary equation.

            Vector2 targetVelocity = _storedVelocity;
            Vector2 direction = _storedTarget - origin;
            Vector2 relativePosition = origin - direction;
            float theta = Vector2.Angle(relativePosition, targetVelocity);

            float a = (targetVelocity.magnitude * targetVelocity.magnitude) - (_combatStats.projectileWeaponStats.projectileMoveSpeed.Calculated * _combatStats.projectileWeaponStats.projectileMoveSpeed.Calculated);
            float b = -2 * Mathf.Cos(theta * Mathf.Deg2Rad) * relativePosition.magnitude * targetVelocity.magnitude;
            float c = relativePosition.magnitude * relativePosition.magnitude;
            float delta = Mathf.Sqrt((b * b) - (4 * a * c));
            _timeToReachTarget = -(b + delta) / (2 * a);

            _storedPrediction = _storedTarget + (targetVelocity * _timeToReachTarget);
            Vector2 difference = _storedPrediction - origin;

            return difference.normalized;
        }

        [ContextMenu("Setup")]
        public void SetupInspector()
        {
            unflippedShootOffset = transform;
            unflippedMeleeOffset = transform;
            flippedMeleeOffset = transform;
            flippedShootOffset = transform;
        }
    }
}