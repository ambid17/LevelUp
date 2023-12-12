using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigames.Fight
{
    public class PlayerWeaponController : WeaponController
    {
        public WeaponMode _currentWeaponMode = WeaponMode.Projectile;

        protected PlayerEntity _overridenEntity;

        public PlayerWeaponArm CurrentArm => _currentArm ?? leftArm;

        [SerializeField]
        private PlayerWeaponArm leftArm;
        [SerializeField]
        private PlayerWeaponArm rightArm;
        [SerializeField]
        private PlayerProjectile projectilePrefab;

        private PlayerWeaponArm _currentArm;
        private Camera _cam;

        private int _leftSortingOrder = 0;
        private int _rightSortingOrder = 0;

        protected override void Start()
        {
            MyEntity = GameManager.PlayerEntity;
            base.Start();
            _overridenEntity = MyEntity as PlayerEntity;

            _currentArm = leftArm;
            _cam = GameManager.PlayerEntity.PlayerCamera;

            Platform.EventService.Add<PlayerChangedDirectionEvent>(SwitchDirection);
        }

        protected override void Update()
        {
            base.Update();

            if (GameManager.PlayerEntity.IsDead)
            {
                return;
            }
            ControlArms();
            TryShoot();
            TryMelee();
            MyEntity.Stats.combatStats.projectileWeaponStats.TryRegenAmmo();
        }

        private void ControlArms()
        {
            leftArm.MySpriteRenderer.sortingOrder = _leftSortingOrder;
            rightArm.MySpriteRenderer.sortingOrder = _rightSortingOrder;
            float currentRotation = _currentArm.transform.rotation.eulerAngles.z;
            if (currentRotation < _currentArm.MinRotation && currentRotation > _currentArm.MaxRotation)
            {
                _currentArm.ReturnToIdle();
                _currentArm = SwitchArms();
                _currentArm.StopAllCoroutines();
            }

            _currentArm.transform.rotation = PhysicsUtils.LookAt(transform, _cam.ScreenToWorldPoint(Input.mousePosition), _currentArm.StartRotation);
        }

        private void TryShoot()
        {
            if (_currentWeaponMode == WeaponMode.Melee)
            {
                return;
            }
            CurrentArm.Attack(MyEntity.Stats.combatStats.projectileWeaponStats.rateOfFire.Calculated);
        }

        private void TryMelee()
        {
            if (_currentWeaponMode == WeaponMode.Projectile)
            {
                return;
            }
            CurrentArm.Attack(MyEntity.Stats.combatStats.meleeWeaponStats.rateOfFire.Calculated);
        }

        public void SwitchDirection(PlayerChangedDirectionEvent e)
        {
            int baseLayer = GameManager.PlayerEntity.VisualController.SpriteRenderer.sortingOrder;

            switch (e.NewDirection)
            {
                case Direction.Down:
                    _leftSortingOrder = baseLayer + 1;
                    _rightSortingOrder = baseLayer + 1;
                    break;
                case Direction.Up:
                    _leftSortingOrder = baseLayer - 1;
                    _rightSortingOrder = baseLayer - 1;
                    break;
                case Direction.Left:
                    _leftSortingOrder = baseLayer - 1;
                    _rightSortingOrder = baseLayer + 1;
                    break;
                case Direction.Right:
                    _leftSortingOrder = baseLayer + 1;
                    _rightSortingOrder = baseLayer - 1;
                    break;
            }
        }

        private PlayerWeaponArm SwitchArms()
        {
            if (_currentArm == leftArm)
            {
                return rightArm;
            }
            return leftArm;
        }

        protected override bool CanShoot()
        {
            return ShotTimer >= MyEntity.Stats.combatStats.projectileWeaponStats.rateOfFire.Calculated && Input.GetKey(KeyCode.Mouse0) && MyEntity.Stats.combatStats.projectileWeaponStats.maxAmmo.Calculated > 0;
        }
        protected override bool CanMelee()
        {
            return MeleeTimer >= MyEntity.Stats.combatStats.meleeWeaponStats.rateOfFire.Calculated && Input.GetKey(KeyCode.Mouse0);
        }

        public override void Shoot()
        {
            for (int i = 0; i < MyEntity.Stats.combatStats.projectileWeaponStats.projectilesPerShot.Calculated; i++)
            {
                PlayerProjectile projectile = Instantiate(projectilePrefab);

                Vector2 direction = GameManager.PlayerEntity.PlayerCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

                // Map the indices to start from the leftmost projectile and spawn them to the right using the offset
                float indexOffset = (float)i - i / 2;
                Vector2 offset = Vector2.Perpendicular(direction).normalized * indexOffset * MyEntity.Stats.combatStats.projectileWeaponStats.projectileSpread.Calculated;

                projectile.transform.position = _overridenEntity.WeaponArmController.CurrentArm.ProjectileOrigin.position.AsVector2() + offset;

                projectile.Setup(MyEntity, direction);
            }
        }
    }
}