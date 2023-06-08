using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public enum WeaponMode
    {
        Melee,
        Projectile,
    }

    public class WeaponArmAnimationController : AnimationManager
    {
        public WeaponMode CurrentWeaponMode = WeaponMode.Projectile;

        [Header("Projectile")]
        AnimationName projectileIdle;
        AnimationName projectileEquip;
        AnimationName projectileShoot;

        [Header("Melee")]
        AnimationName meleeIdle;
        AnimationName meleeEquip;
        AnimationName meleeShoot;

        private void Start()
        {
            GameManager.EventService.Add<PlayerChangedWeaponEvent>(SwitchWeapon);
        }

        public void SwitchWeapon(PlayerChangedWeaponEvent e)
        {
            CurrentWeaponMode = e.WeaponMode;
            PlayEquipAnimation();
        }

        public void PlayIdleAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                PlayAnimation(projectileIdle, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                PlayAnimation(meleeIdle, 0);
            }
        }

        public void PlayEquipAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                PlayAnimation(projectileEquip, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                PlayAnimation(meleeEquip, 0);
            }
        }

        public void PlayShootAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                PlayAnimation(projectileShoot, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                PlayAnimation(meleeShoot, 0);
            }
        }
    }

}