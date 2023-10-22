using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponController : ProjectileWeaponController
    {
        public bool IsEquipped { get; set; }

        protected bool isCurrentArm => myArm == _overridenEntity.WeaponArmController.CurrentArm;

        [SerializeField]
        private PlayerWeaponArm myArm;

        protected PlayerEntity _overridenEntity;

        protected override void Start()
        {
            MyEntity = GameManager.PlayerEntity;
            base.Start();
            _overridenEntity = MyEntity as PlayerEntity;
        }
    }
}