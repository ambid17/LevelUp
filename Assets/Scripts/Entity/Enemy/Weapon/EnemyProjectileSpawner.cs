using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyProjectileSpawner : EnemyProjectile
    {
        public Transform Offset => offSet;

        [SerializeField]
        private Animator anim;
        [SerializeField]
        private Transform offSet;
        [SerializeField]
        private EnemyProjectile projectilePrefab;
        [SerializeField]
        private Sprite projectileSprite;

        private Entity _overridenEntity;
        private Vector2 _direction;
        private WeaponController _controller;

        protected override bool ShouldDie()
        {
            return anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
        }

        protected override void Move()
        {
            return;
        }

        public override void Setup(Entity myEntity, Vector2 direction, WeaponController controller)
        {
            _overridenEntity = myEntity;
            _direction = direction;
            _controller = controller;
            float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 180f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public void SpawnProjectile()
        {
            EnemyProjectile projectile = Instantiate(projectilePrefab);
            projectile.transform.position = offSet.position;
            projectile.MySpriteRenderer.sprite = projectileSprite;
            projectile.Setup(_overridenEntity, _direction, _controller);
        }
    }
}

