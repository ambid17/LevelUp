using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponArm : MonoBehaviour
    {
        public PlayerWeaponController EquippedWeapon => _equippedWeaponController ?? GetComponent<PlayerProjectileWeaponController>();

        public SpriteRenderer MySpriteRenderer;
        public float StartRotation;
        public float MinRotation;
        public float MaxRotation;
        public WeaponArmAnimationController AnimationController;

        public Transform MeleeOrigin;
        public Transform ProjectileOrigin;


        [SerializeField]
        private float returnToIdleSpeed = 2;

        private PlayerWeaponController _equippedWeaponController;
        private PlayerProjectileWeaponController _projectileWeaponController;
        private PlayerMeleeWeaponController _meleeWeaponController;

        private void Start()
        {
            SetupWeaponControllers();
        }

        private void SetupWeaponControllers()
        {
            _projectileWeaponController = GetComponent<PlayerProjectileWeaponController>();
            _meleeWeaponController = GetComponent<PlayerMeleeWeaponController>();
            _equippedWeaponController = _projectileWeaponController;
            _equippedWeaponController.IsEquipped = true;
            Platform.EventService.Add<PlayerDiedEvent>(Die);
            Platform.EventService.Add<PlayerRevivedEvent>(Revive);
        }

        public void Die()
        {
            AnimationController.PlayDieAnimation();
        }

        public void Revive()
        {
            AnimationController.PlayReviveAnimation();
        }

        private void Update()
        {
            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            if (AnimationController.IsAnimFinished)
            {
                AnimationController.PlayIdleAnimation();
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                AnimationController.PlayEquipAnimation();
            }
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

        public void ChangeEquippedWeapon()
        {
            _equippedWeaponController.IsEquipped = false;
            _equippedWeaponController = _equippedWeaponController == _projectileWeaponController ? _meleeWeaponController : _projectileWeaponController;
            _equippedWeaponController.IsEquipped = true;
        }

        public void MeleeShoot()
        {
            _meleeWeaponController.Shoot();
        }

        public void ProjectileShoot()
        {
            _projectileWeaponController.Shoot();
        }
    }
}