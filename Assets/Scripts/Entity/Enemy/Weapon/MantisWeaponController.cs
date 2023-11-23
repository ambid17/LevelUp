using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class MantisWeaponController : MeleeEnemyWeaponController
    {
        public int RandomCombo => Random.Range(minCombo, maxCombo + 1);
        public MeleeWeapon ComboWeapon => comboWeapon;
        public bool CanUseCombo => _comboTimer > comboWeapon.fireRate;

        [SerializeField]
        private MeleeWeapon comboWeapon;
        [SerializeField]
        private int minCombo = 5;
        [SerializeField]
        private int maxCombo = 10;

        private float _comboTimer;
        private HitData _comboHitData;
        protected override void Update()
        {
            base.Update();
            _comboTimer += Time.deltaTime;
        }

        public override void CalculateHitData()
        {
            base.CalculateHitData();
            _comboHitData = new HitData(MyEntity, comboWeapon.damage);
        }

        public void ShootCombo()
        {
            ShotTimer = 0;
            if (Vector2.Distance(transform.position, GameManager.PlayerEntity.transform.position) > comboWeapon.attackRange)
            {
                return;
            }
            GameManager.PlayerEntity.TakeHit(_comboHitData);
        }
    }
}