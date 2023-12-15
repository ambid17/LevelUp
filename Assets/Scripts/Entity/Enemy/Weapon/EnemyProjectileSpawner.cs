using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Minigames.Fight
{
    /// <summary>
    /// If the enemy gets to the point of spawning a projectile and the animation gets interrupted (by death or CC)
    /// this creates the graphic for the projectile being spawned, and spawns the projectile.
    /// 
    /// This guarantees that if an enemy has reached the point in their animation where they shoot, that the shot's animation goes off
    /// </summary>
    public class EnemyProjectileSpawner : ProjectileController
    {
        public Transform Offset => offSet;

        [SerializeField]
        private Animator anim;
        [SerializeField]
        private Transform offSet;
        [SerializeField]
        private ProjectileController projectilePrefab;
        [SerializeField]
        private Sprite projectileSprite;
        [SerializeField]
        private AnimatorController myAnimation;
        [SerializeField]
        private float startAngle = 180;

        private Entity _overridenEntity;
        private Vector2 _direction;
        private float _storedLifetimeOverride;
        private bool _isDoneSpawning;
        

        public override void Setup(Entity myEntity, WeaponStats weapon, Vector2 direction, float lifetimeOverride = 0)
        {
            _overridenEntity = myEntity;
            _direction = direction;
            _myWeaponStats = weapon;
            _storedLifetimeOverride = lifetimeOverride;
        }

        public void FaceTarget(Vector2 target)
        {
            transform.rotation = PhysicsUtils.LookAt(transform, target, startAngle);
        }

        protected override void Move()
        {
        }

        protected override bool ShouldDie()
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && _isDoneSpawning;
        }

        /// <summary>
        /// Called from animation
        /// </summary>
        public void SpawnProjectile()
        {
            ProjectileController projectile = Instantiate(projectilePrefab);
            projectile.transform.position = offSet.position;
            projectile.Setup(_overridenEntity, _myWeaponStats, _direction, _storedLifetimeOverride);
            projectile.OverrideVisuals(projectileSprite, myAnimation);
            _isDoneSpawning = true;
        }
    }
}

