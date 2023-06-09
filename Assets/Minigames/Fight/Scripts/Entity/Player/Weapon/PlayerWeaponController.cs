using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponController : ProjectileWeaponController
    {
        public bool IsEquipped { get; set; }

        private PlayerEntity _overridenEntity;

        protected override void Start()
        {
            MyEntity = GameManager.PlayerEntity;
            base.Start();
            _overridenEntity = MyEntity as PlayerEntity;
        }
        protected void TryShoot()
        {
            _overridenEntity.WeaponArmController.Playshoot();
        }
    }
}