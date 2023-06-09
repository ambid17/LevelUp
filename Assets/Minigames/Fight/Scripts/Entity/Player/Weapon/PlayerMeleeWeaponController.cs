using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerMeleeWeaponController : PlayerWeaponController
    {
        
        protected override bool CanShoot()
        {
            return Input.GetKey(KeyCode.Mouse0) && IsEquipped;
        }
        protected override void Update()
        {
            base.Update();
            if (CanShoot())
            {
                Debug.Log("melee shot");
                TryShoot();
            }
        }
        public override void Shoot()
        {

        }
    }
}