using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponArm : MonoBehaviour
    {
        public SpriteRenderer MySpriteRenderer;
        public float StartRotation;
        public float MinRotation;
        public float MaxRotation;
        public WeaponArmAnimationController AnimationController;

        public Transform MeleeOrigin;
        public Transform ProjectileOrigin;

        public PlayerWeaponController WeaponController;


        [SerializeField]
        private float returnToIdleSpeed = 2;

        private void Start()
        {
            SetupWeaponControllers();
        }

        private void SetupWeaponControllers()
        {
            Platform.EventService.Add<PlayerChangedWeaponEvent>(SwitchWeapons);
            Platform.EventService.Add<PlayerDiedEvent>(Die);
            Platform.EventService.Add<PlayerRevivedEvent>(Revive);
        }

        public void Die()
        {
            AnimationController.PlayDieAnimation(WeaponController.CurrentWeaponMode);
        }

        public void Revive()
        {
            AnimationController.PlayReviveAnimation(WeaponController.CurrentWeaponMode);
        }

        public void SwitchWeapons(PlayerChangedWeaponEvent e)
        {
            AnimationController.PlayEquipAnimation(e.NewWeaponMode);
        }

        public void Attack(float fireRate)
        {
            AnimationController.PlayAttackAnimation(WeaponController.CurrentWeaponMode, fireRate);
        }

        public void ReturnToIdle()
        {
            StartCoroutine(RotateTowardsZero());
        }

        IEnumerator RotateTowardsZero()
        {
            while (transform.eulerAngles.z != 0)
            {
                transform.rotation = PhysicsUtils.LookAt(transform, transform.position, 0, returnToIdleSpeed * Time.deltaTime);
                yield return null;
            }
        }

        public void MeleeShoot()
        {
            WeaponController.Shoot();
        }

        public void ProjectileShoot()
        {
            WeaponController.Melee();
        }
    }
}