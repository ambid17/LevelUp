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
        private AnimatorController animation;
        [SerializeField]
        private float angleOffset = -180;

        private Entity _overridenEntity;
        private Vector2 _direction;
        

        public override void Setup(Entity myEntity, Vector2 direction)
        {
            _overridenEntity = myEntity;
            _direction = direction;
            float rotationToTarget = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg + angleOffset;
            transform.rotation = Quaternion.AngleAxis(rotationToTarget, Vector3.forward);
        }

        /// <summary>
        /// Called from animation
        /// </summary>
        public void SpawnProjectile()
        {
            ProjectileController projectile = Instantiate(projectilePrefab);
            projectile.transform.position = offSet.position;
            projectile.Setup(_overridenEntity, _direction);
            projectile.OverrideVisuals(projectileSprite, animation);
        }
    }
}

