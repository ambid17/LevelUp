using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
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

        private Entity _overridenEntity;
        private Vector2 _direction;

        public override void Setup(Entity myEntity, Vector2 direction)
        {
            _overridenEntity = myEntity;
            _direction = direction;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 180f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void SpawnProjectile()
        {
            ProjectileController projectile = Instantiate(projectilePrefab);
            projectile.transform.position = offSet.position;
            projectile.SetSprite(projectileSprite);
            projectile.Setup(_overridenEntity, _direction);
        }
    }
}

