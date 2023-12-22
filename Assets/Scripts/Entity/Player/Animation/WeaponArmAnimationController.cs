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

        public void PlayIdleAnimation(WeaponMode mode)
        {
            if (mode == WeaponMode.Projectile)
            {
                if (IsAnimPlaying(projectileIdle))
                {
                    return;
                }
                PlayAnimation(projectileIdle, 0);
            }
            else if (mode == WeaponMode.Melee)
            {
                if (IsAnimPlaying(meleeIdle))
                {
                    return;
                }
                PlayAnimation(meleeIdle, 0);
            }
        }
        
        private void ForceIdleAnimation(WeaponMode mode)
        {
            if (mode == WeaponMode.Projectile)
            {
                if (IsAnimPlaying(projectileIdle))
                {
                    return;
                }
                OverrideAnimation(projectileIdle, 0);
            }
            else if (mode == WeaponMode.Melee)
            {
                if (IsAnimPlaying(meleeIdle))
                {
                    return;
                }
                OverrideAnimation(meleeIdle, 0);
            }
        }

        public void PlayEquipAnimation(WeaponMode mode)
        {
            AnimationName name = new();
            if (mode == WeaponMode.Melee)
            {
                name = projectileEquip;
            }
            else if (mode == WeaponMode.Projectile)
            {
                name = meleeEquip;
            }
            StartCoroutine(PlayAnimOnce(name, mode));
        }

        public void PlayAttackAnimation(WeaponMode mode, float fireRate)
        {
            float projectileSpeedModifier = 1 / (fireRate * 0.9f);

            AnimationName name = new();
            if (mode == WeaponMode.Projectile)
            {
                name = projectileShoot;
            }
            else if (mode == WeaponMode.Melee)
            {
                name = meleeShoot;
            }
            anim.SetFloat("PlaybackSpeed", projectileSpeedModifier);
            StartCoroutine(PlayAnimOnce(name, mode));
        }

        public void PlayDieAnimation(WeaponMode mode)
        {
            if (mode == WeaponMode.Projectile)
            {
                OverrideAnimation(projectileDie, 0);
            }
            else if (mode == WeaponMode.Melee)
            {
                OverrideAnimation(meleeDie, 0);
            }
        }

        public void PlayReviveAnimation(WeaponMode mode)
        {
            if (mode == WeaponMode.Projectile)
            {
                OverrideAnimation(projectileRevive, 0);
            }
            else if (mode == WeaponMode.Melee)
            {
                OverrideAnimation(meleeRevive, 0);
            }
        }

        /// <summary>
        /// Some unskippable anims will get stuck at their last frame, this ensures they always return to idle
        /// </summary>
        IEnumerator PlayAnimOnce(AnimationName name, WeaponMode mode)
        {
            while (!IsAnimPlaying(name))
            {
                OverrideAnimation(name, 0);
                yield return new WaitForSeconds(0);
            }
            while (CurrentAnimationNomralizedTime < name.MaxBufferPercentage)
            {
                yield return new WaitForSeconds(0);
            }
            if (bufferedAnimation == null)
            {
                yield return null;
            }
            ForceIdleAnimation(mode);
        }
    }

}