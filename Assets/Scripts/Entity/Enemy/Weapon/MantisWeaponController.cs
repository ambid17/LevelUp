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

        private WeaponStats _comboWeapon = new();

        private float _comboTimer;
        protected override void Update()
        {
            base.Update();
            _comboTimer += Time.deltaTime;
        }

        public void ShootCombo()
        {

        }

        // Called by animator to ensure cooldown gets called appropriately.
        public void ResetCombo()
        {
            _comboTimer = 0;
        }
    }
}