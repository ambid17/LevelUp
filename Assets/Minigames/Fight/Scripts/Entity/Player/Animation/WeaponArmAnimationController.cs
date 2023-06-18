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
        [SerializeField]
        private AnimationName projectileIdle;
        [SerializeField]
        private AnimationName projectileEquip;
        [SerializeField]
        private AnimationName projectileShoot;
        [SerializeField]
        private AnimationName projectileDie;
        [SerializeField]
        private AnimationName projectileRevive;
        
        [Header("Melee")]
        [SerializeField]
        private AnimationName meleeIdle;
        [SerializeField]
        private AnimationName meleeEquip;
        [SerializeField]
        private AnimationName meleeShoot;
        [SerializeField]
        private AnimationName meleeDie;
        [SerializeField]
        private AnimationName meleeRevive;

        [SerializeField]
        private PlayerWeaponArm arm;

        private bool _isAttemptingEquip;

        public void PlayIdleAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                if (IsAnimPlaying(projectileIdle))
                {
                    return;
                }
                PlayAnimation(projectileIdle, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                if (IsAnimPlaying(meleeIdle))
                {
                    return;
                }
                PlayAnimation(meleeIdle, 0);
            }
        }

        public void PlayEquipAnimation()
        {
            AnimationName name = new();
            if (CurrentWeaponMode == WeaponMode.Melee)
            {
                name = projectileEquip;
            }
            else if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                name = meleeEquip;
            }
            StartCoroutine(ChangeWeapons(name));
        }

        public void PlayShootAnimation()
        {
            AnimationName storedCurrentAnim = currentAnimation;
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                PlayAnimation(projectileShoot, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                PlayAnimation(meleeShoot, 0);
            }
            if (_isAttemptingEquip)
            {
                QueAnimation(storedCurrentAnim);
            }
        }

        public void PlayDieAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                OverrideAnimation(projectileDie, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                OverrideAnimation(meleeDie, 0);
            }
        }

        public void PlayReviveAnimation()
        {
            if (CurrentWeaponMode == WeaponMode.Projectile)
            {
                OverrideAnimation(projectileRevive, 0);
            }
            else if (CurrentWeaponMode == WeaponMode.Melee)
            {
                OverrideAnimation(projectileRevive, 0);
            }
        }

        IEnumerator ChangeWeapons(AnimationName name)
        {
            _isAttemptingEquip = true;
            PlayAnimation(name, 0);
            while (!IsAnimPlaying(name))
            {
                yield return null;
            }
            while (CurrentAnimationNomralizedTime < projectileShoot.AcceptableOverrideTime)
            {
                yield return null;
            }
            CurrentWeaponMode = CurrentWeaponMode == WeaponMode.Projectile ? WeaponMode.Melee : WeaponMode.Projectile;
            arm.ChangeEquippedWeapon();
            _isAttemptingEquip = false;
        }
    }

}