using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class HammerWeaponController : MeleeWeaponController
    {
        enum ComboState
        {
            None,
            Slash1,
            Slash2,
            Slam
        }
        private float _comboTimer;
        private ComboState _comboState;
        private float combo2Time = 2f;
        private float slamComboTime = 2f;

        protected override void Update()
        {
            if (ShouldPreventUpdate())
            {
                return;
            }

            ShotTimer += Time.deltaTime;
            WeaponAbilityTimer += Time.deltaTime;
            _comboTimer += Time.deltaTime;

            if(CanShoot())
            {
                ShotTimer = 0;
                CheckComboState();
                _comboTimer = 0;
                Shoot();
            }

            if (CanUseWeaponAbility())
            {
                WeaponAbilityTimer = 0;
                EventService.Dispatch<PlayerUsedAbilityEvent>();
                UseWeaponAbility();
            }
        }

        private void CheckComboState()
        {
            if (_comboState == ComboState.Slash1)
            {
                
            }
        }
        
        protected override void Shoot()
        {
            // if (_comboState == ComboState.Slash1)
            // {
                meleeWeaponInstance.TriggerAnimation("Attack");
            // }
        }
        
        protected override void UseWeaponAbility()
        {
            meleeWeaponInstance.TriggerAnimation("Slam");
        }
    }
}