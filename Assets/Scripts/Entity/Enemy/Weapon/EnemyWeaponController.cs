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
        private Transform shootOffset => MyEntity.VisualController.SpriteRenderer.flipX ? flippedShootOffset : unflippedShootOffset;
        private Transform meleeOffset => MyEntity.VisualController.SpriteRenderer.flipX ? flippedMeleeOffset : unflippedMeleeOffset;

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

        private Vector2 _storedDirection;
        private Vector2 _storedTarget;

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
                direction = PredictProjectileDirection(projectileSpawner.Offset.position);
            }
            else
            {
                direction = PredictProjectileDirection(projectile.transform.position);
            }

            if (_destroyOnReachTarget)
            {
                _combatStats.projectileWeaponStats.projectileLifeTime.OverrideStat(_timeToReachTarget);
            }
            // Set weapon mode here instead of anywhere else to ensure it's the same frame as projectile setting up.
            CurrentWeaponMode = WeaponMode.Projectile;
            projectile.Setup(MyEntity, direction);
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
            return ShootTimer >= _combatStats.meleeWeaponStats.rateOfFire.Calculated;
        }

        public override void Melee()
        {
            ProjectileController melee = Instantiate(_combatStats.meleeWeaponStats.projectilePrefab);

            melee.transform.position = meleeOffset.position;
            melee.transform.rotation = PhysicsUtils.LookAt(transform, _storedTarget, 180);

            // Set weapon mode here instead of anywhere else to ensure it's the same frame as projectile setting up.
            CurrentWeaponMode = WeaponMode.Melee;

            melee.Setup(MyEntity, _storedDirection);
        }

        // Called by animator to ensure less than perfect aim.
        // Resets melee timer to make animation cancelling more effective.
        public void SetMeleeDirection()
        {
            MeleeTimer = 0;
            _storedDirection = GameManager.PlayerEntity.transform.position - meleeOffset.position;
            _storedTarget = GameManager.PlayerEntity.transform.position;
        }

        // Called by animator to ensure less than perfect aim.
        // Resets shot timer to make animation cancelling more effective.
        public void SetProjectileDirection()
        {
            ShootTimer = 0;
            _storedDirection = GameManager.PlayerEntity.transform.position - shootOffset.position;
            _storedTarget = GameManager.PlayerEntity.transform.position;
        }

        private Vector2 PredictProjectileDirection(Vector2 origin)
        {
            Vector2 targetVelocity = GameManager.PlayerEntity.Rigidbody2D.velocity;

            Vector2 relativePosition = origin - _storedDirection;
            float theta = Vector2.Angle(relativePosition, targetVelocity);

            float a = (targetVelocity.magnitude * targetVelocity.magnitude) - (_combatStats.projectileWeaponStats.projectileMoveSpeed.Calculated * _combatStats.projectileWeaponStats.projectileMoveSpeed.Calculated);
            float b = -2 * Mathf.Cos(theta * Mathf.Deg2Rad) * relativePosition.magnitude * targetVelocity.magnitude;
            float c = relativePosition.magnitude * relativePosition.magnitude;
            float delta = Mathf.Sqrt((b * b) - (4 * a * c));
            _timeToReachTarget = -(b + delta) / (2 * a);

            Vector2 prediction = _storedTarget + (targetVelocity * _timeToReachTarget);
            Vector2 difference = prediction - origin;

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