using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MantisWeaponController : EnemyWeaponController
    {
        public int RandomCombo => Random.Range(minCombo, maxCombo + 1);
        public WeaponStats ComboWeapon => _comboWeapon;
        public bool CanUseCombo => _comboTimer > _comboWeapon.rateOfFire.Calculated;

        
        [SerializeField]
        private int minCombo = 5;
        [SerializeField]
        private int maxCombo = 10;
        [SerializeField]
        private WeaponStats _comboWeapon = new();

        private float _comboTimer;

        protected override void Update()
        {
            base.Update();
            _comboTimer += Time.deltaTime;
        }

        public void ShootCombo()
        {
            _comboTimer = 0;
            ProjectileController melee = Instantiate(_combatStats.meleeWeaponStats.projectilePrefab);

            melee.transform.position = meleeOffset.position;
            melee.transform.rotation = PhysicsUtils.LookAt(transform, _storedTarget, 180);

            // Set weapon mode here instead of anywhere else to ensure it's the same frame as projectile setting up.
            CurrentWeaponMode = WeaponMode.Melee;

            melee.Setup(MyEntity, _comboWeapon, _storedDirection);
        }
    }
}