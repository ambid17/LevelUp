using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class EnemyMultiAttackController : MonoBehaviour
    {
        [SerializeField]
        private MeleeEnemyWeaponController meleeWeapon;
        [SerializeField]
        private EnemyProjectileWeaponController projectileWeapon;

        public void ShootMelee()
        {
            meleeWeapon.Shoot();
        }
        public void ShootProjectile()
        {
            projectileWeapon.Shoot();
        }
    }
}